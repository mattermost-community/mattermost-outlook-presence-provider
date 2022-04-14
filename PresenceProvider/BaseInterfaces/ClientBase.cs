using System;
using UCCollaborationLib;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OutlookPresenceProvider
{
    // Note: LyncClient inherits from both ILyncClient and _ILyncClientEvents_Event
    // You must implement LyncClient because the event handlers in _ILyncClientEvents expect you to pass a LyncClient interface.
    [ComVisible(true)]
    [ComSourceInterfaces(typeof(_ILyncClientEvents))]
    public class ClientBase :
        Client,
        LyncClient
    {
        #region Constructors
        public ClientBase()
        {
            _clientState = ClientState.ucClientStateSignedIn;
            // TODO: Make this dynamic.
            _uri = "shivam.chauhan@brightscoutdev.onmicrosoft.com";
            _self = new IMClientSelf(new IMClientContact(_uri));
            _contactManager = new IMClientContactManager();
            _conversationManager = new IMClientConversationManager();
        }
        #endregion

        #region Interfaces
        private IMClientContactManager _contactManager;
        public ContactManager ContactManager
        {
            get
            {
                return _contactManager;
            }
        }

        private ConversationManager _conversationManager;
        public ConversationManager ConversationManager
        {
            get
            {
                return _conversationManager;
            }
        }

        private IMClientSelf _self;
        public Self Self
        {
            get
            {
                return _self;
            }
        }

        private ClientState _clientState = ClientState.ucClientStateUninitialized;
        public ClientState State
        {
            get
            {
                return _clientState;
            }
        }

        private string _uri = "";
        public string Uri
        {
            get
            {
                return _uri;
            }
        }

        public LyncClientCapabilityTypes Capabilities => throw new NotImplementedException();

        public ConferenceScheduler ConferenceScheduler => throw new NotImplementedException();

        public DelegatorClient[] DelegatorClients => throw new NotImplementedException();

        public DeviceManager DeviceManager => throw new NotImplementedException();

        public bool InSuppressedMode => throw new NotImplementedException();

        public ContactManager PrivateContactManager => throw new NotImplementedException();

        public RoomManager RoomManager => throw new NotImplementedException();

        public ClientSettings Settings => throw new NotImplementedException();

        public SignInConfiguration SignInConfiguration => throw new NotImplementedException();

        public ClientType Type => throw new NotImplementedException();


        public Utilities Utilities => throw new NotImplementedException();

        // This field is of a type that implements the 
        // IAsynchronousOperation interface.
        private IMClientAsyncOperation _asyncOperation = new IMClientAsyncOperation();
        public AsynchronousOperation SignIn(string _userUri, string _domainAndUser,
            string _password, object _IMClientCallback, object _state)
        {
            ClientState _previousClientState = _clientState;
            _clientState = ClientState.ucClientStateSignedIn;
            // The IMClientStateChangedEventData class implements the 
            // IClientStateChangedEventData interface.
            IMClientStateChangedEventData eventData =
                new IMClientStateChangedEventData(_previousClientState,
                _clientState);
            if (_userUri != null)
            {
                // During the sign-in process, create a new contact with
                // the contact information of the currently signed-in user.
                _self = new IMClientSelf(new IMClientContact(_userUri));
            }
            // Raise the _ILyncClientEvents.OnStateChanged event.
            RaiseOnStateChangedEvent(eventData);
            return _asyncOperation;
        }


        public ApplicationRegistration CreateApplicationRegistration(string _appGuid, string _appName)
        {
            throw new NotImplementedException();
        }
        public AsynchronousOperation Initialize(string _clientName, string _version = "0", string _clientShortName = "0", string _clientNameAbbreviation = "0", string _clientLongName = "0", SupportedFeatures _supportedFeatures = SupportedFeatures.ucAllFeatures, [IUnknownConstant] object _CommunicatorClientCallback = null, object _state = null)
        {
            throw new NotImplementedException();
        }
        public AsynchronousOperation Shutdown([IUnknownConstant] object _CommunicatorClientCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation SignOut([IUnknownConstant] object _CommunicatorClientCallback, object _state)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region _ILyncClientEvents support
        public event _ILyncClientEvents_OnStateChangedEventHandler OnStateChanged;
        public event _ILyncClientEvents_OnNotificationReceivedEventHandler OnNotificationReceived;
        public event _ILyncClientEvents_OnCredentialRequestedEventHandler OnCredentialRequested;
        public event _ILyncClientEvents_OnSignInDelayedEventHandler OnSignInDelayed;
        public event _ILyncClientEvents_OnCapabilitiesChangedEventHandler OnCapabilitiesChanged;
        public event _ILyncClientEvents_OnDelegatorClientAddedEventHandler OnDelegatorClientAdded;
        public event _ILyncClientEvents_OnDelegatorClientRemovedEventHandler OnDelegatorClientRemoved;
        // Notifies Office apps that the IM client state (signed out, signing in, singed in, signing out, etc) has changed.
        internal void RaiseOnStateChangedEvent(ClientStateChangedEventData eventData)
        {
            if (OnStateChanged != null)
            {
                OnStateChanged(this, eventData);
            }
        }
        // Notifies Office apps that the IM client has received a notification event from MAPI (e.g. autodiscover has finished)
        internal void RaiseOnNotificationReceivedEvent(LyncClientNotificationReceivedEventData eventData)
        {
            if (OnNotificationReceived != null)
            {
                OnNotificationReceived(this, eventData);
            }
        }
        // Notifies Office apps that the IM client has received a request for credentials for some operation (e.g. sign in, web search)
        internal void RaiseOnCredentialRequestedEvent(CredentialRequestedEventData eventData)
        {
            if (OnCredentialRequested != null)
            {
                OnCredentialRequested(this, eventData);
            }
        }
        // Notifies Office apps that the IM client has been delayed from signing in and gives an estimated delay time.
        internal void RaiseOnSignInDelayedEvent(SignInDelayedEventData eventData)
        {
            if (OnSignInDelayed != null)
            {
                OnSignInDelayed(this, eventData);
            }
        }
        // Notifies Office apps that the capabilities of this IM client have changed.
        internal void RaiseOnCapabilitiesChangedEvent(PreferredCapabilitiesChangedEventData eventData)
        {
            if (OnCapabilitiesChanged != null)
            {
                OnCapabilitiesChanged(this, eventData);
            }
        }
        // Notifies Office apps that a DelegatorClient object has been added to the IM client object.
        internal void RaiseOnDelegatorClientAdded(DelegatorClientCollectionEventData eventData)
        {
            if (OnDelegatorClientAdded != null)
            {
                OnDelegatorClientAdded(this, eventData);
            }
        }
        // Notifies Office apps that a DelegatorClient object has been removed from the IM client object.
        internal void RaiseOnDelegatorClientRemoved(DelegatorClientCollectionEventData eventData)
        {
            if (OnDelegatorClientRemoved != null)
            {
                OnDelegatorClientRemoved(this, eventData);
            }
        }
        #endregion
    }
}
