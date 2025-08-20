using UnityEngine;
using DG.Tweening;
using System;

namespace Appearable
{
    /// <summary>
    /// Handles drop appear behavior by moving the object from outPos (high) to inPos (ground)
    /// Inherits timing and easing from AppearableBaseTween.
    /// DropAppear is meant to be Show() multiple times without Hide(), and Hide() should not reset its position.
    /// As a result, we only have Show() and ShowImmediate() override
    /// while keeping the Hide() and HideImmediate() as the default implementation 
    /// </summary>
    /// 
    // [DefaultExecutionOrder(-50)]
    public class DropAppear : AppearableBaseTween
    {
        [SerializeField] private Transform inPos;
        [SerializeField] private Transform outPos;
        
        public event EventHandler<float> OnDropStart;
        public event EventHandler OnDropComplete;   


        public bool ActiveState 
        {
            get { return activeState; }
            set { activeState = value; }
        
        }
 
        /// <summary>
        /// Shows the object by dropping it from outPos to inPos with an Ease defined by inEase.
        /// </summary>
        /// <param name="startDelay">Optional delay before the drop.</param>
        public override void Show(float startDelay = 0f)
        {
            if (!activeState)
            {
                activeState = true;
                gameObject.SetActive(true);
                activeTween?.Kill();
                
                transform.localPosition = outPos.localPosition;


                activeTween = transform.DOLocalMove(inPos.localPosition, duration)
                    .SetEase(showEase)
                    .SetDelay(startDelay)
                    .OnStart(() =>
                    {
                        OnDropStart?.Invoke(this, duration);
                    })
                    .OnComplete(() =>
                    {
                        OnDropComplete?.Invoke(this, EventArgs.Empty);
                    })
                    .OnKill(() => transform.localPosition = inPos.localPosition);
            }
        }

        public override void ShowImmediate()
        {
            base.ShowImmediate();
            
            // fast-forward to end drop position
            transform.localPosition = inPos.localPosition;
            
            // Invoking these 2 events for consistency
            OnDropStart?.Invoke(this, 0.0f);
            OnDropComplete?.Invoke(this, EventArgs.Empty);
        }

        public override void Hide()
        {
            // now, ok to have the same implementation as HideImmediate.
            HideImmediate();
        }

        public override void HideImmediate()
        {
            // kill the tween first.
            if (activeState)
            {
                activeTween?.Kill();
            }

            base.HideImmediate();
        }

        /// <summary>
        /// special case temp handling:
        /// set the activeState immediately to false when start showing, to ensure 
        /// the tween can be called multiple times
        /// </summary>
        public void ResetActiveState()
        {
            activeState = false;
        }
    }
}
