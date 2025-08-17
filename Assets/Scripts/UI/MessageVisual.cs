using UnityEngine;
using Models.Message;
using TMPro;
public class MessageVisual : MonoBehaviour
{
        
    [SerializeField] private TMP_Text messageTextField;
    [SerializeField] private TMP_Text senderNameTextField;
    [SerializeField] private TMP_Text timeField;
    private Color msgColor = Color.black;

    public void SetMessage(string message) 
    {
        messageTextField.text = message;
    }
    public void SetSender(string sender) 
    {
        senderNameTextField.text = sender;
    }

    public void SetTextColor(Color color) 
    {
        this.msgColor = color;
        messageTextField.color = this.msgColor;
    }
    public void SetTime(string time) 
    {
        timeField.text = time;
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
