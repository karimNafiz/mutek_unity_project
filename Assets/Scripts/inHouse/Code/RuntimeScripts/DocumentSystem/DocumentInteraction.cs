using UnityEngine;
using System;
using GameManager;
using System.Collections;
[RequireComponent(typeof(Document))]
public class DocumentInteraction : MonoBehaviour
{
    private Document document_SelectableObjBase;

    //private Coroutine detectQCoroutine;

    private bool isSelected = false;

    private void Awake()
    {
        document_SelectableObjBase = GetComponent<Document>();
        if (document_SelectableObjBase == null)
        {
            throw new Exception("Document_SelectableObjBase is null");

        }
    }

    private void Start()
    {
        /*
            register to the event
         */
        document_SelectableObjBase.OnObjSelected += Document_SelectableObjBase_OnObjSelected;
        //GameInput.Instance.IsQKeyPerformedThisFrame += GameInput_IsQKeyPerformedThisFrame;
    }
    private void OnDestroy()
    {
        document_SelectableObjBase.OnObjSelected -= Document_SelectableObjBase_OnObjSelected;
    }

    private void Document_SelectableObjBase_OnObjSelected(object sender, EventArgs e)
    {
        if (isSelected) return;
        isSelected = true;
        Debug.Log("Document object selected, activating flip book interaction script");
        InputActons.InputEvents.Instance.OnQPress += GameInput_IsQKeyPerformedThisFrame; // note this is an action and an event EventHandler


    }
    private void GameInput_IsQKeyPerformedThisFrame(object sender , EventArgs e)
    {
        if (isSelected)
        {
            /*
                deactivate the flip book interaction script
             */
            isSelected = false;
            InputActons.InputEvents.Instance.OnQPress -= GameInput_IsQKeyPerformedThisFrame; // remove the event handler
            Debug.Log("Q key pressed, deselecting document object");
            DocumentSelector_ObjSelectorBase.Instance.DeselectObj();
        }
    }



}
