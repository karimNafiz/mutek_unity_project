using EventHandling;
using UnityEngine;
using UnityEngine.UI;
using EventHandling.Events;

namespace UI {
    public class SuspicionSystemVisual : MonoBehaviour
    {
        [SerializeField] private Slider surveillanceSlider;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private void Awake()
        {
            if (surveillanceSlider == null)
            {
                Debug.LogError("Surveillance Slider is not assigned in the inspector.");
            }
            EventBus<SuspicionChangedEvent>.Register(new EventBinding<SuspicionChangedEvent>(EventBus_OnSuspicionValueChanged));
        }


        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void EventBus_OnSuspicionValueChanged(SuspicionChangedEvent e) 
        {
            float delta = e.Delta / e.MaxValue;
            surveillanceSlider.value += delta;
            surveillanceSlider.value = Mathf.Clamp01(surveillanceSlider.value);

        }
    }
}