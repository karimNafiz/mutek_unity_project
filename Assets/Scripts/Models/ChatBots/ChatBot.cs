using Models.Users;

namespace Models.ChatBots
{
    public enum ChatStatus 
    { 
        ONLINE,
        OFFLINE
 
    }
    public class ChatBot
    {
        private User bot;
        // this status represents the chat status
        // chat status represents if they are active for chat or not
        // might be confusing along with bot status, but status represents either the bot is alive or dead
        private ChatStatus status;
        public User Bot
        {
            get { return bot; }
            set { bot = value; }
        }

        public ChatBot(User bot)
        {
            this.bot = bot;
        }

        public ChatStatus Status 
        {
            get { return status; }
            set { status = value; }
        
        }


    }
}
