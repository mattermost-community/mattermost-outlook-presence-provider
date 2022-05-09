using System;
using System.Diagnostics;
using System.Collections.Generic;
using UCCollaborationLib;

namespace OutlookPresenceProvider.Mattermost
{
    public class Store
    {
        private Dictionary<string, string> _store;
        public Store()
        {
            _store = new Dictionary<string, string>();
        }

        public void Add(string email, string status)
        {
            _store[email] = status;
        }

        public ContactAvailability GetAvailability(string email)
        {
            try
            {
                Trace.TraceInformation($"Returning availability of user: {email} from the store.");
                return Constants.StatusAvailabilityMap(_store[email]);
            } catch (Exception ex)
            {
                Utils.LogException(ex);
                return ContactAvailability.ucAvailabilityOffline;
            }
        }

        public bool Remove(string email)
        {
            return _store.Remove(email);
        }
    }
}
