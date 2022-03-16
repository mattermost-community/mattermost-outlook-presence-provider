using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCCollaborationLib;
using System.Runtime.InteropServices;

namespace OutlookPresenceProvider
{
    [ComVisible(true)]
    public class IMClientConversationManager : ConversationManager
    {
        public IMClientConversationManager()
        {
            _summarizerStates = ConversationSummarizerStates.ucConversationSummarizerNoSummarizedState;
        }
        public Conversation AddConversation()
        {
            throw new NotImplementedException();
        }

        public Conversation JoinConference(string _conferenceUrl)
        {
            throw new NotImplementedException();
        }

        private ConversationSummarizerStates _summarizerStates;
        public ConversationSummarizerStates SummarizerStates
        {
            get { return _summarizerStates; }
            set { _summarizerStates = value;}
        }

        public ConversationCollection Conversations => throw new NotImplementedException();

        public VoiceMailsManager VoiceMailsManager => throw new NotImplementedException();

        public PreviousConversationsManagerDictionary PreviousConversationsManagers => throw new NotImplementedException();

        public event _IConversationManagerEvents_OnConversationAddedEventHandler OnConversationAdded;
        public event _IConversationManagerEvents_OnConversationRemovedEventHandler OnConversationRemoved;
        public event _IConversationManagerEvents_OnSummarizerStatesChangedEventHandler OnSummarizerStatesChanged;
    }
}
