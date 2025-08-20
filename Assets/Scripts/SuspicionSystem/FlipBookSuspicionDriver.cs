using System.Collections.Generic;
using UnityEngine;
using EventHandling;
using EventHandling.Events;
using Utility.Singleton;
using ScriptableObjects;                 // SO_FlipbookPageCollection

namespace SuspicionSystem
{
    /// <summary>
    /// Singleton that listens to flipbook + surveillance events and applies continuous
    /// suspicion deltas (per second) based on which book is currently held.
    /// </summary>
    public class FlipbookSuspicionDriver : SingletonMonoBehavior<FlipbookSuspicionDriver>
    {
        [Header("References")]
        [SerializeField] private SuspicionSystem suspicionSystem;

        [Header("Book Lists")]
        [Tooltip("Books that RAISE suspicion while held up.")]
        [SerializeField] private List<SO_FlipbookPageCollection> suspiciousBooks = new();
        [Tooltip("Books that LOWER suspicion while held up.")]
        [SerializeField] private List<SO_FlipbookPageCollection> permissibleBooks = new();

        [Header("Rates (per second)")]
        [Tooltip("Suspicion increase per second while holding any book in 'Suspicious Books'.")]
        [SerializeField, Min(0f)] private float suspiciousIncreasePerSecond = 4f;
        [Tooltip("Suspicion decrease per second while holding any book in 'Permissible Books'.")]
        [SerializeField, Min(0f)] private float permissibleDecreasePerSecond = 3f;

        [Header("Gating")]
        [Tooltip("If enabled, apply the per-second changes ONLY while a security camera is ON.")]
        [SerializeField] private bool onlyWhileCameraOn = true;

        // --- runtime state ---
        private SO_FlipbookPageCollection currentBook;
        private bool isBookUp;
        private bool isCameraOn;

        // event bindings
        private EventBinding<OnBookHeldUp> onBookUpBinding;
        private EventBinding<OnBookPutDown> onBookDownBinding;
        private EventBinding<OnBookChanged> onBookChangedBinding;
        private EventBinding<SecurityCamTurnedOnEvent> onCamOnBinding;
        private EventBinding<SecurityCamTurnedOffEvent> onCamOffBinding;

        protected override void Awake()
        {
            base.Awake();

            if (!suspicionSystem)
                suspicionSystem = SuspicionSystem.Instance;

            // set up bindings
            onBookUpBinding = new EventBinding<OnBookHeldUp>(e =>
            {
                currentBook = e.Book;
                isBookUp = true;
            });

            onBookDownBinding = new EventBinding<OnBookPutDown>(e =>
            {
                // keep last book reference but mark down
                isBookUp = false;
            });

            onBookChangedBinding = new EventBinding<OnBookChanged>(e =>
            {
                currentBook = e.NewBook;
                // up/down state is controlled by held-up / put-down events
            });

            onCamOnBinding = new EventBinding<SecurityCamTurnedOnEvent>(_ => isCameraOn = true);
            onCamOffBinding = new EventBinding<SecurityCamTurnedOffEvent>(_ => isCameraOn = false);
        }

        private void OnEnable()
        {
            EventBus<OnBookHeldUp>.Register(onBookUpBinding);
            EventBus<OnBookPutDown>.Register(onBookDownBinding);
            EventBus<OnBookChanged>.Register(onBookChangedBinding);

            EventBus<SecurityCamTurnedOnEvent>.Register(onCamOnBinding);
            EventBus<SecurityCamTurnedOffEvent>.Register(onCamOffBinding);
        }

        private void OnDisable()
        {
            EventBus<OnBookHeldUp>.Deregister(onBookUpBinding);
            EventBus<OnBookPutDown>.Deregister(onBookDownBinding);
            EventBus<OnBookChanged>.Deregister(onBookChangedBinding);

            EventBus<SecurityCamTurnedOnEvent>.Deregister(onCamOnBinding);
            EventBus<SecurityCamTurnedOffEvent>.Deregister(onCamOffBinding);
        }

        private void Update()
        {
            if (!suspicionSystem) return;
            if (!isBookUp) return;
            if (onlyWhileCameraOn && !isCameraOn) return;
            if (!currentBook) return;

            float dt = Time.deltaTime;

            // If the current book is classified as suspicious, increase suspicion.
            if (suspiciousBooks != null && suspiciousBooks.Contains(currentBook))
            {
                float amount = suspiciousIncreasePerSecond * dt;
                if (amount > 0f) suspicionSystem.Increase(amount, $"Book:{currentBook.name}");
                return; // don't double-count if the book appears in both lists
            }

            // If the current book is permissible, decrease suspicion.
            if (permissibleBooks != null && permissibleBooks.Contains(currentBook))
            {
                float amount = permissibleDecreasePerSecond * dt;
                if (amount > 0f) suspicionSystem.Decrease(amount, $"Book:{currentBook.name}");
            }
        }

        // ---------- Optional helpers for runtime configuration ---------------

        public void SetRates(float increasePerSec, float decreasePerSec)
        {
            suspiciousIncreasePerSecond = Mathf.Max(0f, increasePerSec);
            permissibleDecreasePerSecond = Mathf.Max(0f, decreasePerSec);
        }

        public void SetOnlyWhileCameraOn(bool value) => onlyWhileCameraOn = value;

        public void SetLists(IEnumerable<SO_FlipbookPageCollection> suspicious, IEnumerable<SO_FlipbookPageCollection> permissible)
        {
            suspiciousBooks = suspicious != null ? new List<SO_FlipbookPageCollection>(suspicious) : new List<SO_FlipbookPageCollection>();
            permissibleBooks = permissible != null ? new List<SO_FlipbookPageCollection>(permissible) : new List<SO_FlipbookPageCollection>();
        }
    }
}
