using Models.Chat;

namespace Models.Bots
{
    public class Bot : Chat.User
    {

        private int id;
        // this is the status of the bot like if they are alive or not
        private int status;
        public int ID { get { return id; } }

        public Bot(string name, int age, int id) : base(name, age) 
        {
            this.id = id;
        }

    }
}
