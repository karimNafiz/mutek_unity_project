using System;
using Appearable;
using UnityEngine;

namespace SelectableBase
{
    /// <summary>
    /// The base class of all selectable 3D objects in the scene.
    /// Handles hover and selection state, and raises related events.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public abstract class SelectableObjBase : MonoBehaviour, IAppearable
    {
        // Ray adding some spaghetti static event here
        public static event EventHandler OnAnyObjSelected;
        public static event EventHandler OnAnyObjDeselected;
        
        private bool activeState = true;
        
        /// <summary>
        /// Event triggered when the object is hovered over.
        /// </summary>
        public event EventHandler OnObjHovered;

        /// <summary>
        /// Event triggered when the object is no longer hovered over.
        /// </summary>
        public event EventHandler OnObjUnhovered;

        /// <summary>
        /// Event triggered when the object is selected.
        /// </summary>
        public event EventHandler OnObjSelected;

        /// <summary>
        /// Event triggered when the object is deselected.
        /// </summary>
        public event EventHandler OnObjDeselected;

        /// <summary>
        /// Event triggered when the object is shown.
        /// </summary>
        public event EventHandler OnObjShow;
        
        /// <summary>
        /// Event triggered when the object is hidden.
        /// </summary>
        public event EventHandler OnObjHide;
        
        /// <summary>
        /// Whether the object is currently hovered.
        /// </summary>
        protected bool isHovered = false;
        public bool IsHovered { get => isHovered; }

        /// <summary>
        /// Whether the object is currently selected.
        /// </summary>
        protected bool isSelected = false;
        public bool IsSelected { get => isSelected; }

        private Collider collider;

        protected virtual void Awake()
        {
            collider = GetComponent<Collider>();
        }

        /// <summary>
        /// Safely invokes the OnObjHovered event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Event arguments.</param>
        protected virtual void InvokeOnObjHovered(object sender, EventArgs args)
        {
            OnObjHovered?.Invoke(sender, args);
        }

        /// <summary>
        /// Safely invokes the OnObjUnhovered event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Event arguments.</param>
        protected virtual void InvokeOnObjUnhovered(object sender, EventArgs args)
        {
            OnObjUnhovered?.Invoke(sender, args);
        }

        /// <summary>
        /// Safely invokes the OnObjSelected event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Event arguments.</param>
        protected virtual void InvokeOnObjSelected(object sender, EventArgs args)
        {
            OnObjSelected?.Invoke(sender, args);
            OnAnyObjSelected?.Invoke(sender, args);
        }

        /// <summary>
        /// Safely invokes the OnObjDeselected event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Event arguments.</param>
        protected virtual void InvokeOnObjDeselected(object sender, EventArgs args)
        {
            OnObjDeselected?.Invoke(sender, args);
            OnAnyObjDeselected?.Invoke(sender, args);
        }

        /// <summary>
        /// Safely invokes the OnObjShow event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Event arguments.</param>
        protected virtual void InvokeOnObjShow(object sender, EventArgs args)
        {
            OnObjShow?.Invoke(sender, args);
        }

        /// <summary>
        /// Safely invokes the OnObjHide event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Event arguments.</param>
        protected virtual void InvokeOnObjHide(object sender, EventArgs args)
        {
            OnObjHide?.Invoke(sender, args);
        }

        public void Show(float startDelay = 0)
        {
            if (!activeState)
            {
                activeState = true;
                collider.enabled = true;
                InvokeOnObjShow(this, EventArgs.Empty);
            }
        }

        public void Hide()
        {
            if (activeState)
            {
                activeState = false;
                collider.enabled = false;
                InvokeOnObjHide(this, EventArgs.Empty);
            }
        }
        
        public bool GetActiveState()
        {
            return activeState;
        }

        /// <summary>
        /// Handles hover detection for this object, updating the hovered state and raising events.
        /// This callback is registered to the corresponding ObjectSelector in the children implementation of this class
        /// </summary>
        /// <param name="sender">The sender of the hover event.</param>
        /// <param name="e">The selection arguments containing the transform to compare.</param>
        protected virtual void ObjSelector_OnHoverObj(object sender, ObjSelectionArgs e)
        {
            if (e._transform == transform)
            {
                isHovered = true;
                InvokeOnObjHovered(this, EventArgs.Empty);
            }
            else
            {
                if (isHovered)
                {
                    isHovered = false;
                    InvokeOnObjUnhovered(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Placeholder for selection logic. Should be overridden in derived classes.
        /// This callback is registered to the corresponding ObjectSelector in the children implementation of this class
        /// </summary>
        /// <param name="sender">The sender of the select event.</param>
        /// <param name="e">The selection arguments containing the transform to compare.</param>
        protected virtual void ObjSelector_OnSelectObj(object sender, ObjSelectionArgs e)
        {
            Debug.LogError("No implementation of SelectableObjBase.ObjSelector_OnSelectObj_Handler");

        }
    }
}

