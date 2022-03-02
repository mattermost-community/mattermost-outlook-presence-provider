using System;
using UCCollaborationLib;
using System.Runtime.CompilerServices;

namespace OutlookPresenceProvider
{
    // Note: LyncClient inherits from both ILyncClient and _ILyncClientEvents_Event
    // You must implement LyncClient because the event handlers in _ILyncClientEvents expect you to pass a LyncClient interface.
    public class ClientBase :
        Client,
        Client2,
        LyncClient
    {
        #region Interfaces
        private ContactManager _contactManager;
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

        private Self _iself;
        public Self Self
        {
            get
            {
                return _iself;
            }
        }

        private ClientState _clientState;
        public ClientState State
        {
            get
            {
                return _clientState;
            }
        }

        private string _uri;
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
        // This field is of a type that implements the ISelf interface.
        private IMClientSelf _self;
        public AsynchronousOperation SignIn(string _userUri, string _domainAndUser,
            string _password, object _IMClientCallback, object _state)
        {
            ClientState _previousClientState = this._clientState;
            this._clientState = ClientState.ucClientStateSignedIn;
            // The IMClientStateChangedEventData class implements the 
            // IClientStateChangedEventData interface.
            IMClientStateChangedEventData eventData =
                new IMClientStateChangedEventData(_previousClientState,
                this._clientState);
            if (_userUri != null)
            {
                // During the sign-in process, create a new contact with
                // the contact information of the currently signed-in user.

                // TODO: Uncomment and correct this
                //this._self = new IMClientSelf(IMContact.BuildContact(_userUri));
            }
            // Raise the _ILyncClientEvents.OnStateChanged event.
            OnStateChanged(this, eventData as ClientStateChangedEventData);

            return this._asyncOperation;
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
            if (this.OnStateChanged != null)
            {
                this.OnStateChanged(this, eventData);
            }
        }
        // Notifies Office apps that the IM client has received a notification event from MAPI (e.g. autodiscover has finished)
        internal void RaiseOnNotificationReceivedEvent(LyncClientNotificationReceivedEventData eventData)
        {
            if (this.OnNotificationReceived != null)
            {
                this.OnNotificationReceived(this, eventData);
            }
        }
        // Notifies Office apps that the IM client has received a request for credentials for some operation (e.g. sign in, web search)
        internal void RaiseOnCredentialRequestedEvent(CredentialRequestedEventData eventData)
        {
            if (this.OnCredentialRequested != null)
            {
                this.OnCredentialRequested(this, eventData);
            }
        }
        // Notifies Office apps that the IM client has been delayed from signing in and gives an estimated delay time.
        internal void RaiseOnSignInDelayedEvent(SignInDelayedEventData eventData)
        {
            if (this.OnSignInDelayed != null)
            {
                this.OnSignInDelayed(this, eventData);
            }
        }
        // Notifies Office apps that the capabilities of this IM client have changed.
        internal void RaiseOnCapabilitiesChangedEvent(PreferredCapabilitiesChangedEventData eventData)
        {
            if (this.OnCapabilitiesChanged != null)
            {
                this.OnCapabilitiesChanged(this, eventData);
            }
        }
        // Notifies Office apps that a DelegatorClient object has been added to the IM client object.
        internal void RaiseOnDelegatorClientAdded(DelegatorClientCollectionEventData eventData)
        {
            if (this.OnDelegatorClientAdded != null)
            {
                this.OnDelegatorClientAdded(this, eventData);
            }
        }
        // Notifies Office apps that a DelegatorClient object has been removed from the IM client object.
        internal void RaiseOnDelegatorClientRemoved(DelegatorClientCollectionEventData eventData)
        {
            if (this.OnDelegatorClientRemoved != null)
            {
                this.OnDelegatorClientRemoved(this, eventData);
            }
        }
        #endregion
    }


    public class AutomationBase : IAutomation
    {
        public ConversationWindow GetConversationWindow(Conversation _conversation)
        {
            throw new NotImplementedException();
        }

        public ConversationWindow JoinConference(string _conferenceUrl)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation JoinConferenceEx(string _conferenceUrl, long _parentHWND = 0, [IUnknownConstant] object _callback = null, object _state = null)
        {
            throw new NotImplementedException();
        }

        public void LaunchAddContactWizard(string _contactEmail = "0")
        {
            throw new NotImplementedException();
        }

        public ConversationWindow StartConversation(AutomationModalities _conversationModes, string[] _participantUris, AutomationModalitySettings[] _contextTypes, object[] _contextDatas)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation StartConversationEx(AutomationModalities _conversationModes, string[] _participantUris, AutomationModalitySettings[] _contextTypes, object[] _contextDatas, [IUnknownConstant] object s_callback, object _state)
        {
            throw new NotImplementedException();
        }
    }

}
