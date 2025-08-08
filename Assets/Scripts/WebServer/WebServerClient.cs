using System;
using Utility.Singleton;

namespace WebServer
{
    public class WebServerClient : SingletonMonoBehavior<WebServerClient>
    {
        // need function for GettingBots
        /*need to rethink this shit*/
        public void GetBots(Url url, string endpoint, Action<string> onSuccess, Action<string> onErr)  
        { 

        }



        // need a function for GetMessagesForASpecificBot
        // a function to send a message and gets a response back
    }
}
