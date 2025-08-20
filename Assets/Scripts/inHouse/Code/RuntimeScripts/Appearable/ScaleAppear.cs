using UnityEngine;
using DG.Tweening;

namespace Appearable
{
    /// <summary>
    /// Handles appear/disappear behavior by scaling the object in and out.
    /// Uses DOTween to animate scale transitions with easing.
    /// Inherits timing and easing configurations from AppearableBaseTween.
    /// </summary>
    public class ScaleAppear : AppearableBaseTween
    {
        protected Vector3 originalScale;

        /// <summary>
        /// Initializes the original scale and hides the objec on Awake.
        /// </summary>
        private void Awake()
        {
            originalScale = transform.localScale;
            // transform.localScale = Vector3.zero;
        }

        /// <summary>
        /// Shows the object by scaling it from zero to its original size with optional delay.
        /// </summary>
        /// <param name="startDelay">Optional delay before the scaling begins.</param>
        public override void Show(float startDelay = 0f)
        {
            if (!activeState)
            {
                activeState = true;
                gameObject.SetActive(true);
                activeTween?.Kill();
                // restore to min scale
                transform.localScale = Vector3.zero;

                // NOTE: Using Sequence with AppendInterval defers creating the child DOScale tween
                // until after the delay. If you call Hide() immediately, DOKill() won’t find the
                // (not-yet-created) tween, so it still runs once the interval elapses. Use a direct
                // tween with SetDelay or store/kill the Sequence reference to avoid this “ghost” tween.
                
                activeTween = transform.DOScale(originalScale, duration)
                    .SetEase(showEase)
                    .SetDelay(startDelay)
                    .OnKill(() => transform.localScale = originalScale);
            }
        }

        /// <summary>
        /// Hides the object by scaling it down to zero and deactivating it on completion.
        /// </summary>
        public override void Hide()
        {
            if (activeState)
            {
                activeState = false;
                activeTween?.Kill();
                // restore to original scale
                transform.localScale = originalScale;
                // originally it was duration * 0.5f, for the do tween time
                // im manually setting it to a large value for testing purposes
                activeTween = transform.DOScale(Vector3.zero, duration * 0.5f)
                    .SetEase(hideEase)
                    .OnKill(() => transform.localScale = Vector3.zero);
            }
        }

        public override void ShowImmediate()
        {
            base.ShowImmediate();
            // fast-forward to end scale result
            transform.localScale = originalScale;
        }

        public override void HideImmediate()
        {
            base.HideImmediate();
            // fast-forward to end scale result
            transform.localScale = Vector3.zero;
        }

    }
}