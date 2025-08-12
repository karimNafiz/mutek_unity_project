using UnityEngine;

namespace InteractionSystem
{

    public class InteractionSensor : MonoBehaviour
    {
        [SerializeField] private float castLength;
        [SerializeField] private LayerMask interactionLength;

        private bool isInteractionDetec = false;

    }
}