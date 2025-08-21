using UnityEngine;
using SelectableBase;
using System;
using GameManager;
using InputActons;
using UnityEngine.UI;
using Unity.Cinemachine;
using Appearable;
using Utility.Lerp;

public class Computer : SelectableObjBase
{
    [SerializeField] private Canvas computerCanvas;
    [SerializeField] private Image screen;
    [SerializeField] private CinemachineCamera camPosCompNotSelected;
    [SerializeField] private CinemachineCamera camPosCompSelected;
    private void RegisterToSelectorEvents()
    {
        /*
            these two functions already exist in the base class
            if I want my own implementation, I need to override them 
         
         */
        ComputerSelector_ObjSelectorBase.Instance.OnSelectObj += ObjSelector_OnSelectObj;
        ComputerSelector_ObjSelectorBase.Instance.OnHoverObj += ObjSelector_OnHoverObj;

    }

    private void DeregisterToSelectorEvents()
    {
        ComputerSelector_ObjSelectorBase.Instance.OnSelectObj -= ObjSelector_OnSelectObj;
        ComputerSelector_ObjSelectorBase.Instance.OnHoverObj -= ObjSelector_OnHoverObj;

    }

    void Start()
    {
        if (screen == null)
        {
            throw new Exception("screen is not set ");
        
        }
        if (computerCanvas == null)
        {
            throw new Exception("screen is not set ");
            
        }
        //screen.SetLerpTechnique(new TimeLerp(0.5f)); /* this is the brainchild ray and karim fuck */
        TurnOffScreen();
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
        TurnOnScreen();
    }
    private void InputEvents_OnQPressed(object sender, EventArgs e)
    {

        InputEvents.Instance.OnQPress -= InputEvents_OnQPressed; // unregister from Q press event
        ComputerSelector_ObjSelectorBase.Instance.DeselectObj(); // deselect the computer object
        TurnOffScreen();
    }

    /*
        the image i have a serialized reference to is a black screen infront of the chat UI
            when the computer is selected, I make the screen transparent
        when the computer is deselected, I make the screen opaque again
     
     */
    private void TurnOnScreen() 
    {
        //screen.Hide(); // hide the screen, this will make it transparent    
        Color color = screen.color;
        color.a = 0.0f; // make the screen transparent
        screen.color = color;
        camPosCompNotSelected.gameObject.SetActive(false);
        camPosCompSelected.gameObject.SetActive(true);
        computerCanvas.GetComponent<GraphicRaycaster>().enabled = true;
    }
    private void TurnOffScreen() 
    {
        Color color = screen.color;
        color.a = 1.0f; // make the screen transparent
        screen.color = color;
        //screen.Show();
        camPosCompNotSelected.gameObject.SetActive(true);
        camPosCompSelected.gameObject.SetActive(false);
        computerCanvas.GetComponent<GraphicRaycaster>().enabled = false;
    }
}