using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Net.Http;
using UCCollaborationLib;
using Microsoft.Win32;
using System.Text.Json.Nodes;

namespace OutlookPresenceProvider
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(_IContactEvents))]
    [ComVisible(true)]
    public class IMClientContact: Contact
    {
        public IMClientContact()
        {
            _settingDictionary = new IMClientContactSettingDictionary();
            _groupCollection = new IMClientGroupCollection();
        }

        public IMClientContact(string uri) : this()
        {
            _uri = uri;
        }

        private IMClientContactSettingDictionary _settingDictionary;
        public ContactSettingDictionary Settings
        {
            get
            {
                // The IMClientContactSettingDictionary class implements
                // the IContactSettingDictionary interface.
                return _settingDictionary;
            }
        }

        private IMClientGroupCollection _groupCollection;
        public GroupCollection CustomGroups
        {
            get
            {
                // The IMClientGroupCollection class implements
                // the IGroupCollection interface.
                return _groupCollection;
            }
        }

        private string _uri;
        public string Uri
        {
            get => _uri;
            set => _uri = value;
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => _displayName = value;
        }

        public ContactManager ContactManager => throw new NotImplementedException();

        public UnifiedCommunicationType UnifiedCommunicationType
        {
            get => UnifiedCommunicationType.ucUnifiedCommunicationNotEnabled;
            set => UnifiedCommunicationType = value;
        }

        private HttpClient httpClient = PresenceProvider.httpClient;

        public bool CanStart(ModalityTypes _modalityTypes)
        {
            Console.WriteLine(_modalityTypes);
            return false;
        }

        private ContactAvailability _availability = ContactAvailability.ucAvailabilityNone;

        private ContactAvailability GetAvailabilityFromMattermost()
        {
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.lync.model.contactavailability?view=lync-client
            string serverUrl = "";
            using (RegistryKey IMProviders = Registry.CurrentUser.OpenSubKey("SOFTWARE\\IM Providers", true))
            {
                using (RegistryKey IMProvider = IMProviders.CreateSubKey(PresenceProvider.COMAppExeName))
                {
                    serverUrl = (string)IMProvider.GetValue("MattermostServerURL");
                }
            }
            if (serverUrl == "")
            {
                // We will not be using this value from the registry so just log the error for now
                Console.WriteLine("Invalid server url");
                return ContactAvailability.ucAvailabilityNone;
            }
            string reqUri = $"{serverUrl}/plugins/com.mattermost.presence-provider/api/v1/status/{_uri}";
            Console.WriteLine(reqUri);
            JsonNode statusNode = JsonNode.Parse(httpClient.GetStringAsync(reqUri).GetAwaiter().GetResult());
            return Constants.StatusAvailabilityMap(statusNode["status"].GetValue<string>());
        }

        public object GetContactInformation(ContactInformationType _contactInformationType)
        {
            try
            {
                // Determine the information to return from the contact's data based
                // on the value passed in for the _contactInformationType parameter.
                switch (_contactInformationType)
                {
                    // https://docs.microsoft.com/en-us/dotnet/api/microsoft.lync.model.contactavailability?view=lync-client
                    case ContactInformationType.ucPresenceAvailability:
                        {
                            return GetAvailabilityFromMattermost();
                        }
                    case ContactInformationType.ucPresenceActivityId:
                        {
                            return Constants.AvailabilityActivityIdMap(_availability);
                        }
                    case ContactInformationType.ucPresenceEmailAddresses:
                        {
                            // Return the URI associated with the contact.
                            return _uri;
                        }
                    case ContactInformationType.ucPresenceDisplayName:
                        {
                            // Return the display name associated with the contact.
                            return _displayName;
                        }
                    case ContactInformationType.ucPresenceInstantMessageAddresses:
                        {
                            return new string[] { _uri };
                        }
                    default:
                        {
                            Console.WriteLine(_contactInformationType.ToString());
                            return null;
                        }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public ContactInformationDictionary BatchGetContactInformation(ContactInformationType[] _contactInformationTypes)
        {
            // The IMClientContactInformationDictionary class implements the
            // IContactInformationDictionary interface.
            IMClientContactInformationDictionary contactDictionary =
                new IMClientContactInformationDictionary();
            foreach (ContactInformationType type in _contactInformationTypes)
            {
                // Call GetContactInformation for each type of contact 
                // information to retrieve. This code adds a new entry to
                // a Dictionary object exposed by the
                // ContactInformationDictionary property.
                Console.WriteLine(type.ToString());
                contactDictionary.Add(type, GetContactInformation(type));
            }
            return contactDictionary;
        }

        public AsynchronousOperation ChangeSetting(ContactSetting _contactSettingType, object _contactSettingValue, [IUnknownConstant] object _contactCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public bool CanChangeSetting(ContactSetting _contactSetting)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation MoveToGroup(Group _targetGroup, Group _sourceGroup, [IUnknownConstant] object _contactCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public bool CanMoveToGroup(Group _targetGroup, Group _sourceGroup)
        {
            throw new NotImplementedException();
        }

        public ContactEndpoint CreateContactEndpoint(string _telephoneUri)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation GetOrganizationInformation(OrganizationStructureTypes _orgInfoTypes, [IUnknownConstant] object _contactCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public ContactInformationDictionary GetMultipleContactInformation(ContactInformationType[] _contactInformationTypes)
        {
            throw new NotImplementedException();
        }

        public event _IContactEvents_OnContactInformationChangedEventHandler OnContactInformationChanged;
        public void RaiseOnContactInformationChangedEvent(ContactInformationChangedEventData _eventData)
        {
            _IContactEvents_OnContactInformationChangedEventHandler handler = OnContactInformationChanged;
            if (handler != null)
            {
                try
                {
                    handler(this, _eventData);
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public event _IContactEvents_OnSettingChangedEventHandler OnSettingChanged;
        internal void RaiseOnSettingChangedEvent(ContactSettingChangedEventData _eventData)
        {
            if(OnSettingChanged != null)
            {
                OnSettingChanged(this, _eventData);
            }
        }

        public event _IContactEvents_OnUriChangedEventHandler OnUriChanged;
        internal void RaiseOnUriChangedEvent(UriChangedEventData _eventData)
        {
            if (OnUriChanged != null)
            {
                OnUriChanged(this, _eventData);
            }
        }
    }
}
