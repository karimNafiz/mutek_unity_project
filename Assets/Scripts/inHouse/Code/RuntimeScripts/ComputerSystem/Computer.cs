using UnityEngine;
using SelectableBase;
using System;
public class Computer : SelectableObjBase
{
    private void RegisterToSelectorEvents() 
    { 
        /*
            these two functions already exist in the base class
            if I want my own implementation, I need to override them 
         
         */
        ComputerSelector.Instance.OnSelectObj += ObjSelector_OnSelectObj;
        ComputerSelector.Instance.OnHoverObj += ObjSelector_OnHoverObj;

    }

    private void DeregisterToSelectorEvents() 
    { 
        ComputerSelector.Instance.OnSelectObj -= ObjSelector_OnSelectObj;
        ComputerSelector.Instance.OnHoverObj -= ObjSelector_OnHoverObj;

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
        if (e._transform == transform)
            InvokeOnObjSelected(this, EventArgs.Empty);   // fire SelectableObjBase event
    }
}
