
using System;

namespace EventHandling 
{
    public interface IEventBinding<T> where T : IEvent 
    {
        Action<T> OnEvent { get; set; }
        Action OnEventNoArgs { get; set; }


    }

    public class EventBinding<T> : IEventBinding<T> where T : IEvent 
    {

        // assigning the private fields to empty delegates to avoid null checks
        Action<T> onEvent = _ => { };
        Action onEventNoArgs = () => { };

        // implementing the interface properties
        Action<T> IEventBinding<T>.OnEvent 
        {
            get { return onEvent; }
            set { onEvent = value; }    
        }

        Action IEventBinding<T>.OnEventNoArgs
        {
            get { return onEventNoArgs; }
            set { onEventNoArgs = value; }
        }

        // constructor to initialize the event bindings 
        public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;
        public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

        public void Add(Action onEvent) => onEventNoArgs += onEvent;
        public void Remove(Action onEvent) => onEventNoArgs -= onEvent;

        public void Add(Action<T> onEvent) => this.onEvent += onEvent;
        public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;



    }

}
