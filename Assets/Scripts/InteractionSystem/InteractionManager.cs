using UnityEngine;
using InputActons;
using EventHandling;
using EventHandling.Events;
using GameStates;

namespace InteractionSystem
{
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private InteractionSensor interactionSensor;
        private IInteractable currentInteractable;

        // gay ass public property
        public InteractionSensor InteractionSensor
        {
            get { return interactionSensor; }
            set 
            {
                // that means we are changing the interaction sensor
                if (interactionSensor != null) 
                {
                    UnsubscribeToInteractionSensor();
                }
            
                interactionSensor = value;
                SubscribeToInteractionSensor();
            }
        
        }



        private void Start()
        {
            InputReader.Instance.OnPlayerInteract_FirstPersonMode += Instance_OnPlayerInteract_FirstPersonMode;
            
            if (interactionSensor == null) return;
            // I know this looks weird please check the property this will make more sense 
            InteractionSensor = interactionSensor;

        }

        private void Instance_OnPlayerInteract_FirstPersonMode(bool obj)
        {
            Debug.Log("Interaction button pressed ");
            Debug.Log($"obj value -> {obj}");
            if (!obj) return;
            if (currentInteractable == null) return;
            Debug.Log("should run interactions ");
            // first Unsubscribe to InteractionSensor
            UnsubscribeToInteractionSensor();
            // listen to the interaction OnInteractionEnd event
            currentInteractable.OnInteractionEnd += CurrentInteractable_OnInteractionEnd;
            // need to change to game state

            Debug.Log($"Starting Interaction from the interaction manager ");
            EventBus<OnGameStateChange>.Raise(new OnGameStateChange()
            {
                _gameState = EGameState.INTERACTION
            }) ;
            currentInteractable.Interact(new InteractionContext()); // right now providing an emtpy context 

        }

        private void CurrentInteractable_OnInteractionEnd(object sender, System.EventArgs e)
        {
            currentInteractable.OnInteractionEnd -= CurrentInteractable_OnInteractionEnd;
            EventBus<OnGameStateChange>.Raise(new OnGameStateChange()
            {
                _gameState = EGameState.FIRSTPERSONMOVEMENT
            });
            currentInteractable = null;
            SubscribeToInteractionSensor();

        }

        private void SubscribeToInteractionSensor() 
        {
            interactionSensor.OnInteractionDetect += InteractionSensor_OnInteractionDetect;
            interactionSensor.OnInteractionDetectionEnd += InteractionSensor_OnInteractionDetectionEnd;

        }
        private void UnsubscribeToInteractionSensor() 
        {
            interactionSensor.OnInteractionDetect -= InteractionSensor_OnInteractionDetect;
            interactionSensor.OnInteractionDetectionEnd -= InteractionSensor_OnInteractionDetectionEnd;


        }



        private void InteractionSensor_OnInteractionDetectionEnd(object sender, IInteractable e)
        {
            currentInteractable = null;
        }

        private void InteractionSensor_OnInteractionDetect(object sender, IInteractable e)
        {
            if (e == null) return;
            currentInteractable = e;
        }
    }
}