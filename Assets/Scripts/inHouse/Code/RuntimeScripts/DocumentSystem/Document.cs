using System;
using InputActons;
using SelectableBase;
using UnityEngine;

public class Document : SelectableObjBase
{
    private void RegisterToSelectorEvents()
    {
        /*
            these two functions already exist in the base class
            if I want my own implementation, I need to override them 
         
         */
        DocumentSelector_ObjSelectorBase.Instance.OnSelectObj += ObjSelector_OnSelectObj;
        DocumentSelector_ObjSelectorBase.Instance.OnHoverObj += ObjSelector_OnHoverObj;

    }

    private void DeregisterToSelectorEvents()
    {
        DocumentSelector_ObjSelectorBase.Instance.OnSelectObj -= ObjSelector_OnSelectObj;
        DocumentSelector_ObjSelectorBase.Instance.OnHoverObj -= ObjSelector_OnHoverObj;

    }

    void Start()
    {
        RegisterToSelectorEvents();
    }


    protected override void ObjSelector_OnHoverObj(object sender, ObjSelectionArgs e)
    {
        base.ObjSelector_OnHoverObj(sender, e); // default hover VFX
    }

    protected override void ObjSelector_OnSelectObj(object sender, ObjSelectionArgs e)
    {
        if (!e._transform == transform) return;
        
        InvokeOnObjSelected(this, EventArgs.Empty);   // fire SelectableObjBase event
        
        InputEvents.Instance.OnQPress += InputEvents_OnQPressed; // register to Q press event

    }
    private void InputEvents_OnQPressed(object sender , EventArgs e) 
    {  
        
        InputEvents.Instance.OnQPress -= InputEvents_OnQPressed; // register to Q press event
        DocumentSelector_ObjSelectorBase.Instance.DeselectObj(); // deselect this document
    
    }

}
