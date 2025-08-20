using UnityEngine;
using Utility.Lerp;
using System.Collections;

namespace Appearable
{
    /// <summary>
    /// Base class for appearable game objects that uses a ILerpable implementation for fade in and out.
    /// Implements the IAppearable interface, managing visibility and supporting delay-based appearance.
    /// </summary>
    public class AppearableBaseLerp: MonoBehaviour, IAppearable
    {
        [SerializeField] protected float duration = 1f;
        
        // Interpolation helpers
        // the current lerp technique.
        protected ILerpable currLerpTechnique;
        // the lerp technique is set externally by the user
        protected ILerpable externLerpTechnique;
        
        protected Coroutine lerpCoroutine;
        protected bool activeState = false;
        
        /// <inheritdoc />
        public virtual void Show(float delay = 0f)
        {
            if (activeState) return;
            activeState = true;
            StopLerping(0f);
            lerpCoroutine = StartCoroutine(LerpRoutine(true, delay));
        }

        public virtual void ShowImmediate()
        {
            if (activeState) return;
            activeState = true;
            StopLerping(1f);
        }

        public virtual void Hide()
        {
            if (!activeState) return;
            //Debug.Log("I am hiding myself");
            activeState = false;
            StopLerping(1f);
            lerpCoroutine = StartCoroutine(LerpRoutine(false));
        }

        public virtual void HideImmediate()
        {
            if (!activeState) return;
            activeState = false;
            StopLerping(0f);
        }
        

        public bool GetActiveState()
        {
            return activeState;
        }

        protected IEnumerator LerpRoutine(bool fadeIn, float delay = 0f)
        {
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }
            
            // if the lerpTechnique is not set externally, use the fallback technique.
            if (externLerpTechnique == null)
            {
                currLerpTechnique = FallbackLerpTechnique();
            }
            else
            {
                currLerpTechnique = externLerpTechnique;
                externLerpTechnique = null;
            }
            
            float start = fadeIn ? 0f : 1f;
            float end = fadeIn ? 1f : 0f;
            
            while (true)
            {
                (bool isEndLerp, float t) = currLerpTechnique.TickLerp();
                // adjust the lerp factor based on the fadein flag
                float factor = Mathf.Lerp(start, end, t);
                ApplyLerpFactor(factor);
                
                if (isEndLerp)
                {
                    break;
                }
                yield return null;
            }

            // reset the lerpTechnique to null.
            // ResetLerpTechnique();
            
            if (fadeIn)
            {
                OnCompleteShow();
            }
            else
            {
                OnCompleteHide();
            }
        }
        
        
        /// <summary>
        /// Allows subclasses to perform any cleanup or state sync after fade-in completes.
        /// </summary>
        protected virtual void OnCompleteShow()
        {
            
        }

        /// <summary>
        /// Allows subclasses to perform any cleanup or state sync after fade-off completes.
        /// </summary>
        protected virtual void OnCompleteHide()
        {
            
        }
        
        /// <summary>
        /// Subclasses apply the interpolated factor (0 to 1) (e.g. alpha).
        /// </summary>
        protected virtual void ApplyLerpFactor(float factor)
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// Stops the lerp coroutine and force apply the given end lerp factor.
        /// </summary>
        /// <param name="endLerpFactor"></param>
        protected void StopLerping(float endLerpFactor)
        {
            if (lerpCoroutine != null)
            {
                StopCoroutine(lerpCoroutine);
                lerpCoroutine = null;
            }
            // ResetLerpTechnique();
            ApplyLerpFactor(endLerpFactor);
        }
        
        
        public void SetLerpTechnique(ILerpable lerpTechnique)
        {
            this.externLerpTechnique = lerpTechnique;
        }
        
        /// <summary>
        /// Reset the curr lerp technique to null.
        /// </summary>
        // protected void ResetLerpTechnique()
        // {
        //     currLerpTechnique = null;
        // }

        protected ILerpable FallbackLerpTechnique()
        {
            return new TimeLerp(duration);
        }
        
    }
}