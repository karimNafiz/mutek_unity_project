using UnityEngine;
using System;
using Utility; // for RaycastSensor

namespace InteractionSystem
{
    public class InteractionSensor : MonoBehaviour
    {
        [SerializeField] private float castLength = 3f;
        [SerializeField] private LayerMask interactionMask = ~0; // default: everything

        private Camera playerCamera;
        private RaycastSensor raycastSensor;

        private IInteractable currentTarget;

        public event EventHandler<IInteractable> OnInteractionDetect;
        public event EventHandler<IInteractable> OnInteractionDetectionEnd;

        private void Awake()
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("InteractionSensor: No main camera found!");
                enabled = false;
                return;
            }

            // Create RaycastSensor using camera transform
            raycastSensor = new RaycastSensor(playerCamera.transform);
            raycastSensor.castLength = castLength;
            raycastSensor.layermask = interactionMask;
            raycastSensor.SetCastDirection(RaycastSensor.CastDirection.Forward);
        }

        private void LateUpdate()
        {
            // every late update check for detection
            DetectInteraction();
            raycastSensor.DrawDebugAlways();
        }

        private void DetectInteraction()
        {
            // Origin is camera position
            // set the cast origin to the camers transform position
            // the camera might move
            // so its important
            raycastSensor.SetCastOrigin(playerCamera.transform.position);

            // Cast ray
            raycastSensor.Cast();
            // if we hit something interactable
            if (raycastSensor.HasDetectedHit())
            {
                var hitTransform = raycastSensor.GetTransform();
                var interactable = hitTransform.GetComponentInParent<IInteractable>();

                if (interactable != null)
                {
                    if (interactable != currentTarget)
                    {
                        // New target
                        EndCurrentTarget();
                        currentTarget = interactable;
                        OnInteractionDetect?.Invoke(this, currentTarget);
                    }
                }
                else
                {
                    EndCurrentTarget();
                }
            }
            // if we don't hit something
            else
            {
                EndCurrentTarget();
            }

            // Debug visuals
            raycastSensor.DrawDebug();
        }

        private void EndCurrentTarget()
        {
            if (currentTarget != null)
            {
                OnInteractionDetectionEnd?.Invoke(this, currentTarget);
                currentTarget = null;
            }
        }
    }
}
