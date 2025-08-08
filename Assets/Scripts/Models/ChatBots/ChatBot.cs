using Models.Bots;

namespace Models.ChatBots
{
    public class ChatBot
    {
        private Bot bot;
        // this status represents the chat status
        private int status;
        public Bot Bot
        {
            get { return bot; }
            set { bot = value; }
        }

        public ChatBot(Bot bot)
        {
            this.bot = bot;
        }

    }
}
