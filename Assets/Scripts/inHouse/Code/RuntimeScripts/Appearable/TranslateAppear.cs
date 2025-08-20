using UnityEngine;
using DG.Tweening;

namespace Appearable
{
    /// <summary>
    /// Handles appear/disappear behavior by translating the object between defined positions.
    /// Inherits animation timing and easing from AppearableBaseTween.
    /// </summary>
    public class TranslateAppear : AppearableBaseTween
    {
        [SerializeField] private Transform inPos;
        [SerializeField] private Transform outPos;

        /// <summary>
        /// Initializes the object position to the "out" state on Awake.
        /// </summary>
        private void Awake()
        {
            transform.position = outPos.position;
        }

        /// <summary>
        /// Shows the object by animating its local position from outPos to inPos.
        /// </summary>
        /// <param name="startDelay">Delay before the translation animation starts.</param>
        public override void Show(float startDelay = 0f)
        {
            if (!activeState)
            {
                activeState = true;
                gameObject.SetActive(true);
                activeTween?.Kill();
                // restore to outPos
                transform.localPosition = outPos.localPosition;
                
                activeTween = transform.DOLocalMove(inPos.localPosition, duration)
                    .SetEase(showEase)
                    .SetDelay(startDelay)
                    .OnKill(() => transform.localPosition = inPos.localPosition);
            }
        }

        /// <summary>
        /// Hides the object by animating its local position from inPos to outPos.
        /// </summary>
        public override void Hide()
        {
            if (activeState)
            {
                activeState = false;
                activeTween?.Kill();
                // restore to inPos
                transform.localPosition = inPos.localPosition;
                
                activeTween = transform.DOLocalMove(outPos.localPosition, duration)
                    .SetEase(hideEase)
                    .OnKill(() => transform.localPosition = outPos.localPosition);
            }
        }

        
        public override void ShowImmediate()
        {
            base.ShowImmediate();
            
            // fast-forward to end translate result
            transform.localPosition = inPos.localPosition;
        }

        public override void HideImmediate()
        {
            base.HideImmediate();
            
            // fast-forward to end translate result
            transform.localPosition = outPos.localPosition;
        }
    }
}