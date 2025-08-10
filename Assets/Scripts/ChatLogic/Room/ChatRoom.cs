using System.Collections.Generic;
using Models.Bots;
using Models.Users;
using Models.Message;
namespace Chat.Room
{

    // it needs to store a reference to a user
    // another reference to the bot


    public class ChatRoom
    {
        // id to uniquely identify a chat room
        private int id;

        
        private Bot bot;

        // this will hold the list of messages in chronological order
        private List<Message> messages_chronological;

        public ChatRoom(int id ,Bot bot) 
        {
            this.id = id;
            this.bot = bot;
            messages_chronological = new List<Message>();
        }
        public bool AddMessage(Message message) 
        {
            if (messages_chronological == null) return false;
            // if the bot isn't the same we just return false
            if (message.Bot != this.bot) return false;
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