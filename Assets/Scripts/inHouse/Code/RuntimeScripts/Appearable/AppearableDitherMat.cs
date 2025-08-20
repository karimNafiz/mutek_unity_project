using System;
using System.Collections;
using UnityEngine;
using Utility.Lerp;

namespace Appearable
{
    /// <summary>
    /// Controls the visibility of a material using a custom dithering shader by modifying
    /// its _AlphaManualControl property over time with a provided Lerpable technique.
    /// Implements IAppearable for compatibility with standard show/hide interfaces.
    /// </summary>
    public class AppearableDitherMat : AppearableBaseLerp
    {
        [Header("Material Reference")]
        [SerializeField] private Renderer ditherMaterialRenderer;
        [SerializeField] private bool isVisible;
        private Material ditherMat;
        
        // private ILerpable lerpingTechnique;
        // private Coroutine lerpCoroutine;
        // private bool activeState;
        

        /// <summary>
        /// Initializes the material and sets its alpha based on the initial serialized visibility state.
        /// </summary>
        void Awake()
        {
            ditherMat = ditherMaterialRenderer.material;
            float startAlphaFactor = isVisible ? 1 : 0;
            activeState = isVisible;
            ApplyLerpFactor(startAlphaFactor);
        }


        /// <summary>
        /// Assigns the interpolation strategy used for fade-in/out.
        /// </summary>
        /// <param name="lerpingTechnique">An object implementing the Lerpable interface.</param>
        // public void SetLerpingTechnique(ILerpable lerpingTechnique)
        // {
        //     this.lerpingTechnique = lerpingTechnique;
        // }

        /// <summary>
        /// Begins the coroutine to gradually show the object by increasing its alpha value.
        /// </summary>
        /// <param name="delay">Currently unused. Included for IAppearable compatibility.</param>
        // public void Show(float delay = 0)
        // {
        //     if (!activeState)
        //     {
        //         StopLerping(0);
        //         activeState = true;
        //         lerpCoroutine = StartCoroutine(StartLerping(activeState));
        //     }
        // }

        /// <summary>
        /// Begins the coroutine to gradually hide the object by decreasing its alpha value.
        /// </summary>
        // public void Hide()
        // {
        //     if (activeState)
        //     {
        //         StopLerping(1);
        //         activeState = false;
        //         lerpCoroutine = StartCoroutine(StartLerping(activeState));
        //     }
        // }

        // public bool GetActiveState()
        // {
        //     return activeState;
        // }

        /// <summary>
        /// Immediately stops any ongoing fade and sets the material to a specific alpha value.
        /// </summary>
        /// <param name="endLerpFactor">The alpha transparency to apply immediately.</param>
        // public void StopLerping(float endLerpFactor)
        // {
        //     if (lerpCoroutine != null)
        //     {
        //         StopCoroutine(lerpCoroutine);
        //         lerpCoroutine = null;
        //     }
        //     ApplyToMaterial(endLerpFactor);
        // }

        /// <summary>
        /// Coroutine that interpolates the material alpha over time based on the assigned Lerpable.
        /// </summary>
        /// <param name="flag">True for fade-in, false for fade-out.</param>
        // private IEnumerator StartLerping(bool flag)
        // {
        //     float normalizer = !flag ? 1 : 0;
        //     (bool isEndLerp, float alphaFactor) lerpResult;
        //     while (true)
        //     {
        //         lerpResult = this.lerpingTechnique.TickLerp();
        //         ApplyToMaterial(Math.Abs(normalizer - lerpResult.alphaFactor));
        //         if (lerpResult.isEndLerp) break;
        //
        //         yield return null;
        //     }

        // It doesn't look like we need this.
        // if (lerpCoroutine != null)
        // {
        //     StopCoroutine(lerpCoroutine);
        //     lerpCoroutine = null;
        // }
        // }


        /// <summary>
        /// Immediately stops any ongoing fade and sets the material to a fully opaque state.
        /// Overriten for the purpose because we don't want to change the active state when called this. 
        /// This function is just called to change the visual.
        /// </summary>
        public override void ShowImmediate()
        {
            StopLerping(1);
        }

        /// <summary>
        /// Immediately stops any ongoing fade and sets the material to a fully transparent state.
        /// Overriten for the purpose because we don't want to change the active state when called this. 
        /// This function is just called to change the visual.
        /// </summary>
        public override void HideImmediate()
        {
            StopLerping(0);
        }

        /// <summary>
        /// Applies the given alpha value to the material's _AlphaManualControl property.
        /// </summary>
        /// <param name="factor">The alpha transparency factor (0 to 1).</param>
        protected override void ApplyLerpFactor(float factor)
        {
            ditherMat.SetFloat("_AlphaManualControl", factor);
        }
    }
}