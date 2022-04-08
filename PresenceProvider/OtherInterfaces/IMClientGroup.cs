using System;
using System.Collections;
using System.Collections.Generic;
using UCCollaborationLib;
using System.Runtime.InteropServices;

namespace OutlookPresenceProvider
{
    [ComVisible(true)]
    public class IMClientGroup: Group
    {
        private List<IMClientContact> _contacts;
        public IMClientGroup(GroupType type)
        {
            _type = type;
            _contacts = new List<IMClientContact>();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var contact in _contacts)
            {
                yield return contact;
            }
        }

        public bool TryGetContact(string _uri, out Contact _value)
        {
            foreach (IMClientContact contact in _contacts)
            {
                if (contact.Uri == _uri)
                {
                    _value = contact;
                    return true;
                }
            }
            _value = null;
            return false;
        }

        public AsynchronousOperation AddContact(Contact _contact, object _groupCallback, object _state)
        {
            _contacts.Add(_contact as IMClientContact);
            return null;
        }

        public AsynchronousOperation RemoveContact(Contact pContact, object _groupCallback, object _state)
        {
            _contacts.Remove(pContact as IMClientContact);
            return null;
        }

        public bool CanInvoke(GroupAction _action, Contact _contact)
        {
            throw new NotImplementedException();
        }

        public int Count => _contacts.Count;

        private GroupType _type;
        public GroupType Type
        {
            get => _type;
            set => _type = value;
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private string _id;
        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public ContactManager ContactManager => throw new NotImplementedException();
    }
}
