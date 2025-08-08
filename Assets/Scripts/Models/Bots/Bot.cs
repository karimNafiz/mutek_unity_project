
using Models.Users;

namespace Models.Bots
{
    public class Bot : User
    {


        private int status;
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public Bot(string name,int age, int status, int id) : base(name, age, id) 
        {
            this.status = status;
        }

    }
}
