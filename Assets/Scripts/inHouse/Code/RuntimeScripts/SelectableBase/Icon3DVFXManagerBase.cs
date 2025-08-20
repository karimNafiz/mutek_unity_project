using System;
using Appearable;
using UnityEngine;

namespace SelectableBase
{
    /// <summary>
    /// Abstract base class for managing 3D icon VFX related to a selectable object.
    /// Initializes icon-related visuals on start.
    /// </summary>
    public abstract class Icon3DVFXManagerBase : SelectableObjEventHandlerBase
    {
        [SerializeField] private AppearableBaseTween iconTween;

        /// <summary>
        /// Unity lifecycle method called on start.
        /// Calls base start logic to register selectable object events and initializes the icon.
        /// </summary>
        protected virtual void Start()
        {
            InitIcon();
        }

        /// <summary>
        /// Initializes the icon visuals. Must be implemented by derived classes.
        /// </summary>
        protected abstract void InitIcon();

        /// <summary>
        /// Show the icon visual using a tween (if available) upon object show event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event arguments</param>
        protected override void SelectableObj_OnObjShow(object sender, EventArgs e)
        {
            // Debug.Log("///////Show Icon///////");
            if (iconTween != null)
            {
                iconTween.Show();
            }
            else
            {
                // turn on this visual manager game object
                gameObject.SetActive(true); 
            }
        }

        /// <summary>
        /// Hide the icon visual using a tween (if available) upon object hide event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event arguments</param>
        protected override void SelectableObj_OnObjHide(object sender, EventArgs e)
        {
            // Debug.Log("///////Hide Icon///////");
            if (iconTween != null)
            {
                iconTween.Hide();
            }
            else
            {
                // turn off this visual manager game object
                gameObject.SetActive(false);
            }
        }
    }
}

