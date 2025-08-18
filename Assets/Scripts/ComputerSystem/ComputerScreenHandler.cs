using EventHandling;
using UnityEngine;
using UnityEngine.UI;


namespace ComputerSystem
{
    /*
        this class represents the black screen infront of the compter screen
        if the computer system is activated we turn it off 
        if the computer system is deactivated we turn it on
     
     */


    [RequireComponent(typeof(Image))]
    public class ComputerScreenHandler : MonoBehaviour
    {
        private EventBinding<OnComputerSystemActivate> eventBinding_OnComputerSystemActivate;
        private EventBinding<OnComputerSystemDeactivate> eventBinding_OnComputerSystemDeactivate;
        private Image screenImg;
        private void Awake()
        {
            eventBinding_OnComputerSystemActivate = new EventBinding<OnComputerSystemActivate>(EventBus_OnComputerSystemActivate);
            eventBinding_OnComputerSystemDeactivate = new EventBinding<OnComputerSystemDeactivate>(EventBus_OnComputerSystemDeactivate);
        }
        private void Start()
        {
            screenImg = GetComponent<Image>();  
        }

        private void EventBus_OnComputerSystemActivate(OnComputerSystemActivate args) 
        {
            if (screenImg == null) return;
            Color c = screenImg.color;
            c.a = 0.0f; // Set the alpha to fully transparent
            screenImg.color = c; // Apply the color change
        }
        private void EventBus_OnComputerSystemDeactivate(OnComputerSystemDeactivate args) 
        { 
            if(screenImg == null) return;
            Color c = screenImg.color;
            c.a = 1.0f; // Set the alpha to fully opaque
            screenImg.color = c; // Apply the color change
        }
    }
}

