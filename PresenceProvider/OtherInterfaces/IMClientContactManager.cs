using System;
using System.Runtime.InteropServices;
using UCCollaborationLib;
using System.Net;

namespace OutlookPresenceProvider
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(_IContactManagerEvents))]
    [ComVisible(true)]
    public class IMClientContactManager : ContactManager
    {
        public IMClientContactManager()
        {
            _groupCollections = new IMClientGroupCollection();
            _contactSubscription = new IMClientContactSubscription();
        }

        public GroupCollection Groups => _groupCollections;

        private IMClientGroupCollection _groupCollections;
        public Contact GetContactByUri(string _contactUri)
        {
            Console.WriteLine(_contactUri);
            // Declare a Contact variable to contain information about the contact.
            Contact tempContact = null;
            // The _groupCollections field is an IGroupCollection object. Iterate 
            // over each group in collection to see if the 
            // contact is a part of the group.
            foreach (IMClientGroup group in _groupCollections)
            {
                if (group.TryGetContact(_contactUri, out tempContact))
                {
                    break;
                }
            }
            // Check to see that the URI returned a valid contact. If it
            // did not, create a new contact.
            if (tempContact == null)
            {
                tempContact = new IMClientContact(_contactUri);
            }
            // Return the contact to the calling code.
            return tempContact;
        }

        public AsynchronousOperation Lookup(string _lookupString, object _contactsAndGroupsCallback, object _state)
        {
            Console.WriteLine(_lookupString);
            Console.WriteLine(_contactsAndGroupsCallback);
            Console.WriteLine(_state);
            IMClientAsyncOperation asyncOperation = new IMClientAsyncOperation();
            try
            {
                asyncOperation.AsyncState = _state;
                asyncOperation.IsCompleted = true;
                asyncOperation.IsSucceeded = true;
                asyncOperation.StatusCode = (int)HttpStatusCode.OK;

                Console.WriteLine(_contactsAndGroupsCallback is _IContactsAndGroupsCallback);
                _IContactsAndGroupsCallback itf = (_IContactsAndGroupsCallback)_contactsAndGroupsCallback;
                itf.OnLookup(this, null, asyncOperation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return asyncOperation;
        }

        // Declare a private field to contain an IContactSubscription object.
        private IMClientContactSubscription _contactSubscription;
        // Return the IContactSubscription object associated 
        // with the IContactManager object.
        public ContactSubscription CreateSubscription()
        {
            return _contactSubscription;
        }

        public AsynchronousOperation Search(string _searchString, SearchProviders _providers = SearchProviders.ucSearchProviderDefault, SearchFields _searchFields = SearchFields.ucSearchAllFields, SearchOptions _searchOptions = SearchOptions.ucSearchDefault, uint _maxResults = 30, object _contactsAndGroupsCallback = null, object _state = null)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation RemoveContactFromAllGroups(Contact _contact, object _contactsAndGroupsCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public string GetSearchProviderID(SearchProviders _provider)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation AddCustomGroup(string _customGroupName, object _contactsAndGroupsCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation AddDistributionGroup(DistributionGroup _distributionGroup, object _contactsAndGroupsCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation RemoveGroup(Group _group, object _contactsAndGroupsCallback, object _state)
        {
            throw new NotImplementedException();
        }

        public void SuspendSubscriptions()
        {
            throw new NotImplementedException();
        }

        public void ResumeSubscriptions()
        {
            throw new NotImplementedException();
        }

        public SearchFields GetSearchFields()
        {
            throw new NotImplementedException();
        }

        public SearchProviderStatusType GetSearchProviderStatus(SearchProviders _provider)
        {
            throw new NotImplementedException();
        }

        public string GetExpertSearchQueryString(string _searchString)
        {
            throw new NotImplementedException();
        }

        public Contact GetSelfContact()
        {
            throw new NotImplementedException();
        }

        public Contact GetContactByTel(string _telUri)
        {
            throw new NotImplementedException();
        }

        #region _IContactManagerEvents support
        public event _IContactManagerEvents_OnGroupAddedEventHandler OnGroupAdded;
        internal void RaiseOnGroupAddedEvent(GroupCollectionChangedEventData _eventData)
        {
            if (OnGroupAdded != null)
            {
                OnGroupAdded(this, _eventData);
            }
        }

        public event _IContactManagerEvents_OnGroupRemovedEventHandler OnGroupRemoved;
        internal void RaiseOnGroupRemovedEvent(GroupCollectionChangedEventData _eventData)
        {
            if (OnGroupRemoved != null)
            {
                OnGroupRemoved(this, _eventData);
            }
        }

        public event _IContactManagerEvents_OnSearchProviderStateChangedEventHandler OnSearchProviderStateChanged;
        internal void RaiseOnSearchProviderStateChangedEvent(SearchProviderStateChangedEventData _eventData)
        {
            if (OnSearchProviderStateChanged != null)
            {
                OnSearchProviderStateChanged(this, _eventData);
            }
        }
        #endregion
    }
}
