using System;
using SelectableBase;
using UnityEngine;

/// <summary>
/// Based on the user mouse position, maps the camera target local position to a range of values 
/// </summary>
public class CameraTargetPositionController : MonoBehaviour
{
    // to what extent the target local Z position should be offset
    [SerializeField] private Vector2 targetPositionDeltaZMinMax;
    [SerializeField] private bool isInverseZ;
    // to what extent the target local Y position should be offset
    [SerializeField] private Vector2 targetPositionDeltaYMaxMax;
    [SerializeField] private bool isInverseY;

    private Vector3 originalTargetLocalPosition;
    private bool isEnabled = true;
    
    private void Start()
    {
        originalTargetLocalPosition = transform.localPosition;
        
        SelectableObjBase.OnAnyObjSelected += SelectableObjBase_OnAnyObjSelected;
        SelectableObjBase.OnAnyObjDeselected += SelectableObjBase_OnAnyObjDeselected;
    }

    private void OnDestroy()
    {
                
        SelectableObjBase.OnAnyObjSelected -= SelectableObjBase_OnAnyObjSelected;
        SelectableObjBase.OnAnyObjDeselected -= SelectableObjBase_OnAnyObjDeselected;
    }

    private void SelectableObjBase_OnAnyObjSelected(object sender, EventArgs e)
    {
        Disable();
    }

    private void SelectableObjBase_OnAnyObjDeselected(object sender, EventArgs e)
    {
        Enable();
    }    

    private void LateUpdate()
    {
        if (!isEnabled)
        {
            return;
        }
        
        Vector2 mousePosition = Input.mousePosition;

        // calculate ther percentage of the mouse position that is within the screen
        float lerpFactorZ = Mathf.InverseLerp(0, Screen.width, mousePosition.x);
        float lerpFactorY = Mathf.InverseLerp(0, Screen.height, mousePosition.y);

        if (isInverseZ)
        {
            lerpFactorZ = 1 - lerpFactorZ;
        }

        if (isInverseY)
        {
            lerpFactorY = 1 - lerpFactorY;       
        }
        
        // calculate the target local position
        Vector3 targetLocalPosition = originalTargetLocalPosition;
        targetLocalPosition.z += Mathf.Lerp(targetPositionDeltaZMinMax.x, targetPositionDeltaZMinMax.y, lerpFactorZ);
        targetLocalPosition.y += Mathf.Lerp(targetPositionDeltaYMaxMax.x, targetPositionDeltaYMaxMax.y, lerpFactorY);
        
        // set the target local position
        transform.localPosition = targetLocalPosition;
    }

    public void Disable()
    {
        isEnabled = false;
    }

    public void Enable()
    {
        isEnabled = true;
    }
}
