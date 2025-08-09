
using Models.Users;
using UnityEngine;
namespace Models.Bots
{
    public class Bot : User
    {


        private int status;
        private string location;
        public int Status
        {
            get { return status; }
            //set { status = value; }
        }
        public string Location
        {
            get { return location; }
        }


        public Bot(string name, string location, int age, int status, int id) : base(name, age, id) 
        {
            this.status = status;
            this.location = location;
        }

        public override string ToString()
        {
            return $"Bot: id{this.id} name {this.name} age {this.age} status{this.status} location{this.location}";
        }

    }
}
