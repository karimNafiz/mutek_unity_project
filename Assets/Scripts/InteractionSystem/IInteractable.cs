using UnityEngine;
using System;
namespace InteractionSystem
{
    public struct InteractionContext { }

    public interface IInteractable
    {
        // to notify if the interaction is complete
        public event EventHandler OnInteractionEnd;
        // function to call when to start the interaction 
        public void Interact(InteractionContext ctx);
    }
}