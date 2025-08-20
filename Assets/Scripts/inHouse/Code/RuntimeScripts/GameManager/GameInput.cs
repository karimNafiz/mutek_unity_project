using System;
using System.Collections;
using UnityEngine;
using Utility;

namespace GameManager
{
    /// <summary>
    /// Centralized input manager that wraps Unity Input System with locking, event broadcasting,
    /// and user input abstractions for UI and player controls.
    /// </summary>
    public class GameInput : SingletonMonoBehaviour<GameInput>
    {
        // Lock-related events
        /// <summary>
        /// Triggered when input becomes locked.
        /// </summary>
        public event EventHandler OnInputLock;

        /// <summary>
        /// Triggered when input is unlocked.
        /// </summary>
        public event EventHandler OnInputUnLock;

        // action events
        public event EventHandler OnLMBPressed;
        public event EventHandler OnLMBReleased;
        public event EventHandler OnRMBPressed;
        public event EventHandler OnRMBReleased;
        public event EventHandler OnEnterPressed;

        private CityPlayerInputMaps inputActions;
        private int lockCount;

        /// <summary>
        /// Initializes input actions and enables action maps.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            inputActions = new CityPlayerInputMaps();
            inputActions.Cinemachine.Enable();
            inputActions.UI.Enable();
            inputActions.Player.Enable();
        }


        /// <summary>
        /// Registers callbacks for input actions from the new input system.
        /// </summary>
        private void Start()
        {
            // register callback to input system so we know when actions are performed
            inputActions.Player.LMBClick.performed += LMBClick_performed;
            inputActions.Player.LMBClick.canceled += LMBClick_canceled;
            inputActions.Player.RMBClick.performed += RMBClick_performed;
            inputActions.Player.RMBClick.canceled += RMBClick_canceled;
            inputActions.UI.Enter.performed += Enter_performed;
        }


        #region Lock
        // Lock Management
        private bool IsInputLocked()
        {
            return lockCount > 0;
        }

        private void AddLock()
        {
            OnInputLock?.Invoke(this, EventArgs.Empty);
            lockCount++;
        }

        private void RemoveLock()
        {
            if (lockCount > 0)
            {
                lockCount--;
            }


            if (lockCount == 0)
            {
                OnInputUnLock?.Invoke(this, EventArgs.Empty);
            }
        }
        private IEnumerator AutoLockCoroutine(float time)
        {
            // Debug.Log($"++++Locking Inputs for :{time} seconds");
            AddLock();

            yield return new WaitForSeconds(time);

            // Debug.Log("----Unlocking Inputs");
            RemoveLock();
        }


        /// <summary>
        /// Temporarily locks player input for a specified duration.
        /// </summary>
        /// <param name="time">Duration in seconds to lock input.</param>
        public void LockForSeconds(float time)
        {
            StartCoroutine(AutoLockCoroutine(time));
        }

        public void LockInput() 
        {
            AddLock();
        
        }
        public void UnLockInput() 
        {
            RemoveLock();
        }


        [ContextMenu("Test Lock Input")]
        public void TestLockInput()
        {
            LockForSeconds(5);
        }

        #endregion


        #region UI Input Actions
        /// ARROW
        public bool IsArrowUpHeld()
        {
            return !IsInputLocked() && inputActions.UI.ArrowUp.IsPressed();
        }

        public bool IsArrowDownHeld()
        {
            return !IsInputLocked() && inputActions.UI.ArrowDown.IsPressed();
        }

