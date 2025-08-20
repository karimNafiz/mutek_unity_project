using System;
using DG.Tweening;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace Appearable
{
    /// <summary>
    /// Base class for appearable UI or game objects with tween-related settings.
    /// Implements the IAppearable interface, managing visibility and supporting delay-based appearance.
    /// </summary>
    public class AppearableBaseTween : MonoBehaviour, IAppearable
    {
        [SerializeField] protected float duration = 1f;
        [FormerlySerializedAs("inEase")] 
        [SerializeField] protected Ease showEase = Ease.OutBack;
        [FormerlySerializedAs("outEase")] 
        [SerializeField] protected Ease hideEase = Ease.InBack;

        /// <summary>
        /// Tracks whether the object is currently visible.
        /// This state switches instantly regardless whether the tween is playing or not. 
        /// </summary>
        [SerializeField] protected bool activeState = false;

        /// <summary>
        /// The currently active tween
        /// </summary>
        protected Tween activeTween;

        protected virtual void Start()
        {
            if (activeState)
            {
                ShowImmediate();
            }
            else
            {
                HideImmediate();
            }
        }

        /// <summary>
        /// Shows the object with an optional delay. Default implementation simply activates the GameObject.
        /// Can be overridden to add custom animation behavior.
        /// </summary>
        /// <param name="startDelay">Optional delay before showing the object.</param>
        public virtual void Show(float startDelay = 0f)
        {
            if (!activeState)
            {
                gameObject.SetActive(true);
                activeState = true;
            }
        }

        /// <summary>
        /// Hides the object immediately by deactivating the GameObject.
        /// Can be overridden for custom animation logic.
        /// </summary>
        public virtual void Hide()
        {
            if (activeState)
            {
                gameObject.SetActive(false);
                activeState = false;
            }
        }
        
        /// <summary>
        /// Returns the current active state of the AppearableBaseTween object.
        /// </summary>
        /// <returns></returns>
        public bool GetActiveState()
        {
            return activeState;
        }

        /// <summary>
        /// Immediately shows the object without delay or animation.
        /// Does not provide overriding.
        /// </summary>
        public virtual void ShowImmediate()
        {
            if (!activeState)
            {
                gameObject.SetActive(true);
                activeState = true;
            }
        }

        /// <summary>
        /// Immediately hides the object without animation.
        /// Does not provide overriding.
        /// </summary>
        public virtual void HideImmediate()
        {
            if (activeState)
            {
                gameObject.SetActive(false);
                activeState = false;
            }
        }

        /// <summary>
        /// Coroutine that waits for the configured duration. Can be used in derived animation logic.
        /// </summary>
        /// <return>WaitForSeconds yield instruction.</returns>
        public IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(duration);
        }
        
        
    }
}