using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility.Singleton;
namespace SelectableBase
{
    /// <summary>
    /// Event arguments for object selection events, containing a reference to the object's transform.
    /// </summary>
    public class ObjSelectionArgs : EventArgs
    {
        public Transform _transform;
        public Vector3 _hitPos;
    }
    
    /// <summary>
    /// The base class for various object selectors handling object hover and selection logic.
    /// </summary>
    /// <typeparam name="T">Type parameter constrained to MonoBehaviour, typically the derived class.</typeparam>
    public abstract class ObjSelectorBase<T> : Utility.SingletonMonoBehaviour<T> where T : MonoBehaviour
    {
        /// <summary>
        /// Event triggered when an object is hovered over.
        /// </summary>
        public event EventHandler<ObjSelectionArgs> OnHoverObj;

        /// <summary>
        /// Event triggered when an object is selected.
        /// </summary>
        public event EventHandler<ObjSelectionArgs> OnSelectObj;
        //public event EventHandler<ObjSelectionArgs> OnDeselectObj;

        [Space]
        [SerializeField] protected LayerMask interactableLayerMask;
        [SerializeField] protected LayerMask ignoreLayerMask;
        [SerializeField] protected Transform hoveredObj;
        [SerializeField] protected Transform selectedObj;
        [SerializeField] protected float raycastDistance = 100f;
        
        protected int layerMaskToCheck;

        /// <summary>
        /// Initializes the object selector, setting the raycast layer mask.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            // changes made by karim 
            layerMaskToCheck = ~ignoreLayerMask.value;

            //Debug.LogWarning(gameObject.name + ": " + layerMaskToCheck);
        }

        /// <summary>
        /// Safely invokes the OnHoverObj event with the given sender and arguments.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Event arguments containing the hovered object.</param>
        protected void InvokeOnHoverObj(object sender, ObjSelectionArgs args)
        {
            OnHoverObj?.Invoke(sender, args);
        }

        /// <summary>
        /// Safely invokes the OnSelectObj event with the given sender and arguments.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">Event arguments containing the selected object.</param>
        protected void InvokeOnSelectObj(object sender, ObjSelectionArgs args)
        {
            OnSelectObj?.Invoke(sender, args);
        }

        /// <summary>
        /// Detects if pointer is over any UI element.
        /// </summary>
        /// <returns></returns>
        protected bool IsPointerOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }


        /// <summary>
        /// Performs raycasting to detect and update the hovered object.
        /// </summary>
        /// TODO: Look at the implementation in WalkableGroundSelector, see if this can be combined.
        /// Maybe not, this is a bit cheaper than the one in WalkableGroundSelector because
        /// this does not check for threshold distance
        protected virtual void ProcessObjectHover()
        {
            // Early return if anything is already selected
            if (selectedObj != null)
            {
                return;
            }
            
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Transform hitObj = null;

            //Physics.Raycast(ray, out hit, raycastDistance, layerMaskToCheck);
            //Debug.Log(layerMaskToCheck);
            //Debug.Log($"raycast hit {hit.transform.name}");

            if (Physics.Raycast(ray, out hit, raycastDistance, layerMaskToCheck)   // hit something
                 && (interactableLayerMask.value & (1 << hit.transform.gameObject.layer)) != 0
                 && !IsPointerOverUI()) // hit obj in the right layer
            {

                hitObj = hit.transform;

                if (hitObj != hoveredObj)
                {
                    
                    SetHoveredObject(hitObj);
                }
            }
            else
            {
                SetHoveredObject(null);
            }
        }


        /// <summary>
        /// Placeholder for object selection logic. Should be overridden in derived classes.
        /// </summary>
        protected virtual void ProcessObjectSelection()
        {
            Debug.LogError("No implementation ObjSelectorBase.ProcessObjectSelection()");
        }

        /// <summary>
        /// Placeholder for object deselection logic. Should be overridden in derived classes.
        /// </summary>
        protected virtual void ProcessObjectDeselection()
        {
            Debug.LogError("No implementation ObjSelectorBase.ProcessObjectDeselection()");
        }



        /// <summary>
        /// Sets the currently hovered object and triggers the corresponding event.
        /// This is used when we don't care where the hit point is, just the object itself.
        /// </summary>
        /// <param name="objToHover">The object to mark as hovered.</param>
        protected virtual void SetHoveredObject(Transform objToHover)
        {
            hoveredObj = objToHover;
            InvokeOnHoverObj(this, new ObjSelectionArgs() { _transform = objToHover });
        }

        
        /// <summary>
        /// The overload method for SetHoveredObject that takes in both the objToHover and hitPos.
        /// </summary>
        /// <param name="objToHover">The object to mark as hovered</param>
        /// <param name="hitPos">The position of the raycast</param>
        protected virtual void SetHoveredObject(Transform objToHover, Vector3 hitPos)
        {
            hoveredObj = objToHover;
            InvokeOnHoverObj(this, new ObjSelectionArgs() 
            {
                _transform = objToHover, 
                _hitPos = hitPos
                
            });
        }


        /// <summary>
        /// Sets the currently selected object and triggers the corresponding event.
        /// </summary>
        /// <param name="objToSelect">The object to mark as selected.</param>
        protected virtual void SetSelectedObject(Transform objToSelect)
        {
            hoveredObj = null;
            selectedObj = objToSelect;
            InvokeOnSelectObj(this, new ObjSelectionArgs() { _transform = objToSelect });
        }

        
        /// <summary>
        /// The overload method for SetSelectedObject that takes in both the objToSelect and hitPos.
        /// </summary>
        /// <param name="objToSelect">The object to mark as selected</param>
        /// <param name="hitPos">The position of the raycast</param>
        protected virtual void SetSelectedObject(Transform objToSelect, Vector3 hitPos)
        {
            hoveredObj = null;
            selectedObj = objToSelect;
            InvokeOnSelectObj(this, new ObjSelectionArgs() 
            {
                _transform = objToSelect, 
                _hitPos = hitPos
            });
        }

    }
}

