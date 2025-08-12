using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UIElementClickCatcher : MonoBehaviour, IPointerClickHandler
{
    public event EventHandler OnUIClicked;  
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnUIClicked?.Invoke(this, EventArgs.Empty);

    }


}
