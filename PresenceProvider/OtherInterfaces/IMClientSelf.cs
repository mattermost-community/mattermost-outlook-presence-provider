﻿using System;
using System.Runtime.CompilerServices;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    public class IMClientSelf : ISelf
    {

        // Declare a private field to store contact data for local user.
        private IMClientContact _contactData;
        // In the constructor for the ISelf object, the calling code 
        // must supply contact data.
        public IMClientSelf(IMClientContact _selfContactData)
        {
            this._contactData = _selfContactData;
        }
        // When accessed, the Contact property returns a reference
        // to the IContact object that represents the local user.
        public IMClientContact Contact
        {
            get
            {
                return this._contactData as IMClientContact;
            }
        }

        public AsynchronousOperation PublishContactInformation(PublishableContactInformationType[] _publishablePresenceItemTypes, object[] _publishablePresenceItemValues, [IUnknownConstant] object _selfCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public Phone CreatePhone(ContactEndpointType _phoneType, string _phoneUri, bool _toBePublished = false)
        {
            throw new NotImplementedException();
        }

        public bool CanSetPhone(ContactEndpointType _phoneType)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation SetPhones(Phone[] _phones, [IUnknownConstant] object _selfCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public CustomAvailabilityState[] GetPublishableCustomAvailabilityStates(int _localeId)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation SetPhone(ContactEndpointType _phoneType, string _phoneUri, bool _toBePublished = false, [IUnknownConstant] object _selfCallback = null, object _state = null)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation RemovePhone(ContactEndpointType _phoneType, [IUnknownConstant] object _selfCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public Phone GetPhone(ContactEndpointType _phoneType)
        {
            throw new NotImplementedException();
        }

        public dynamic GetSettingInternal(SettingItemTypeInternal _settingItem)
        {
            throw new NotImplementedException();
        }

        public Phone[] Phones => throw new NotImplementedException();

        public AccessPermission[] Permissions => throw new NotImplementedException();

        public bool PhotoDisplayed => throw new NotImplementedException();

        public ContactEndpoint TestCallEndpoint => throw new NotImplementedException();

        public bool IsInResiliencyMode => throw new NotImplementedException();
    }
}