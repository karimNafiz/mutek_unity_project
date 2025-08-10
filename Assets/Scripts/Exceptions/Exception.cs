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

    public class BotDoesNotExistException : Exception 
    {
        public int botID;
        public BotDoesNotExistException(int botID) 
        {
            this.botID = botID;
        }
        public override string Message => $"bot with botID {this.botID} does not exist";

    }

    public class BotIDMismatchExeption : Exception 
    { 
        
    
    
    
    }


}
