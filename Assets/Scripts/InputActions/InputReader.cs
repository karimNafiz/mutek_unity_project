using UnityEngine;
using System;
using Utility.Singleton;
namespace InputActons
{
    public class InputReader : SingletonMonoBehavior<InputReader> 
    {
        private PlayerInputActions playerInputActions;
        public event Action<Vector2> OnPlayerMove_FirstPersonMode;
        public event Action<Vector2> OnPlayerLook_FirstPersonMode;
        public event Action<bool> OnPlayerInteract_FirstPersonMode;

        // maybe we can keep track of the game state and according to the game state we can change the bindings


        protected override void Awake()
        {
            base.Awake();
            // create a new instance of the class
            playerInputActions = new PlayerInputActions();
            // TODO: later on when we introduce more stuff, we take care of enabling which action map to use
            playerInputActions.FirstPersonMovement.Enable();
            playerInputActions.FirstPersonMovement.Movement.performed += MovementPerformed_FirstPersonMode;
            playerInputActions.FirstPersonMovement.Looking.performed += LookingPeformed_FirstPersonMode;
            playerInputActions.FirstPersonMovement.Interaction.performed += InteractonPerformed_FirstPersonMode;


        }

        private void InteractonPerformed_FirstPersonMode(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnPlayerInteract_FirstPersonMode?.Invoke(obj.ReadValue<bool>());
        }

        private void LookingPeformed_FirstPersonMode(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnPlayerLook_FirstPersonMode?.Invoke(obj.ReadValue<Vector2>());
        }

        private void MovementPerformed_FirstPersonMode(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnPlayerMove_FirstPersonMode?.Invoke(obj.ReadValue<Vector2>());
        }
    }
}