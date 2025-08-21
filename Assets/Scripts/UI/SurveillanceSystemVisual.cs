using UnityEngine;
using EventHandling.Events;
using EventHandling;

public class SurveillanceSystemVisual : MonoBehaviour
{
    private void Awake()
    {
        EventBus<SecurityCamTurnedOnEvent>.Register(new EventBinding<SecurityCamTurnedOnEvent>(SecCamOn));
        EventBus<SecurityCamTurnedOffEvent>.Register(new EventBinding<SecurityCamTurnedOffEvent>(SecCamOff));
    }


    private void Start()
    {
        
    }


    private void SecCamOn(SecurityCamTurnedOnEvent e)
    {
        this.gameObject.SetActive(true);
        // Additional logic for when the camera turns on can be added here
    }
    private void SecCamOff(SecurityCamTurnedOffEvent e)
    {
        this.gameObject.SetActive(false);
        // Additional logic for when the camera turns off can be added here
    }
}
