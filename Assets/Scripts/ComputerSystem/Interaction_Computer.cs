using UnityEngine;
using InteractionSystem;
using System;
using EventHandling;
using InputActons;
namespace ComputerSystem {
    public class Interaction_Computer : MonoBehaviour, IInteractable
    {
        public event EventHandler OnInteractionEnd;

        public void Interact(InteractionContext ctx)
        {
            EventBus<OnComputerSystemActivate>.Raise(new OnComputerSystemActivate());
            InputReader.Instance.OnQuiteInteraction_InteractionMode += HandleOnQuiteInteraction_InteractionMode;
        }

        private void HandleOnQuiteInteraction_InteractionMode(bool flag)
        {
            EventBus<OnComputerSystemDeactivate>.Raise(new OnComputerSystemDeactivate());
            InputReader.Instance.OnQuiteInteraction_InteractionMode -= HandleOnQuiteInteraction_InteractionMode;
            OnInteractionEnd?.Invoke(this, EventArgs.Empty);
        }


    }
}