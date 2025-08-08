using Models.User;

namespace Models.Message 
{ 
    public class Message
    {
        private User.User sender;
        private User.User receiver;
        private string message;
        private string timestamp;


        public Message(User.User sender, User.User receiver, string message, string timestamp)
        {
            this.sender = sender;
            this.receiver = receiver;
            this.message = message;
            this.timestamp = timestamp;
        }

        public Message() { }





        


    }


}
