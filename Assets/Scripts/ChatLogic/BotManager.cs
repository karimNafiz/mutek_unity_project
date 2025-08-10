using System.Collections.Generic;
using Utility.Singleton;
using WebServer;
using Models.Bots;
using System;
using EventHandling;
using EventHandling.Events;
using Exceptions;
using Models.Message;
using UnityEngine;
using Models.Users;
/*
    This class will hold all the ChattableBots
    the player can chat with
    making it a monobehavior 

 */
public class BotManager: SingletonMonoBehavior<BotManager>
{

    private HashSet<Bot> bots;
    private Dictionary<int, Bot> botsDict;
    private int botCount;
    private User user;
    private bool isFirstLoad;

    public User User
    {
        get { return user; }
        set { user = value; }
    
    }


    private void Start()
    {
        bots = new HashSet<Bot>();
        botsDict = new Dictionary<int, Bot>();
        botCount = 0;
        isFirstLoad = true;
        // this function here will fill up the variables bots and botsDict
        GetBotsFrmWebServer();
        Debug.Log($"got the bots ");



    }


    public void AddBot(Bot bot) 
    {
        if (this.bots.Contains(bot)) return;
        this.bots.Add(bot);  
    }


    /// <summary>
    /// the role of this function to make a Get Request to the webser to get all the bots
    /// </summary>
    private void GetBotsFrmWebServer()
    {
        WebServerClient.Instance.GetBots(GlobalConfigs.Instance.GetServerUrl(), GlobalConfigs.Instance.globalConstant.bots_endpoint_get, GetBotsOnSuccess, GetBotsOnErr);

    }
    /// <summary>
    /// this is the callback that will be run when the get request is successful, and we parsed all the json into Bots
    /// I have the written the code, in such a way that we will get back the
    /// </summary>
    // TODO: due to the fact that everything is a pointer, make sure to not making too many changes to keep bullshit errors to a minimum 

    private void GetBotsOnSuccess(HashSet<Bot> incomingBots)
    {
        // Build a map from the server result
        Dictionary<int, Bot> incoming = new Dictionary<int, Bot>();
        foreach (var b in incomingBots)
            incoming[b.ID] = b;

        // Compute differences
        var added = new Dictionary<int, Bot>();
        var removed = new Dictionary<int, Bot>();

        // Added or updated
        foreach (var kv in incoming)
        {
            int id = kv.Key;
            var bot = kv.Value;

            if (!botsDict.ContainsKey(id))
            {
                added[id] = bot;
            }
        }

        // Removed
        foreach (var kv in botsDict)
        {
            if (!incoming.ContainsKey(kv.Key))
                removed[kv.Key] = kv.Value;
        }

        // Apply changes to local state
        foreach (var kv in added)
        {
            bots.Add(kv.Value);          // consider removing HashSet entirely (see note below)
            botsDict[kv.Key] = kv.Value;
        }
        foreach (var kv in removed)
        {
            bots.Remove(kv.Value);
            botsDict.Remove(kv.Key);
        }

        botCount = botsDict.Count;

        // Now raise the correct events
        if (this.isFirstLoad)
        {
            this.isFirstLoad = false;
            EventBus<OnBotManagerInitialized>.Raise(new OnBotManagerInitialized
            {
                _bots = new Dictionary<int, Bot>(botsDict)
            });
            
        }
        else
        {
            if (added.Count > 0)
                EventBus<OnBotCountIncrease>.Raise(new OnBotCountIncrease { _bots = added });

            if (removed.Count > 0)
                EventBus<OnBotCountDecrease>.Raise(new OnBotCountDecrease { _bots = removed });
        }
    }








/// <summary>
/// this is the callback that will be run when the get request (to get all the bots) is unsuccessful
/// </summary>
    private void GetBotsOnErr(Exception e) 
    { 
        
    
    }

    //public List<Message> GetMessagesWithBot(int botID)
    //{
    //    if (!this.botsDict.ContainsKey(botID)) 
    //    {
    //        throw new BotDoesNotExistException(botID);
    //    }
    //    return GetMessagesWithBot(this.botsDict[botID]);
        
    
    //}
    
    //// task this function needs to make a web request 
    //public List<Message> GetMessagesWithBot(Bot bot) 
    //{ 

    //    WebSer
        
    
    //}




}
