using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UIElementClickCatcher : MonoBehaviour, IPointerClickHandler
{
    public event EventHandler OnUIClicked;  
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"UI area clicked {eventData.position}");

    }

    public void InvokeOnUIClicked() 
    {
        OnUIClicked?.Invoke(this, EventArgs.Empty);
    }
}
