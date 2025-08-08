
namespace Models.Chat
{

    public class Bots
    {
        private string name;
        private int age;

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

        public Bots(string name, int age) 
        {
            this.age = age;
            this.name = name;
        }



    }
}