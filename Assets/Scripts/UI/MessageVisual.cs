using UnityEngine;
using Models.Message;
using TMPro;
public class MessageVisual : MonoBehaviour
{
        
    [SerializeField] private TMP_Text messageTextField;


    public void SetMessage(string message) 
    {
        messageTextField.text = message;
    }
    public void Show() 
    {
        this.gameObject.SetActive(true);
    }
    public void Hide() 
    {
        this.gameObject.SetActive(false);
    
    }
}
