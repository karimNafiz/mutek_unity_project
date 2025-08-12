using UnityEngine;
using InputActons;
namespace AdvancedController
{


    namespace StarterAssets
    {
        public class FirstPersonCamera : MonoBehaviour
        {
            [Header("Cinemachine")]
            [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
            public GameObject CinemachineCameraTarget;
            [Tooltip("How far in degrees can you move the camera up")]
            public float TopClamp = 90.0f;
            [Tooltip("How far in degrees can you move the camera down")]
            public float BottomClamp = -90.0f;
            [Tooltip("Rotation speed of the camera")]
            public float RotationSpeed = 1.0f;

            private float _cinemachineTargetPitch;
            private float _rotationVelocity;

            


            private const float _threshold = 0.01f;

            private Vector2 mouseInput = Vector2.zero;
            private void Awake()
            {
                Cursor.lockState = CursorLockMode.Locked;
                // Hide the cursor
                Cursor.visible = false;
            }
            private void Start()
            {
                InputReader.Instance.OnPlayerLook_FirstPersonMode += Instance_OnPlayerLook_FirstPersonMode;

            }

            private void Instance_OnPlayerLook_FirstPersonMode(Vector2 obj)
            {
                this.mouseInput = obj;
            }

            private void LateUpdate()
            {
                CameraRotation();
            }

            private void CameraRotation()
            {
                if (mouseInput.sqrMagnitude >= _threshold)
                {
                    

                    // vertical rotation (pitch)
                    _cinemachineTargetPitch += mouseInput.y * RotationSpeed * Time.deltaTime;
                    _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
                    CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch *-1, 0.0f, 0.0f);

                    // horizontal rotation (yaw)
                    _rotationVelocity = mouseInput.x * RotationSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.up * _rotationVelocity);
                }
            }

            private static float ClampAngle(float angle, float min, float max)
            {
                if (angle < -360f) angle += 360f;
                if (angle > 360f) angle -= 360f;
                return Mathf.Clamp(angle, min, max);
            }
        }
    }

}