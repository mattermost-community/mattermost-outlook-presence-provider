﻿using System;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using UCCollaborationLib;
using Websocket.Client;
using System.Net.WebSockets;
using System.Web;
using System.IO;

namespace OutlookPresenceProvider.Mattermost
{
    public class Client
    {
        public Client()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            UpdateURLs();
            InitializeWebsocketClientInNewThread();
        }

        ~Client()
        {
            _wsClient?.Stop(WebSocketCloseStatus.NormalClosure, "Connection closed.");
            _wsClient?.Dispose();
            mre.Set();
            mre.Close();
        }

        private HttpClient _client;
        private string _serverUrl;
        private string _secret;
        private Uri _pluginUrl;
        private UriBuilder _wsServerUrl;
        
        // mre is used to block and release threads manually.
        // It is created in the unsignaled state.
        private ManualResetEvent mre = new ManualResetEvent(false);
        private WebsocketClient _wsClient;
        public WebsocketClient WsClient => _wsClient;
        
        private void UpdateURLs()
        {
            _secret = GetValueFromConfig(Constants.MattermostSecret);
            _serverUrl = GetValueFromConfig(Constants.MattermostServerURL);
            if (_secret == "" || _serverUrl == "")
            {
                Console.WriteLine("Invalid server url or secret.");
                throw new Exception("Invalid server url or secret.");
            }
            _pluginUrl = new Uri($"{_serverUrl}/plugins/{Constants.PluginId}/api/v1/");
            _wsServerUrl = new UriBuilder(_pluginUrl);
            _wsServerUrl.Scheme = _pluginUrl.Scheme == "https" ? "wss" : "ws";
        }
        
        public ContactAvailability GetAvailabilityFromMattermost(string email)
        {
            try
            {
                if (_serverUrl == "")
                {
                    // We will not be using this value from the registry so just log the error for now
                    Console.WriteLine("Invalid server url");
                    return ContactAvailability.ucAvailabilityNone;
                }

                UriBuilder reqUrl = new UriBuilder(_pluginUrl);
                reqUrl.Path += $"status/{email}";
                AddQueryParamsToUrl(reqUrl, "secret", _secret);
                JsonNode statusNode = JsonNode.Parse(_client.GetStringAsync(reqUrl.Uri).GetAwaiter().GetResult());
                return Constants.StatusAvailabilityMap(statusNode["status"].GetValue<string>());
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return ContactAvailability.ucAvailabilityNone;
            }
        }

        private void InitializeWebsocketClientInNewThread()
        {
            try
            {
                // Start the websocket client in a new thread
                Thread t = new Thread(InitializeWebsocketClient);
                t.Start();

                // Wait for the websocket client "_wsClient" to get initialized by the other thread running parallely
                while (_wsClient == null) ;
                Console.WriteLine("Websocket client connected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
            }
        }

        private void InitializeWebsocketClient()
        {
            _wsServerUrl.Path += "ws";
            AddQueryParamsToUrl(_wsServerUrl, "secret", _secret);
            var client = new WebsocketClient(_wsServerUrl.Uri);

            // The client will disconnect and reconnect if there is no message from
            // the server in 60 seconds.
            client.ReconnectTimeout = TimeSpan.FromSeconds(Constants.WebsocketReconnectionTimeoutInSeconds);
            client.ReconnectionHappened.Subscribe(info =>
            {
                Console.WriteLine("Reconnection happened, type: " + info.Type);
            });

            client.DisconnectionHappened.Subscribe(info =>
            {
                Console.WriteLine("Disconnection happened, type: " + info.Type);
                if (info.Type == DisconnectionType.Error || info.Type == DisconnectionType.ByServer)
                {
                    throw new Exception("Error in connecting to websocket server.");
                }
            });

            
            client.Start();
            _wsClient = client;
            mre.WaitOne();
            
            Console.WriteLine("Websocket client closed.");
        }

        private string GetValueFromConfig(string key)
        {
            try
            {
                string myfile = $"{Directory.GetCurrentDirectory()}\\config.json";
                // Checking the above file
                if (!File.Exists(myfile))
                {
                    using (StreamWriter sw = File.CreateText(myfile))
                    {
                        sw.WriteLine("{\"MattermostServerURL\": \"\", \"MattermostSecret\": \"\"}");
                    }
                    return "";
                }

                JsonNode configNode = JsonNode.Parse(File.ReadAllText(myfile));
                string val = configNode[key].GetValue<string>();
                return val;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        private void AddQueryParamsToUrl(UriBuilder url, string paramName, string paramValue)
        {
            var query = HttpUtility.ParseQueryString(url.Query);
            query[paramName] = paramValue;
            url.Query = query.ToString();
        }
    }
}