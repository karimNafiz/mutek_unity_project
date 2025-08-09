using System.Collections.Generic;
using Utility.Singleton;
using WebServer;
using Models.Bots;
using System;
using EventHandling;
using EventHandling.Events;
using UnityEngine;
/*
    This class will hold all the ChattableBots
    the player can chat with
    making it a monobehavior 

 */
public class BotManager: SingletonMonoBehavior<BotManager>
{

    private HashSet<Bot> bots;
    private int botCount;


    private void Start()
    {
        bots = new HashSet<Bot>();
        botCount = 0;
        GetBotsFrmWebServer();


    }


    public BotManager() 
    { 
        this.bots = new HashSet<Bot>();
    }


    public void AddFriend(Bot bot) 
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

    private void GetBotsOnSuccess(HashSet<Bot> bots) 
    {
        // we first need to check if the number of bots have changed or not
        // if the number hasn't changed then we don't really do anything
        // i know checking the number isn't the most safe way
        // but we are prototyping
        if (bots.Count == this.botCount) return;
        // if the new bot count is more then we have more bots 
        HashSet<Bot> difference;
        if (bots.Count > this.botCount)
        {
            // if there are more bots than what the bots manager has
            // then the difference hashset has to be created from the incoming bot hashset
            // because we want to find out the new bots except for the ones already in this.bots
            difference = new HashSet<Bot>(bots);
            difference.ExceptWith(this.bots);
            foreach (Bot bot in difference)
            {
                this.bots.Add(bot);
            }
            EventBus<OnBotCountIncrease>.Raise(new OnBotCountIncrease()
            {
                _difference = difference
            }) ;
            return;
        }
        else 
        {
            // read the comment for the if statement
            // in the else block the logic for the if statement is reverse
            difference = new HashSet<Bot>(this.bots);
            difference.ExceptWith(bots);
            foreach (Bot bot in difference) 
            {
                this.bots.Remove(bot);
            }
            EventBus<OnBotCountDecrease>.Raise(new OnBotCountDecrease()
            {
                _difference = difference

            }) ;
            return;
        }





        
    
    }
    /// <summary>
    /// this is the callback that will be run when the get request (to get all the bots) is unsuccessful
    /// </summary>
    private void GetBotsOnErr(Exception e) 
    { 
        
    
    }




}
