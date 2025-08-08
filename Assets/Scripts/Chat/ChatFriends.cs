using System.Collections.Generic;
using Models.ChatBots;


public class ChatFriends
{
    public static ChatFriends instance;
    public static ChatFriends Instance
    {
        get { return instance; }
    }
    private HashSet<ChatBot> friends;

    public ChatFriends() 
    { 
        this.friends = new HashSet<ChatBot>();
    }


    public void AddFriend(ChatBot bot) 
    {
        if (this.friends.Contains(bot)) return;
        this.friends.Add(bot);  
    }





}
