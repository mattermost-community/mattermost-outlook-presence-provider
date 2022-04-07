using System;
using System.Collections.Generic;
using UCCollaborationLib;
using System.Runtime.InteropServices;
using System.Timers;
using System.Net.Http;

namespace OutlookPresenceProvider
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class IMClientContactSubscription : ContactSubscription
    {
        public static System.Timers.Timer myTimer;
        public IMClientContactSubscription()
        {
            _subscribedContacts = new List<IMClientContact>();
        }

        ~IMClientContactSubscription()
        {
            // Stop and dispose the timer when the object of IMClientContactSubscription is destroyed
            myTimer?.Stop();
            myTimer?.Dispose();
        }

        // Store references to all of the IContact objects to subscribe to.
        private List<IMClientContact> _subscribedContacts;

        // Add a new IContact object to the collection of contacts.
        public void AddContact(Contact _contact)
        {
            Console.WriteLine(_contact.Uri);
            _subscribedContacts.Add(_contact as IMClientContact);
        }

        public void Subscribe(ContactSubscriptionRefreshRate _subscriptionFreshness, ContactInformationType[] _contactInformationTypes)
        {
            // TODO: Remove polling strategy and add websocket client
            if (myTimer == null)
            {
                SetTimer();
            }
        }

        public void Unsubscribe()
        {
            throw new NotImplementedException();
        }

        public void RemoveContact(Contact _contact)
        {
            Console.WriteLine(_contact.Uri);
            _subscribedContacts.Remove(_contact as IMClientContact);
        }

        private void SetTimer()
        {
            // Create a timer with a ten second interval.
            myTimer = new System.Timers.Timer(10000);
            // Hook up the Elapsed event for the timer. 
            myTimer.Elapsed += fetchStatuses;
            myTimer.AutoReset = true;
            myTimer.Enabled = true;
        }

        private void fetchStatuses(object source, ElapsedEventArgs args)
        {
            ContactInformationChangedEventData eventData = new IMClientContactInformationChangedEventData();
            foreach (IMClientContact contact in _subscribedContacts)
            {
                contact.RaiseOnContactInformationChangedEvent(eventData);
            }
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
