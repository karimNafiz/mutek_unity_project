using Models.ChatBots;
using Models.Users;
using Models.Bots;
namespace Models.Message 
{ 
    public class Message
    {
        private User user;
        private Bot  bot;
        private string userMessage;
        private string botResponse;
        private string timestamp;

        public User User
        {
            get { return user; }
        
        }
        public Bot Bot
        {
            get { return bot; }
            set { bot = value; }        
        }

        public Message(User user, Bot bot, string userMessage, string botResponse, string timestamp)
        {
            this.user = user;
            this.bot = bot;
            this.userMessage = userMessage;
            this.botResponse = botResponse;
            this.timestamp = timestamp;
        }

        public Message() { }





        


    }


}
