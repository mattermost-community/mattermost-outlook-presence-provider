using System;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Win32;
using System.Text.Json.Nodes;
using UCCollaborationLib;
using Websocket.Client;
using System.Net.WebSockets;

namespace OutlookPresenceProvider.Mattermost
{
    public class Client
    {
        public Client()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _serverUrl = GetServerUrlFromRegistry();
            _pluginUrl = new Uri($"{_serverUrl}/plugins/{Constants.PluginId}/api/v1/");
            _wsServerUrl = new UriBuilder(_pluginUrl);
            _wsServerUrl.Scheme = _pluginUrl.Scheme == "https" ? "wss" : "ws";
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
        private Uri _pluginUrl;
        private UriBuilder _wsServerUrl;

        // mre is used to block and release threads manually.
        // It is created in the unsignaled state.
        private ManualResetEvent mre = new ManualResetEvent(false);
        private WebsocketClient _wsClient;
        public WebsocketClient WsClient => _wsClient;
        
        private string GetServerUrlFromRegistry()
        {
            string serverUrl = "";
            using (RegistryKey IMProviders = Registry.CurrentUser.OpenSubKey("SOFTWARE\\IM Providers", true))
            {
                using (RegistryKey IMProvider = IMProviders.CreateSubKey(PresenceProvider.COMAppExeName))
                {
                    serverUrl = (string)IMProvider.GetValue(Constants.MattermostServerURL);
                }
            }
            return serverUrl;
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
                string reqUri = $"{_pluginUrl}/status/{email}";
                JsonNode statusNode = JsonNode.Parse(_client.GetStringAsync(reqUri).GetAwaiter().GetResult());
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
            var url = new Uri(_wsServerUrl.Uri, "ws");

            var client = new WebsocketClient(url);

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
            });

            
            client.Start();
            _wsClient = client;
            mre.WaitOne();
            
            Console.WriteLine("Websocket client closed.");
        }
    }
}
