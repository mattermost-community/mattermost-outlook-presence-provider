using System.Collections.Generic;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    public class Constants
    {
        public static readonly Dictionary<string, ContactAvailability> statusAvailabilityMap = new Dictionary<string, ContactAvailability>()
        {
            {"online", ContactAvailability.ucAvailabilityFree },
            {"away", ContactAvailability.ucAvailabilityAway },
            {"dnd", ContactAvailability.ucAvailabilityDoNotDisturb },
            {"offline", ContactAvailability.ucAvailabilityOffline },
        };

        public static readonly Dictionary<ContactAvailability, string> availabilityActivityIdMap = new Dictionary<ContactAvailability, string>()
        {
            {ContactAvailability.ucAvailabilityFree, "Free" },
            {ContactAvailability.ucAvailabilityAway, "Away" },
            {ContactAvailability.ucAvailabilityDoNotDisturb, "DoNotDisturb" },
            {ContactAvailability.ucAvailabilityOffline, "Offline" },
        };
    }
}
