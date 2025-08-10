
namespace Models.Users
{

    public class User
    {
        protected string name;
        protected int age;
        protected int id;

        public string Name
        {
            get { return name; }
            // if needed add verification checks on the name
            set { name = value; }
        }
        public int Age
        {
            get { return age; }
            // add verifications for the age
            set { age = value; }
        }
        public int ID
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