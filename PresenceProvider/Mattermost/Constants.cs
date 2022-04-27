using UCCollaborationLib;

namespace OutlookPresenceProvider.Mattermost
{
    public class Constants
    {
        public const string MattermostServerURL = "MattermostServerURL";
        public const string MattermostSecret = "MattermostSecret";
        public const string PluginId = "com.mattermost.outlook-presence";
        public const double WebsocketReconnectionTimeoutInSeconds = 30;

        public static ContactAvailability StatusAvailabilityMap(string status)
        {
            switch (status)
            {
                case "online": return ContactAvailability.ucAvailabilityFree;
                case "away": return ContactAvailability.ucAvailabilityAway;
                case "dnd": return ContactAvailability.ucAvailabilityDoNotDisturb;
                case "offline": return ContactAvailability.ucAvailabilityOffline;
            }
            return ContactAvailability.ucAvailabilityNone;
        }

        public static string AvailabilityActivityIdMap(ContactAvailability availability)
        {
            switch (availability)
            {
                case ContactAvailability.ucAvailabilityFree: return "Free";
                case ContactAvailability.ucAvailabilityAway: return "Away";
                case ContactAvailability.ucAvailabilityDoNotDisturb: return "DoNotDisturb";
                case ContactAvailability.ucAvailabilityOffline: return "Offline";
            }
            return "";
        }
    }
}
