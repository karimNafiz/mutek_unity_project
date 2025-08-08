using Models.Bots;

namespace Models.ChatBots
{
    public class ChatBots
    {
        private Bots.Bots bot;
        // this status represents the chat status
        private int status;
        public Bots.Bots Bot
        {
            get { return bot; }
            set { bot = value; }
        }

        public ChatBots(Bots.Bots bot)
        {
            this.bot = bot;
        }

    }
}
