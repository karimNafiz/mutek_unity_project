using System.Collections;
using UnityEngine;
using EventHandling;
using EventHandling.Events;
using Utility.Singleton;
namespace Gameplay.Surveillance
{
    /// <summary>
    /// Simple logical security camera that alternates between On and Off.
    /// No scene camera required — this only drives timing and events.
    /// </summary>
    public class SurveillanceCameraSystem : SingletonMonoBehavior<SurveillanceCameraSystem>
    {
        [Header("Cycle Durations (seconds)")]
        [SerializeField, Min(0f)] private float onDuration = 10f;
        [SerializeField, Min(0f)] private float offDuration = 5f;

        [Header("Behavior")]
        [SerializeField] private SecurityCamState initialState = SecurityCamState.On;
        [SerializeField] private bool playOnAwake = true;         // auto-start cycle
        [SerializeField] private float initialDelay = 0f;         // optional delay before starting

        [Header("Debug (read-only)")]
        [SerializeField] private SecurityCamState currentState;

        Coroutine cycleRoutine;

        public SecurityCamState CurrentState => currentState;
        public float OnDuration
        {
            get => onDuration;
            set => onDuration = Mathf.Max(0f, value);
        }
        public float OffDuration
        {
            get => offDuration;
            set => offDuration = Mathf.Max(0f, value);
        }

        protected override void  Awake()
        {
            base.Awake();
            currentState = initialState;
        }

        void OnEnable()
        {
            if (playOnAwake)
                StartCycle();
            else
                // still broadcast initial state so listeners know where we begin
                RaiseStateBroadcast(currentState);
        }

        void OnDisable()
        {
            StopCycle();
        }

        // -------- Public control API ----------------------------------------

        /// <summary>Begin (or restart) the on/off cycle from the current state.</summary>
        public void StartCycle()
        {
            StopCycle();
            cycleRoutine = StartCoroutine(Cycle());
        }

        /// <summary>Stop cycling (keeps current state, no more toggles).</summary>
        public void StopCycle()
        {
            if (cycleRoutine != null)
            {
                StopCoroutine(cycleRoutine);
                cycleRoutine = null;
            }
        }

        /// <summary>Force the state immediately and optionally restart timing from here.</summary>
        public void ForceState(SecurityCamState state, bool restartTimer = true)
        {
            if (currentState != state)
                SetState(state);
            if (restartTimer)
                StartCycle();
        }

        /// <summary>Convenience: set both durations (seconds) and optionally restart timer.</summary>
        public void SetDurations(float onSeconds, float offSeconds, bool restartTimer = true)
        {
            onDuration = Mathf.Max(0f, onSeconds);
            offDuration = Mathf.Max(0f, offSeconds);
            if (restartTimer && cycleRoutine != null)
            {
                StartCycle();
            }
        }

        // -------- Core loop --------------------------------------------------

        IEnumerator Cycle()
        {
            // optional startup delay
            if (initialDelay > 0f)
                yield return new WaitForSeconds(initialDelay);

            // broadcast the starting state on (re)start
            RaiseStateBroadcast(currentState);

            while (true)
            {
                float wait = GetPlannedDurationForState(currentState);

                // if duration is 0, toggle immediately next frame
                if (wait > 0f)
                    yield return new WaitForSeconds(wait);
                else
                    yield return null;

                ToggleState();
            }
        }

        // -------- Internals --------------------------------------------------

        float GetPlannedDurationForState(SecurityCamState state)
        {
            return state == SecurityCamState.On ? onDuration : offDuration;
        }

        void ToggleState()
        {
            var next = currentState == SecurityCamState.On ? SecurityCamState.Off : SecurityCamState.On;
            SetState(next);
        }

        void SetState(SecurityCamState next)
        {
            var previous = currentState;
            currentState = next;

            // state-changed event
            EventBus<SecurityCamStateChangedEvent>.Raise(new SecurityCamStateChangedEvent(previous, currentState));

            Debug.Log($"changing surveillance camera state prev state ->{previous} current state -> {currentState}");

            // specialized events with the planned upcoming duration for this state
            if (currentState == SecurityCamState.On)
            {
                EventBus<SecurityCamTurnedOnEvent>.Raise(new SecurityCamTurnedOnEvent(onDuration));
            }
            else
            {
                EventBus<SecurityCamTurnedOffEvent>.Raise(new SecurityCamTurnedOffEvent(offDuration));
            }
        }

        void RaiseStateBroadcast(SecurityCamState state)
        {
            // When we come online or restart, tell listeners what state we're in now
            // by emitting the specific event plus a no-op "changed" from same->same.
            if (state == SecurityCamState.On)
            {
                EventBus<SecurityCamTurnedOnEvent>.Raise(new SecurityCamTurnedOnEvent(onDuration));
            }
            else
            {
                EventBus<SecurityCamTurnedOffEvent>.Raise(new SecurityCamTurnedOffEvent(offDuration));
            }

            EventBus<SecurityCamStateChangedEvent>.Raise(new SecurityCamStateChangedEvent(state, state));
            //Debug.Log($"the surveillance camera state changed prev state -> {}")
        }
    }
}
