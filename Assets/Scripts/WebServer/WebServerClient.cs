using System;
using Utility.Singleton;
using System.Collections.Generic;
using Models.Bots;
using Utility.HTTP;
using Exceptions;
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
                try
                {
                    List<Dictionary<string, object>> dictFrmJson = HTTPClient.Instance.ParseJsonToDictionaryList(json);
                    // important note, the reason im making this hashSet is so that we can do Set Operations on these bots
                    HashSet<Bot> bots = new HashSet<Bot>();
                    foreach (Dictionary<string, object> pair in dictFrmJson)
                    {

                        bots.Add(GetBotFrmJson(pair));


                    }
                    onSuccess?.Invoke(bots);
                }
                catch (BadRequestException ex)
                {
                    // do something
                }
                catch (Exception ex) 
                { 
                    // do something
                }
            });


            Action<string> onErrCallback = new Action<string>((string err)=> 
            { 
            
                
            
            });


            HTTPClient.Instance.GetRequest($"{url.GetHostUrl()}{endpoint}")
            // get the json response back 

        }

        // IMPORTANT TODO: refactor all this bullshit code to it's dedicated file if we are expanding this project
        public Bot GetBotFrmJson(Dictionary<string, object> dict) 
        {
            
            if (!dict.ContainsKey("id")) throw new BadRequestException("json for bot is missing the key id ");        
            if (!dict.ContainsKey("name")) throw new BadRequestException("json for bot is missing the key name ");        
            if (!dict.ContainsKey("location")) throw new BadRequestException("json for bot is missing the key location ");        
            if (!dict.ContainsKey("age")) throw new BadRequestException("json for bot is missing the key age ");        
            if (!dict.ContainsKey("status")) throw new BadRequestException("json for bot is missing the key status ");
            try
            {
                return new Bot(dict["name"].ToString(), dict["location"].ToString(), int.Parse(dict["age"].ToString()), int.Parse(dict["status"].ToString()), int.Parse(dict["id"].ToString()));

            }
            catch (FormatException ex) 
            {
                
                throw ex;
            }
            catch (InvalidCastException ex)
            {
                throw ex;
            }
            catch (OverflowException ex) 
            {
                throw ex;
            }
        
        
        }
        



        // need a function for GetMessagesForASpecificBot
        // a function to send a message and gets a response back
    }
}
