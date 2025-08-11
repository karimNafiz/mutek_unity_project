using UnityEngine;
using InputActons;

namespace AdvancedController
{
    [RequireComponent(typeof(PlayerMover))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Input / Camera")]
        [SerializeField] Transform cameraTransform;   // First-person camera (its forward/right define movement)
                                                      // Optional: your existing input reader with Direction (Vector2)

        [Header("Movement")]
        [SerializeField] float movementSpeed = 7f;    // Horizontal move speed
        [SerializeField] float gravity = 10f;         // Downward accel (since rb.useGravity=false in PlayerMover)
        [SerializeField] float groundSnapExtra = 0.5f;// Tiny extra down push to stick to gentle slopes

        Vector2 inputMap;
        PlayerMover mover;


        Transform tr;

        // We keep a very simple vertical velocity; no momentum or states.
        float verticalVelocity;

        void Awake()
        {
            tr = transform;
            mover = GetComponent<PlayerMover>();
        }

        void Start()
        {
            // If you use a custom InputReader that needs enabling, do it here:
            // input?.EnablePlayerActions();
            inputMap = new Vector2(0, 0);

            InputReader.Instance.OnPlayerMove_FirstPersonMode += Input_OnPlayerMove_FirstPersonMode;
        }

        private void Input_OnPlayerMove_FirstPersonMode(Vector2 obj)
        {
            inputMap = obj;
        }

        void FixedUpdate()
        {
            // 1) Update ground status from the mover’s sensor
            mover.CheckForGround();
            bool isGrounded = mover.IsGrounded();

            // 2) Horizontal velocity from input (camera-relative, flattened)
            Vector3 moveVel = CalculateMovementVelocity();

            // 3) Simple gravity handling
            if (isGrounded && verticalVelocity < 0f)
            {
                // When grounded and moving downward, zero-out vertical so we don't accumulate downward velocity
                verticalVelocity = 0f;
            }
            else
            {
                verticalVelocity -= gravity * Time.fixedDeltaTime;
            }

            // 4) Compose final velocity (horizontal + vertical)
            Vector3 finalVelocity = moveVel + tr.up * verticalVelocity;

            // Optional tiny extra stick to ground (helps on gentle slopes)
            if (isGrounded && verticalVelocity <= 0f)
                finalVelocity -= tr.up * groundSnapExtra;

            // 5) Let the mover add its ground-adjustment velocity & apply to Rigidbody
            mover.SetExtendSensorRange(isGrounded);
            mover.SetVelocity(finalVelocity);
        }

        // ------- Helpers -------

        Vector3 CalculateMovementVelocity()
        {
            Vector3 dir = CalculateMovementDirection();
            return dir * movementSpeed;
        }

        Vector3 CalculateMovementDirection()
        {
            // If no camera assigned, fall back to player transform axes
            if (cameraTransform == null)
            {
                Vector3 localRight = tr.right;
                Vector3 localForward = tr.forward;

                Vector3 d = localRight * GetInput().x + localForward * GetInput().y;
                return d.sqrMagnitude > 1f ? d.normalized : d;
            }

            // First-person: use camera forward/right, flattened onto the player’s up plane
            Vector3 flatRight = Vector3.ProjectOnPlane(cameraTransform.right, tr.up).normalized;
            Vector3 flatForward = Vector3.ProjectOnPlane(cameraTransform.forward, tr.up).normalized;

            Vector3 dir = flatRight * GetInput().x + flatForward * GetInput().y;
            return dir.sqrMagnitude > 1f ? dir.normalized : dir;
        }

        Vector2 GetInput()
        {
            // inputMap is updated by the event
            return inputMap;
        }
    }
}
