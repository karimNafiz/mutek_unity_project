using System;
using Utility.Singleton;
using System.Collections.Generic;
using Models.Bots;
using Utility.HTTP;
using Exceptions;
using UnityEngine;
using Models.Message;

namespace WebServer
{
    public class WebServerClient : SingletonMonoBehavior<WebServerClient>
    {
        
        // need function for GettingBots
        /*need to rethink this shit*/


        // have a class called the ChattableBotsManager -> has all the ChatBots that we can chat with 
        
        // have this function return the list of all the bots 
        public void GetBots(Url url, string endpoint, Action<HashSet<Bot>> onSuccess, Action<Exception> onErr)  
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
                    Debug.Log("Error while parsing json for a bot");
                    Debug.Log($"Bad request Exception {ex.Message}");
                    onErr(ex);
                }
                catch (Exception ex) 
                {
                    // do not really know what to do with this
                    onErr(ex);
                }
            });


            Action<string> onErrCallback = new Action<string>((string err)=> 
            {
                onErr?.Invoke(new BadConnectionException(err));
                
            
            });

            Debug.Log($"trying to make web request endpoint {url.GetHostUrl()}{endpoint}");

            StartCoroutine(HTTPClient.Instance.GetRequest($"{url.GetHostUrl()}{endpoint}", onSuccCallback, onErrCallback));
            // get the json response back 

        }

       
        // rn the bot id is hardcoded into the endpoint
        // TODO: if we have time later, we need to fix it
        public void GetMessages(Url url, string endpoint, Action<List<Message>> onSuccess, Action<Exception> onErr)
        {
            Action<string> onSuccCallback = new Action<string>((string json) =>
            {
                try
                {
                    List<Dictionary<string, object>> dictFrmJson = HTTPClient.Instance.ParseJsonToDictionaryList(json);
                    List<Message> messages = new List<Message>();

                    foreach (Dictionary<string, object> pair in dictFrmJson)
                    {

                        messages.Add(GetMessageFrmJson(pair));
                    }


                }
                catch (Exception ex)
                {

                }

            });



            StartCoroutine(HTTPClient.Instance.GetRequest($"{url.GetHostUrl()}{endpoint}",))



        }


        private Message GetMessageFrmJson(Dictionary<string, object> dict)
        {
            // Validate required keys
            if (!dict.ContainsKey("sender")) throw new BadRequestException("json for message is missing the key sender");
            if (!dict.ContainsKey("receiver")) throw new BadRequestException("json for message is missing the key receiver");
            if (!dict.ContainsKey("message")) throw new BadRequestException("json for message is missing the key message");
            if (!dict.ContainsKey("timestamp")) throw new BadRequestException("json for message is missing the key timestamp");

            try
            {
                // Parse sender
                ChatBot sender;
                if (dict["sender"] is Dictionary<string, object> senderDict)
                {
                    // Reuse your bot parser; relies on Bot being assignable to ChatBot
                    sender = GetBotFrmJson(senderDict);
                }
                else
                {
                    throw new BadRequestException("sender must be a JSON object representing a bot.");
                }

                // Parse receiver
                ChatBot receiver;
                if (dict["receiver"] is Dictionary<string, object> receiverDict)
                {
                    receiver = GetBotFrmJson(receiverDict);
                }
                else
                {
                    throw new BadRequestException("receiver must be a JSON object representing a bot.");
                }

                // Parse message + timestamp
                string msg = dict["message"]?.ToString() ?? string.Empty;
                string ts = dict["timestamp"]?.ToString() ?? string.Empty;

                return new Message(sender, receiver, msg, ts);
            }
            catch (BadRequestException) { throw; }
            catch (FormatException ex) { throw ex; }
            catch (InvalidCastException ex) { throw ex; }
            catch (OverflowException ex) { throw ex; }
        }

        // IMPORTANT TODO: refactor all this bullshit code to it's dedicated file if we are expanding this project
        private Bot GetBotFrmJson(Dictionary<string, object> dict) 
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
