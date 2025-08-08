using Models.ChatBots;

namespace Models.Message 
{ 
    public class Message
    {
        private ChatBots.ChatBots sender;
        private ChatBots.ChatBots receiver;
        private string message;
        private string timestamp;

        public ChatBots.ChatBots Sender { get { return sender; } }
        public ChatBots.ChatBots Receiver { get { return receiver; } }


        public Message(ChatBots.ChatBots sender, ChatBots.ChatBots receiver, string message, string timestamp)
        {
            this.sender = sender;
            this.receiver = receiver;
            this.message = message;
            this.timestamp = timestamp;
        }

        public Message() { }





        


    }


}
