﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
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
            InitializeStore();
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
        private Store _store;
        public Store Store
        {
            get => _store;
        }

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
                Trace.WriteLine("Server URL or Secret cannot be empty.");
                throw new Exception("Server URL or Secret cannot be empty.");
            }
            _pluginUrl = new Uri($"{_serverUrl}/plugins/{Constants.PluginId}/api/v1/");
            _wsServerUrl = new UriBuilder(_pluginUrl);
            _wsServerUrl.Scheme = _pluginUrl.Scheme == "https" ? "wss" : "ws";
        }
        
        private void InitializeStore()
        {
            try
            {
                _store = new Store();
                UriBuilder reqUrl = new UriBuilder(_pluginUrl);
                reqUrl.Path += "status";
                int page = 0;
                AddQueryParamsToUrl(reqUrl, Constants.MattermostRequestParamSecret, _secret);
                while (true)
                {
                    AddQueryParamsToUrl(reqUrl, Constants.MattermostRequestParamPage, page.ToString());
                    JsonArray response = JsonNode.Parse(_client.GetStringAsync(reqUrl.Uri).GetAwaiter().GetResult()).AsArray();
                    if (response.Count == 0)
                    {
                        break;
                    }

                    foreach (JsonNode user in response)
                    {
                        _store.Add(user[Constants.MattermostEmail].GetValue<string>(), user[Constants.MattermostStatus].GetValue<string>());
                    }
                    page++;
                }
            } catch (Exception ex)
            {
                Trace.WriteLine(ex.StackTrace);
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
                Trace.WriteLine("Websocket client connected.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("ERROR: " + ex.ToString());
            }
        }

        private void InitializeWebsocketClient()
        {
            _wsServerUrl.Path += "ws";
            AddQueryParamsToUrl(_wsServerUrl, Constants.MattermostRequestParamSecret, _secret);
            var client = new WebsocketClient(_wsServerUrl.Uri);

            // The client will disconnect and reconnect if there is no message from
            // the server in 60 seconds.
            client.ReconnectTimeout = TimeSpan.FromSeconds(Constants.WebsocketReconnectionTimeoutInSeconds);
            client.ReconnectionHappened.Subscribe(info =>
            {
                Trace.WriteLine("Reconnection happened, type: " + info.Type);
            });

            client.DisconnectionHappened.Subscribe(info =>
            {
                Trace.WriteLine("Disconnection happened, type: " + info.Type);
                if (info.Type == DisconnectionType.Error || info.Type == DisconnectionType.ByServer)
                {
                    throw new Exception("Error in connecting to websocket server.");
                }
            });

            
            client.Start();
            _wsClient = client;
            mre.WaitOne();
            
            Trace.WriteLine("Websocket client closed.");
        }

        private string GetValueFromConfig(string key)
        {
            try
            {
                string myFile = $"{Directory.GetCurrentDirectory()}\\config.json";
                // Checking the config.json file
                if (!File.Exists(myFile))
                {
                    using (StreamWriter sw = File.CreateText(myFile))
                    {
                        sw.WriteLine($"{{\"{Constants.MattermostServerURL}\": \"\", \"{Constants.MattermostSecret}\": \"\"}}");
                    }
                    return "";
                }

                JsonNode configNode = JsonNode.Parse(File.ReadAllText(myFile));
                return configNode[key].GetValue<string>();
            } catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
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
