using Models.User;

namespace Models.Bots
{
    public class Bots : User.User
    {

        private int id;
        public int ID { get { return id; } }

        public Bots(string name, int age, int id) : base(name, age) 
        {
            this.id = id;
        }

    }
}
