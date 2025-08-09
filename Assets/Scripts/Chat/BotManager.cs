using System.Collections.Generic;
using Utility.Singleton;
using WebServer;
using Models.Bots;
using System;
/*
    This class will hold all the ChattableBots
    the player can chat with
    making it a monobehavior 

 */
public class BotManager: SingletonMonoBehavior<BotManager>
{

    private HashSet<Bot> bots;


    private void Start()
    {
        bots = new HashSet<Bot>();
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
        this.bots.UnionWith(bots);
    
    }
    /// <summary>
    /// this is the callback that will be run when the get request (to get all the bots) is unsuccessful
    /// </summary>
    private void GetBotsOnErr(Exception e) 
    { 
        
    
    }




}
