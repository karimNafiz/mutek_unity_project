using System.Collections.Generic;
using Models.ChatBots;
using Utility.Singleton;
using WebServer;
/*
    This class will hold all the ChattableBots
    the player can chat with
    making it a monobehavior 

 */
public class ChatFriends: SingletonMonoBehavior<ChatFriends>
{

    private HashSet<ChatBot> friends;


    private void Start()
    {
        // during start
        // we get all the chat bots

    }


    public ChatFriends() 
    { 
        this.friends = new HashSet<ChatBot>();
    }


    public void AddFriend(ChatBot bot) 
    {
        if (this.friends.Contains(bot)) return;
        this.friends.Add(bot);  
    }

    //private void GetBotsFrmWebServer() 
    //{ 
    //    WebServerClient.Instance.GetBots            
    
    //}




}
