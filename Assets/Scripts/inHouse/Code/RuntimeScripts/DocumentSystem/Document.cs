using System;
using Flipbook;
using InputActons;
using ScriptableObjects;
using SelectableBase;
using UnityEngine;

public class Document : SelectableObjBase
{

    [SerializeField] private SO_FlipbookPageCollection document_FlipBook;




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
        if (document_FlipBook == null) 
        { 
            throw new Exception(tag + ": Document_FlipBook is not assigned!");
        }

        RegisterToSelectorEvents();
    }


    protected override void ObjSelector_OnHoverObj(object sender, ObjSelectionArgs e)
    {
        base.ObjSelector_OnHoverObj(sender, e); // default hover VFX
    }

    protected override void ObjSelector_OnSelectObj(object sender, ObjSelectionArgs e)
    {
        if (!(e._transform == transform)) return;
        
        InvokeOnObjSelected(this, EventArgs.Empty);   // fire SelectableObjBase event
        
        InputEvents.Instance.OnQPress += InputEvents_OnQPressed; // register to Q press event

        // need to set the book collection for the flip book
        FlipbookController.Instance.SetFlipbookPageCollection(document_FlipBook);
        FlipbookController.Instance.Show(); // show the flipbook
        FlipbookController.Instance.HoldUpFlipbook(); // set the book to be up
        
        // hide the object
        gameObject.SetActive(false);
    }
    
    
    private void InputEvents_OnQPressed(object sender , EventArgs e) 
    {  
        InvokeOnObjDeselected(this, EventArgs.Empty);   // fire SelectableObjBase event
        
        InputEvents.Instance.OnQPress -= InputEvents_OnQPressed; // register to Q press event
        DocumentSelector_ObjSelectorBase.Instance.DeselectObj(); // deselect this document

        //FlipbookController.Instance.Hide(); // hide the flipbook    
        
        FlipbookController.Instance.PutDownFlipbook(); // set the book to be down

        // show the object
        gameObject.SetActive(true);

    }

}
