using System;
using UnityEngine;

namespace SelectableBase
{
    /// <summary>
    /// Abstract base class for handling events from a <see cref="SelectableObjBase"/>.
    /// Automatically manges the registration and deregistration events on start and destroy.
    /// Inherit from this class when needed to react to hover and selection events of a selectable object.
    /// </summary>
    public abstract class SelectableObjEventHandlerBase : MonoBehaviour
    {
        /// <summary>
        /// Reference to the associated selectable object.
        /// </summary>
        [SerializeField] protected SelectableObjBase selectableObj;

        private void Awake()
        {
            RegisterEvents();
        }
        
        /// <summary>
        /// Called when the MonoBehaviour is being destroyed.
        /// Unregisters event handlers.
        /// </summary>
        protected virtual void OnDestroy()
        {
            UnregisterEvents();
        }

        /// <summary>
        /// Registers this handler to the selectable object's events.
        /// </summary>
        private void RegisterEvents()
        {
            if (selectableObj != null)
            {
                selectableObj.OnObjHovered += SelectableObj_OnObjHovered;
                selectableObj.OnObjUnhovered += SelectableObj_OnObjUnhovered;
                selectableObj.OnObjSelected += SelectableObj_OnObjSelected;
                selectableObj.OnObjDeselected += SelectableObj_OnObjDeselected;
                selectableObj.OnObjShow += SelectableObj_OnObjShow;
                selectableObj.OnObjHide += SelectableObj_OnObjHide;
            }
        }

        /// <summary>
        /// Unregisters this handler from the selectable object's events.
        /// </summary>
        private void UnregisterEvents()
        {
            if (selectableObj != null)
            {
                selectableObj.OnObjHovered -= SelectableObj_OnObjHovered;
                selectableObj.OnObjUnhovered -= SelectableObj_OnObjUnhovered;
                selectableObj.OnObjSelected -= SelectableObj_OnObjSelected;
                selectableObj.OnObjDeselected -= SelectableObj_OnObjDeselected;
                selectableObj.OnObjShow -= SelectableObj_OnObjShow;
                selectableObj.OnObjHide -= SelectableObj_OnObjHide;
            }
        }

        /// <summary>
        /// Called when the selectable object is hovered over.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        protected abstract void SelectableObj_OnObjHovered(object sender, EventArgs e);

        /// <summary>
        /// Called when the selectable object is no longer hovered over.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        protected abstract void SelectableObj_OnObjUnhovered(object sender, EventArgs e);

        /// <summary>
        /// Called when the selectable object is selected.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        protected abstract void SelectableObj_OnObjSelected(object sender, EventArgs e);

        /// <summary>
        /// Called when the selectable object is deselected.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        protected abstract void SelectableObj_OnObjDeselected(object sender, EventArgs e);
        
        /// <summary>
        /// Called when the selectable object is shown.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        protected abstract void SelectableObj_OnObjShow(object sender, EventArgs e);
        
        /// <summary>
        /// Called when the selectable object is hidden.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        protected abstract void SelectableObj_OnObjHide(object sender, EventArgs e);
    }
}
