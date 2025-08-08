using System;
using Utility.Singleton;
using System.Collections.Generic;
using Models.Bots;
using Utility.HTTP;
namespace WebServer
{
    public class WebServerClient : SingletonMonoBehavior<WebServerClient>
    {
        
        // need function for GettingBots
        /*need to rethink this shit*/


        // have a class called the ChattableBotsManager -> has all the ChatBots that we can chat with 
        
        // have this function return the list of all the bots 
        public void GetBots(Url url, string endpoint, Action<HashSet<Bot>> onSuccess, Action<string> onErr)  
        {

            Action<string> onSuccCallback = new Action<string>((string json)=> 
            {
                List<Dictionary<string, object>> dictFrmJson = HTTPClient.Instance.ParseJsonToDictionaryList(json);
                

            
            }); 


            HTTPClient.Instance.GetRequest($"{url.GetHostUrl()}{endpoint}")
            // get the json response back 

        }


        public Bot GetBotFrmJson(Dictionary<string, object> dict) 
        {
            if (!dict.ContainsKey("name")) 
            {
                
                throw new Exception();
            }
        
        
        }



        // need a function for GetMessagesForASpecificBot
        // a function to send a message and gets a response back
    }
}
