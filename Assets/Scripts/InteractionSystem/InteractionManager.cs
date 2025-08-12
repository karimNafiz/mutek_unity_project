using UnityEngine;

namespace InteractionSystem
{
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private InteractionSensor interactionSensor;

        public InteractionSensor InteractionSensor
        {
            get { return interactionSensor; }
            set { interactionSensor = value; }
        
        }

    }
}