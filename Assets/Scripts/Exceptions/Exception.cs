using System;


namespace Exceptions 
{ 



    public class BadRequestException : Exception 
    {
        public BadRequestException(string message) : base(message) { }


    }

    public class BadConnectionException : Exception 
    {
        public BadConnectionException(string message) : base(message) { }
    
    }


}
