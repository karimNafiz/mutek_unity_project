using UnityEngine;
using InteractionSystem;
using System;
using InputActons;
namespace InteractionSystem.Interactions
{
    public class TestInteraction : MonoBehaviour, IInteractable
    {
        public event EventHandler OnInteractionEnd;

        public void Interact(InteractionContext ctx)
        {
            Debug.Log($"performing the interaction -> TestInteraction ");
            InputReader.Instance.OnQuiteInteraction_InteractionMode += Instance_OnQuiteInteraction_InteractionMode;
        }

        private void Instance_OnQuiteInteraction_InteractionMode(bool obj)
        {
            // need to change this this doesnt make sense
            if (!obj) return;
            InputReader.Instance.OnQuiteInteraction_InteractionMode -= Instance_OnQuiteInteraction_InteractionMode;
            OnInteractionEnd?.Invoke(this, EventArgs.Empty);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}