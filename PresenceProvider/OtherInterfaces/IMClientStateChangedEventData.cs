using UCCollaborationLib;
using System.Runtime.InteropServices;

namespace OutlookPresenceProvider
{
    [ComVisible(true)]
    public class IMClientStateChangedEventData : ClientStateChangedEventData
    {
        public IMClientStateChangedEventData(ClientState oldState, ClientState newState)
        {
            _oldState = oldState;
            _newState = newState;
        }
        private ClientState _newState;
        public ClientState NewState
        {
            get => _newState;
        }

        private ClientState _oldState;
        public ClientState OldState
        {
            get => _oldState;
        }

        private int _statusCode;
        public int StatusCode
        {
            get => _statusCode;
        }
    }
}
