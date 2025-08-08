using System.Collections.Generic;
using Models.ChatBots;
using Models.Message;
namespace Chat.Room
{

    public class ChatRoom
    {
        // id to uniquely identify a chat room
        private int id;
        // hash set to hold all the participants in the chat 
        // the reason I'm making it a hashset because, then we can cross check participants in O(1) time
        private HashSet<ChatBot> participants;
        // this will hold the list of messages in chronological order
        private List<Message> messages_chronological;

        public ChatRoom(int id , ChatBot participant1, ChatBot participant2) 
        {
            this.id = id;
            participants = new HashSet<ChatBot>() { participant1, participant2 };
            messages_chronological = new List<Message>();
        }
        public bool AddMessage(Message message) 
        {
            if (messages_chronological == null) return false;
            if (!this.participants.Contains(message.Sender)) return false;
            if (!this.participants.Contains(message.Receiver)) return false;
            // make sure the time is synced but life is too short to do that
            messages_chronological.Add(message);
            return true;

        }
        public void Clear() 
        {
            this.messages_chronological.Clear();

        }



    }
}