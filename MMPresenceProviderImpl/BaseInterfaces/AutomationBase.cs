using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    [ComVisible(true)]
    public class AutomationBase : Automation
    {
        public AutomationBase()
        {
        }

        public ConversationWindow GetConversationWindow(Conversation _conversation)
        {
            throw new NotImplementedException();
        }

        public ConversationWindow JoinConference(string _conferenceUrl)
        {
            throw new NotImplementedException();
        }

        public AsynchronousOperation JoinConferenceEx(string _conferenceUrl, long _parentHWND = 0, [IUnknownConstant] object _callback = null, object _state = null)
        {
            throw new NotImplementedException();
        }

        public void LaunchAddContactWizard(string _contactEmail = "0")
        {
            throw new NotImplementedException();
        }

        public ConversationWindow StartConversation(AutomationModalities _conversationModes, string[] _participantUris, AutomationModalitySettings[] _contextTypes, object[] _contextDatas)
        {
            return null;
        }

        public AsynchronousOperation StartConversationEx(AutomationModalities _conversationModes, string[] _participantUris, AutomationModalitySettings[] _contextTypes, object[] _contextDatas, [IUnknownConstant] object s_callback, object _state)
        {
            throw new NotImplementedException();
        }
    }
}
