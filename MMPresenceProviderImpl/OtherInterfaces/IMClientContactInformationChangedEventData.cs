using System.Runtime.InteropServices;
using UCCollaborationLib;

namespace MMPresenceProviderImpl
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class IMClientContactInformationChangedEventData : ContactInformationChangedEventData
    {
        public IMClientContactInformationChangedEventData()
        {
            // The scope of this project is to update the presence icon and text for the contacts,
            // so we are just adding two values to the array: "availability" for changing the presence
            // icon and "activityId" for changing the presence text
            _changedContactInformation = new ContactInformationType[2] {
                ContactInformationType.ucPresenceAvailability, ContactInformationType.ucPresenceActivityId
            };
            
        }
        private ContactInformationType[] _changedContactInformation;
        public ContactInformationType[] ChangedContactInformation
        {
            get => _changedContactInformation;
            set => _changedContactInformation = value;
        }
    }
}
