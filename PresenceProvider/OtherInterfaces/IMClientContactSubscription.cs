using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    public class IMClientContactSubscription : ContactSubscription
    {
        // Store references to all of the IContact objects to subscribe to.
        private List<IMClientContact> _subscribedContacts;
        // Add a new IContact object to the collection of contacts.
        public void AddContact(Contact _contact)
        {
            _subscribedContacts.Add(_contact as IMClientContact);
        }

        public void Subscribe(ContactSubscriptionRefreshRate _subscriptionFreshness, ContactInformationType[] _contactInformationTypes)
        {
            Console.WriteLine(_subscriptionFreshness.ToString());
            Console.WriteLine(_contactInformationTypes.ToString());
        }

        public void Unsubscribe()
        {
            throw new NotImplementedException();
        }

        public void RemoveContact(Contact _contact)
        {
            throw new NotImplementedException();
        }

        public void AddContactByUri(string _contactUri)
        {
            throw new NotImplementedException();
        }

        public void AddContacts(Contact[] _contacts)
        {
            throw new NotImplementedException();
        }

        public ContactSubscriptionRefreshRate LastSubscribedRefreshRate => throw new NotImplementedException();

        public ContactInformationType[] LastSubscribedContactInformation => throw new NotImplementedException();

        public Contact[] Contacts => throw new NotImplementedException();
    }
}
