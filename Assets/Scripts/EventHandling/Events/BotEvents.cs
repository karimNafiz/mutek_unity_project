using UnityEngine;
using EventHandling;
using System.Collections.Generic;
using Models.Bots;
namespace EventHandling.Events
{

    public class BotEvent : IEvent
    {
        public Dictionary<int , Bot> _bots;
    }

    public class OnBotCountIncrease : BotEvent 
    { 
        
    }
    public class OnBotCountDecrease : BotEvent

    { 
    }

    public class OnBotManagerInitialized : BotEvent 
    { 
    
    }



}