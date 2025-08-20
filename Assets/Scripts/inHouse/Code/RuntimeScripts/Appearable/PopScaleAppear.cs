using UnityEngine;
using DG.Tweening;

namespace Appearable
{
    /// <summary>
    /// Overrides ScaleAppear to create a "pop in" effect,
    /// starting from a larger scale and shrinking to original.
    /// </summary>
    public class PopScaleAppear : ScaleAppear
    {
        [SerializeField] private float popScaleMultiplier = 1.5f;

        
        
        public override void Show(float startDelay = 0f)
        {
            if (!activeState)
            {
                activeState = true;
                gameObject.SetActive(true);
                activeTween?.Kill();

                // Start from larger scale
                transform.localScale = originalScale * popScaleMultiplier;

                activeTween = transform.DOScale(originalScale, duration)
                    .SetEase(showEase)
                    .SetDelay(startDelay)
                    .OnKill(() => transform.localScale = originalScale);
            }
        }

        public override void Hide()
        {
            
            base.Hide(); // same hide logic from parent
        }
    }
}
