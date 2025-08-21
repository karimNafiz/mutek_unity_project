using UnityEngine;
using System;

namespace SelectableBase
{
    public class OutlineVisual : SelectableObjEventHandlerBase
    {
        [SerializeField] private string outlineLayerName = "Outline";
        private int originalLayer;
        private int outlineLayer;

        private void Start()
        {
            originalLayer = gameObject.layer;
            outlineLayer = LayerMask.NameToLayer(outlineLayerName);
        }

        /// <summary>
        /// Called when the selectable object is hovered over.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        protected override void SelectableObj_OnObjHovered(object sender, EventArgs e)
        {
            gameObject.layer = outlineLayer;
        }

        /// <summary>
        /// Called when the selectable object is no longer hovered over.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        protected override void SelectableObj_OnObjUnhovered(object sender, EventArgs e)
        {
            gameObject.layer = originalLayer;
        }

        /// <summary>
        /// Called when the selectable object is selected.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        protected override void SelectableObj_OnObjSelected(object sender, EventArgs e)
        {
            gameObject.layer = originalLayer;
        }

        /// <summary>
        /// Called when the selectable object is deselected.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        protected override void SelectableObj_OnObjDeselected(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Called when the selectable object is shown.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        protected override void SelectableObj_OnObjShow(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Called when the selectable object is hidden.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments.</param>
        protected override void SelectableObj_OnObjHide(object sender, EventArgs e)
        {
            
        }
    }

}
