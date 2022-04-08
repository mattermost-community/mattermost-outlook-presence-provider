using System.Runtime.InteropServices;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class IMClientContactInformationChangedEventData : ContactInformationChangedEventData
    {
        public IMClientContactInformationChangedEventData()
        {
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
