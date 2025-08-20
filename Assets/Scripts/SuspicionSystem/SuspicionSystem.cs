using UnityEngine;
using EventHandling;
using EventHandling.Events; 

namespace SuspicionSystem
{
    /// <summary>
    /// Core suspicion logic: clamp, bump up/down, and fire events when crossing threshold.
    /// Keep this on a single GameObject in the scene (logical singleton by design, but not static).
    /// </summary>
    public class SuspicionSystem : MonoBehaviour
    {
        [Header("Config")]
        [Tooltip("Starting suspicion value on play.")]
        [SerializeField, Min(0f)] private float startingValue = 0f;

        [Tooltip("Maximum possible suspicion (hard clamp).")]
        [SerializeField, Min(1f)] private float maxValue = 100f;

        [Tooltip("Crossing this from below triggers SuspicionTooHighEvent.")]
        [SerializeField, Min(0f)] private float threshold = 60f;

        [Header("State (read-only)")]
        [SerializeField, ReadOnlyInInspector] private float currentValue = 0f;
        [SerializeField, ReadOnlyInInspector] private bool isAtOrAboveThreshold = false;

        // --- Public API ------------------------------------------------------

        /// <summary>Current suspicion value (0..maxValue).</summary>
        public float Current => currentValue;

        /// <summary>Threshold that triggers the "too high" event when crossed upwards.</summary>
        public float Threshold => threshold;

        /// <summary>Hard maximum clamp for suspicion.</summary>
        public float MaxValue => maxValue;

        /// <summary>
        /// Increase suspicion by amount (post-clamped). Fires change event, and if the value
        /// crosses the threshold from below, fires SuspicionTooHighEvent exactly once.
        /// </summary>
        public void Increase(float amount, string reason = "")
        {
            if (amount <= 0f) return;
            ApplyDelta(+Mathf.Abs(amount), reason);
        }

        /// <summary>
        /// Decrease suspicion by amount (post-clamped). Fires change event.
        /// (Does NOT fire any "cooled down" event; add later if you want.)
        /// </summary>
        public void Decrease(float amount, string reason = "")
        {
            if (amount <= 0f) return;
            ApplyDelta(-Mathf.Abs(amount), reason);
        }

        /// <summary>Set suspicion directly (clamped), firing change and threshold events as appropriate.</summary>
        public void SetValue(float value, string reason = "")
        {
            float clamped = Mathf.Clamp(value, 0f, maxValue);
            float delta = clamped - currentValue;
            if (Mathf.Approximately(delta, 0f)) return;
            ApplyDelta(delta, reason);
        }

        /// <summary>Reset suspicion to starting value (fires change event if different).</summary>
        public void ResetSuspicion(string reason = "Reset")
        {
            SetValue(startingValue, reason);
        }

        /// <summary>Change the threshold at runtime; does not emit events by itself.</summary>
        public void SetThreshold(float newThreshold)
        {
            threshold = Mathf.Clamp(newThreshold, 0f, maxValue);
            // We intentionally do not emit or re-evaluate crossing here.
        }

        /// <summary>Adjust the maximum clamp; will also clamp current/threshold if needed.</summary>
        public void SetMaxValue(float newMax)
        {
            maxValue = Mathf.Max(1f, newMax);
            threshold = Mathf.Min(threshold, maxValue);
            SetValue(Mathf.Min(currentValue, maxValue), "MaxValueChanged");
        }

        // --- Unity lifecycle -------------------------------------------------

        private void Awake()
        {
            currentValue = Mathf.Clamp(startingValue, 0f, maxValue);
            isAtOrAboveThreshold = currentValue >= threshold;
        }

        // --- Internal --------------------------------------------------------

        private void ApplyDelta(float delta, string reason)
        {
            float previous = currentValue;
            float next = Mathf.Clamp(previous + delta, 0f, maxValue);

            // Early-out if no effective change after clamping
            if (Mathf.Approximately(previous, next)) return;

            currentValue = next;

            // 1) Broadcast change
            EventBus<SuspicionChangedEvent>.Raise(
                new SuspicionChangedEvent(previous, currentValue, currentValue - previous, reason));

            // 2) Check upward threshold crossing
            if (!isAtOrAboveThreshold && currentValue >= threshold)
            {
                isAtOrAboveThreshold = true;
                EventBus<SuspicionTooHighEvent>.Raise(
                    new SuspicionTooHighEvent(currentValue, threshold, string.IsNullOrEmpty(reason) ? "ThresholdCrossed" : reason));
            }
            else if (isAtOrAboveThreshold && currentValue < threshold)
            {
                // Track state for future upward crossings; no event requested by design.
                isAtOrAboveThreshold = false;
            }
        }
    }

    // --- Editor helper to show readonly fields without making them public ---
#if UNITY_EDITOR
    using UnityEditor;
    public class ReadOnlyInInspectorAttribute : PropertyAttribute { }
    [CustomPropertyDrawer(typeof(ReadOnlyInInspectorAttribute))]
    public class ReadOnlyInInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif
}
