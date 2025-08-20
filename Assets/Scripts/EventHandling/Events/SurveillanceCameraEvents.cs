
namespace EventHandling.Events
{
    public enum SecurityCamState { On, Off }

    /// <summary>Raised whenever the logical camera toggles state.</summary>
    public readonly struct SecurityCamStateChangedEvent : IEvent
    {
        public readonly SecurityCamState Previous;
        public readonly SecurityCamState Current;

        public SecurityCamStateChangedEvent(SecurityCamState previous, SecurityCamState current)
        {
            Previous = previous;
            Current = current;
        }
    }

    /// <summary>Raised when the logical camera turns ON.</summary>
    public readonly struct SecurityCamTurnedOnEvent : IEvent
    {
        public readonly float DurationPlanned; // seconds it will stay ON next, based on settings
        public SecurityCamTurnedOnEvent(float durationPlanned) => DurationPlanned = durationPlanned;
    }

    /// <summary>Raised when the logical camera turns OFF.</summary>
    public readonly struct SecurityCamTurnedOffEvent : IEvent
    {
        public readonly float DurationPlanned; // seconds it will stay OFF next, based on settings
        public SecurityCamTurnedOffEvent(float durationPlanned) => DurationPlanned = durationPlanned;
    }
}
