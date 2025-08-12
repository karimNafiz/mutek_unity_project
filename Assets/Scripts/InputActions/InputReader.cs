using UnityEngine;
using System;
using Utility.Singleton;
using EventHandling;
using EventHandling.Events;
using GameStates;

namespace InputActons
{
    public class InputReader : SingletonMonoBehavior<InputReader>
    {
        private PlayerInputActions playerInputActions;

        // First-person state
        public event Action<Vector2> OnPlayerMove_FirstPersonMode;
        public event Action<Vector2> OnPlayerLook_FirstPersonMode;
        public event Action<bool> OnPlayerInteract_FirstPersonMode;

        // Interaction state
        public event Action<bool> OnQuiteInteraction_InteractionMode;

        private EventBinding<OnGameStateChange> eventBinding_OnGameStateChange;

        protected override void Awake()
        {
            base.Awake();

            eventBinding_OnGameStateChange = new EventBinding<OnGameStateChange>(EventBus_OnGameStateChange);
            EventBus<OnGameStateChange>.Register(eventBinding_OnGameStateChange);

            playerInputActions = new PlayerInputActions();

            // --- First person map
            playerInputActions.FirstPersonMovement.Movement.performed += Movement_Performed;
            playerInputActions.FirstPersonMovement.Movement.canceled += Movement_Canceled;

            playerInputActions.FirstPersonMovement.Looking.performed += Looking_Performed;
            playerInputActions.FirstPersonMovement.Looking.canceled += Looking_Canceled;

            playerInputActions.FirstPersonMovement.Interaction.performed += Interaction_Performed;
            playerInputActions.FirstPersonMovement.Interaction.canceled += Interaction_Canceled;

            // --- Interaction map
            playerInputActions.Interaction.QuiteInteraction.performed += QuitInteraction_Performed;
            playerInputActions.Interaction.QuiteInteraction.canceled += QuitInteraction_Canceled;

            // If you want a default map enabled before the first game-state event, uncomment:
            // playerInputActions.FirstPersonMovement.Enable();
        }

        private void OnDestroy()
        {
            // Unregister from event bus
            if (eventBinding_OnGameStateChange != null)
                EventBus<OnGameStateChange>.Deregister(eventBinding_OnGameStateChange);

            // Unsubscribe input callbacks (good hygiene)
            if (playerInputActions != null)
            {
                playerInputActions.FirstPersonMovement.Movement.performed -= Movement_Performed;
                playerInputActions.FirstPersonMovement.Movement.canceled -= Movement_Canceled;

                playerInputActions.FirstPersonMovement.Looking.performed -= Looking_Performed;
                playerInputActions.FirstPersonMovement.Looking.canceled -= Looking_Canceled;

                playerInputActions.FirstPersonMovement.Interaction.performed -= Interaction_Performed;
                playerInputActions.FirstPersonMovement.Interaction.canceled -= Interaction_Canceled;

                playerInputActions.Interaction.QuiteInteraction.performed -= QuitInteraction_Performed;
                playerInputActions.Interaction.QuiteInteraction.canceled -= QuitInteraction_Canceled;
            }
        }

        // ---- Game state switching
        private void EventBus_OnGameStateChange(OnGameStateChange state)
        {
            switch (state._gameState)
            {
                case EGameState.FIRSTPERSONMOVEMENT:
                    playerInputActions.FirstPersonMovement.Enable();
                    playerInputActions.Interaction.Disable();
                    break;

                case EGameState.INTERACTION:
                    playerInputActions.FirstPersonMovement.Disable();
                    playerInputActions.Interaction.Enable();
                    break;
            }
        }

        // ---- Movement (Vector2)
        private void Movement_Performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            OnPlayerMove_FirstPersonMode?.Invoke(ctx.ReadValue<Vector2>());
        }
        private void Movement_Canceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            OnPlayerMove_FirstPersonMode?.Invoke(Vector2.zero);
        }

        // ---- Looking (Vector2)
        private void Looking_Performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            OnPlayerLook_FirstPersonMode?.Invoke(ctx.ReadValue<Vector2>());
        }
        private void Looking_Canceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            OnPlayerLook_FirstPersonMode?.Invoke(Vector2.zero);
        }

        // ---- Interaction button (bool)
        private void Interaction_Performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            // Either use phases, or:
            // bool pressed = ctx.ReadValueAsButton();
            OnPlayerInteract_FirstPersonMode?.Invoke(true);
        }
        private void Interaction_Canceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            OnPlayerInteract_FirstPersonMode?.Invoke(false);
        }

        // ---- Quit Interaction (bool)
        private void QuitInteraction_Performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            OnQuiteInteraction_InteractionMode?.Invoke(true);
        }
        private void QuitInteraction_Canceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            OnQuiteInteraction_InteractionMode?.Invoke(false);
        }
    }
}
