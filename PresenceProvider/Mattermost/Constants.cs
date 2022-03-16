using System.Collections.Generic;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    public class Constants
    {
        public static readonly Dictionary<string, ContactAvailability> statusMap = new Dictionary<string, ContactAvailability>()
        {
            {"online", ContactAvailability.ucAvailabilityFree },
            {"away", ContactAvailability.ucAvailabilityAway },
            {"dnd", ContactAvailability.ucAvailabilityDoNotDisturb },
            {"offline", ContactAvailability.ucAvailabilityOffline },
        };
    }
}
