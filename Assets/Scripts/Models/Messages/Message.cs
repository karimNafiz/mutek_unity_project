using Models.ChatBots;

namespace Models.Message 
{ 
    public class Message
    {
        private ChatBot sender;
        private ChatBot receiver;
        private string message;
        private string timestamp;

        public ChatBot Sender { get { return sender; } }
        public ChatBot Receiver { get { return receiver; } }


        public Message(ChatBot sender, ChatBot receiver, string message, string timestamp)
        {
            this.sender = sender;
            this.receiver = receiver;
            this.message = message;
            this.timestamp = timestamp;
        }

        public Message() { }





        


    }


}
