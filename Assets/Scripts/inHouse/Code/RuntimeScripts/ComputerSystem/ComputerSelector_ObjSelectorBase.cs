
using GameManager;
using SelectableBase;
using UnityEngine;

public class ComputerSelector_ObjSelectorBase : ObjSelectorBase<ComputerSelector_ObjSelectorBase>
{
/*
 
    need to handle deselection of the computer
    it should be by using Escape key 
 
 
 */







    private void Update()
    {
        if (selectedObj == null)
        {
            ProcessObjectHover();
            ProcessObjectSelection();
        }
        else
        {
            ProcessObjectDeselection();
        }
    }

    /// <summary>
    /// Processes object hover logic each frame when nothing is selected.
    /// </summary>
    protected override void ProcessObjectHover()
    {
        base.ProcessObjectHover();
    }

    /// <summary>
    /// Processes object selection based on input, selecting hovered Waypoints if applicable.
    /// </summary>
    protected override void ProcessObjectSelection()
    {
        if (selectedObj == null && hoveredObj != null && GameInput.Instance.IsLMBPressedThisFrame())
        {
            if (hoveredObj.GetComponent<Computer>() != null)
            {
                SetSelectedObject(hoveredObj);
            }
        }
    }

    /// <summary>
    /// Handles deselection logic. Currently unused but overridden for future expandability.
    /// </summary>
    protected override void ProcessObjectDeselection()
    {
        // Intentionally left empty for custom logic.
    }

    /// <summary>
    /// Assigns hovered object and invokes base logic.
    /// </summary>
    /// <param name="objToHover">Object to hover.</param>
    protected override void SetHoveredObject(Transform objToHover)
    {
        base.SetHoveredObject(objToHover);
    }

    /// <summary>
    /// Assigns selected object, clearing the hovered object, and invokes base selection logic.
    /// </summary>
    /// <param name="objToSelect">Object to select.</param>
    protected override void SetSelectedObject(Transform objToSelect)
    {
        base.SetSelectedObject(objToSelect);
    }

    /// <summary>
    /// Deselects the currently selected object without firing any events.
    /// Useful for resetting state when another interaction begins.
    /// </summary>
    public void DeselectObj()
    {
        SetSelectedObject(null);
    }
}