        public bool IsLeftArrowKeyPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.UI.LeftArrow.WasPerformedThisFrame();
        }

        public bool IsRightArrowKeyPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.UI.RightArrow.WasPerformedThisFrame();
        }


        /// <summary>
        /// Checks if a number key (0�C9) was pressed this frame.
        /// </summary>
        public bool IsNumberIndexKeyPressedThisFrame(int number)
        {
            switch (number)
            {
                case 0:
                    return IsZeroKeyPressedThisFrame();
                case 1:
                    return IsOneKeyPressedThisFrame();
                case 2:
                    return IsTwoKeyPressedThisFrame();
                case 3:
                    return IsThreeKeyPressedThisFrame();
                case 4:
                    return IsFourKeyPressedThisFrame();
                case 5:
                    return IsFiveKeyPressedThisFrame();
                case 6:
                    return IsSixKeyPressedThisFrame();
                case 7:
                    return IsSevenKeyPressedThisFrame();
                case 8:
                    return IsEightKeyPressedThisFrame();
                case 9:
                    return IsNineKeyPressedThisFrame();
                default:
                    Debug.Log("IsNumberIndexKeyPressedThisFrame failed, returning false");
                    return false;
            }
        }

        private bool IsZeroKeyPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.UI.Zero0.WasPerformedThisFrame();
        }
        private bool IsOneKeyPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.UI.One1.WasPerformedThisFrame();
        }
        private bool IsTwoKeyPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.UI.Two2.WasPerformedThisFrame();
        }
        private bool IsThreeKeyPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.UI.Three3.WasPerformedThisFrame();
        }
        private bool IsFourKeyPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.UI.Four4.WasPerformedThisFrame();
        }
        private bool IsFiveKeyPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.UI.Five5.WasPerformedThisFrame();
        }
        private bool IsSixKeyPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.UI.Six6.WasPerformedThisFrame();
        }
        private bool IsSevenKeyPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.UI.Seven7.WasPerformedThisFrame();
        }
        private bool IsEightKeyPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.UI.Eight8.WasPerformedThisFrame();
        }
        private bool IsNineKeyPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.UI.Nine9.WasPerformedThisFrame();
        }
        #endregion



        #region Player Input Actions
        // Move, look, jump, sprint data fetching for the FirstPersonController

        /// <summary>
        /// Returns the normalized 2D movement vector from player input.
        /// </summary>
        public Vector2 GetMoveVectorNormalized()
        {
            if (IsInputLocked())
            {
                return Vector2.zero;
            }
            else
            {
                return inputActions.Player.Move.ReadValue<Vector2>().normalized;
            }
        }

        /// <summary>
        /// Returns the 2D look vector (e.g., mouse delta).
        /// </summary>
        public Vector2 GetLookVector()
        {
            if (IsInputLocked())
            {
                return Vector2.zero;
            }
            else
            {
                return inputActions.Player.Look.ReadValue<Vector2>();
            }
        }

        public bool IsJumpPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.Player.Jump.WasPerformedThisFrame();
        }

        public bool IsSprintHeld()
        {
            return inputActions.Player.Sprint.IsPressed();
        }


        // LMB checks
        /// <summary>
        /// Checks whether the left mouse button was clicked this frame.
        /// </summary>
        public bool IsLMBPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.Player.LMBClick.WasPerformedThisFrame();
        }


        private void LMBClick_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (!IsInputLocked())
            {
                OnLMBPressed?.Invoke(obj, EventArgs.Empty);
            }
        }

        private void LMBClick_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (!IsInputLocked())
            {
                OnLMBReleased?.Invoke(obj, EventArgs.Empty);
            }
        }



        // RMB checks
        public bool IsRMBPressedThisFrame()
        {
            return !IsInputLocked() && inputActions.Player.RMBClick.WasPerformedThisFrame();
        }

        private void RMBClick_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (!IsInputLocked())
            {
                OnRMBPressed?.Invoke(obj, EventArgs.Empty);
            }
        }

        private void RMBClick_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (!IsInputLocked())
            {
                OnRMBReleased?.Invoke(obj, EventArgs.Empty);
            }
        }

        private void Enter_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (!IsInputLocked())
            {
                OnEnterPressed?.Invoke(obj, EventArgs.Empty);
            }
        }


        public bool IsEscapeKeyPerformedThisFrame()
        {
            return !IsInputLocked() && inputActions.Player.Escape.WasPerformedThisFrame();
        }

        public bool IsControlKeyPressed()
        {
            return !IsInputLocked() && inputActions.Player.Control.IsPressed();
        }
        public bool IsEKeyPerformedThisFrame()
        {
            return !IsInputLocked() && inputActions.Player.E.WasPerformedThisFrame();
        }

        public bool IsQKeyPerformedThisFrame()
        {
            return !IsInputLocked() && inputActions.Player.Q.WasPerformedThisFrame();
        }

        public bool IsRKeyPerformedThisFrame()
        {
            return !IsInputLocked() && inputActions.Player.R.WasPerformedThisFrame();
        }
        public bool IsGKeyPerformedThisFrame()
        {
            return !IsInputLocked() && inputActions.Player.G.WasPerformedThisFrame();
        }



        #endregion

    }
}