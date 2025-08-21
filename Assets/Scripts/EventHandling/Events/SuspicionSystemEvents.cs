

namespace EventHandling.Events
{
    /// <summary>
    /// Raised whenever the suspicion value changes (after clamping).
    /// </summary>
    public readonly struct SuspicionChangedEvent : IEvent
    {
        public readonly float Previous;
        public readonly float Current;
        public readonly float Delta;
        public readonly float MaxValue;
        public readonly string Reason;

        public SuspicionChangedEvent(float previous, float current, float delta, float maxValue, string reason)
        {
            Previous = previous;
            Current = current;
            Delta = delta;
            Reason = reason;
            MaxValue = maxValue;
        }
    }

    /// <summary>
    /// Raised once when suspicion crosses from below the threshold to >= threshold.
    /// </summary>
    public readonly struct SuspicionTooHighEvent : IEvent
    {
        public readonly float Value;
        public readonly float Threshold;
        public readonly string Reason;

        public SuspicionTooHighEvent(float value, float threshold, string reason)
        {
            Value = value;
            Threshold = threshold;
            Reason = reason;
        }
    }
}

