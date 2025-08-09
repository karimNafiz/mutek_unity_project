using UnityEngine;
using EventHandling;
using System.Collections.Generic;
using Models.Bots;
namespace EventHandling.Events
{

    public class BotEvent : IEvent
    {
        public HashSet<Bot> _difference;
    }

    public class OnBotCountIncrease : BotEvent 
    { 
        
    }
    public class OnBotCountDecrease : BotEvent

    { 
    }



}