
namespace Models.Users
{

    public class User
    {
        protected string name;
        protected int age;
        protected int id;

        private string Name
        {
            get { return name; }
            // if needed add verification checks on the name
            set { name = value; }
        }
        private int Age
        {
            get { return age; }
            // add verifications for the age
            set { age = value; }
        }
        private int ID
        {
            get { return id; }
            set { id = value; }
        }

        public User(string name, int age, int id) 
        {
            this.id = id;
            this.age = age;
            this.name = name;
        }



    }
}