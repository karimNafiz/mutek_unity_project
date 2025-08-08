using Models.Chat;

namespace Models.Bots
{
    public class Bots : Chat.Bots
    {

        private int id;
        // this is the status of the bot like if they are alive or not
        private int status;
        public int ID { get { return id; } }

        public Bots(string name, int age, int id) : base(name, age) 
        {
            this.id = id;
        }

    }
}
