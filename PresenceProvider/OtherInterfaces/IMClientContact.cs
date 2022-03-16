﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    public class IMClientContact: IContact
    {
        public IMClientContactSettingDictionary Settings
        {
            get
            {
                // The IMClientContactSettingDictionary class implements
                // the IContactSettingDictionary interface.
                return new IMClientContactSettingDictionary();
            }
        }

        public IMClientGroupCollection CustomGroups
        {
            get
            {
                // The IMClientGroupCollection class implements
                // the IGroupCollection interface.
                return new IMClientGroupCollection();
            }
        }

        public string Uri;
        public string _DisplayName;

        public bool CanStart(ModalityTypes _modalityTypes)
        {
            // Define the capabilities of the current IM client application
            // user by using flags from the ModalityTypes enumeration.
            ModalityTypes userCapabilities =
                ModalityTypes.ucModalityInstantMessage;
            // Perform a simple test for equivalency.
            return _modalityTypes == userCapabilities;
        }

        public object GetContactInformation(ContactInformationType _contactInformationType)
        {
            // Determine the information to return from the contact's data based
            // on the value passed in for the _contactInformationType parameter.
            switch (_contactInformationType)
            {
                case ContactInformationType.ucPresenceEmailAddresses:
                    {
                        // Return the URI associated with the contact.
                        string returnValue = this.Uri.ToLower().Replace("sip:", String.Empty);
                        return returnValue;
                    }
                case ContactInformationType.ucPresenceDisplayName:
                    {
                        // Return the display name associated with the contact.
                        string returnValue = this._DisplayName;
                        return returnValue;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
                    // Additional implementation details omitted.
            }
        }

        public IMClientContactInformationDictionary BatchGetContactInformation(ContactInformationType[] _contactInformationTypes)
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
                contactDictionary.Add(type, this.GetContactInformation(type));
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

        string IContact.Uri => throw new NotImplementedException();

        ContactSettingDictionary IContact.Settings => throw new NotImplementedException();

        public ContactManager ContactManager => throw new NotImplementedException();

        GroupCollection IContact.CustomGroups => throw new NotImplementedException();

        public UnifiedCommunicationType UnifiedCommunicationType => throw new NotImplementedException();
    }

}