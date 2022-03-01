using System;
using System.Collections;
using System.Collections.Generic;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    public class IMClientGroup: Group
    {
        //private List<IMClientContact> _contacts;
        //public IMClientGroup()
        //{
        //    _contacts = new List<IMClientContact>();
        //}
        public IEnumerator GetEnumerator()
        {
            //foreach (var contact in _contacts)
            //{
            //    yield return contact;
            //}
            throw new NotImplementedException();
        }

        public bool TryGetContact(string _uri, out Contact _value)
        {
            //foreach (IMClientContact contact in _contacts)
            //{
            //    if(contact.Uri == _uri)
            //    {
            //        _value = contact;
            //        return true;
            //    }
            //}
            //_value = null;
            //return false;
            throw new NotImplementedException();
        }

        public AsynchronousOperation AddContact(Contact _contact, object _groupCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation RemoveContact(Contact pContact, object _groupCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public bool CanInvoke(GroupAction _action, Contact _contact)
        {
            throw new NotImplementedException();
        }

        public int Count => throw new NotImplementedException();

        public GroupType Type => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        private string _id;
        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public ContactManager ContactManager => throw new NotImplementedException();
    }
}
