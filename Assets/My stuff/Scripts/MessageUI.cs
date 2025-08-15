using UnityEngine;
using TMPro;

//Handles setting the UI elements
public class MessageUI : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI timeMessageSent;
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI messageText;
    private MessageData messageData;

    public void SetMessageData(MessageData data)
    {
        messageData = data;
    }
    public void SetMessageData(string s1, string s2, string s3, Color color)
    {
        messageData = new MessageData(s1, s2, s3, color);
    }

    public string GetTimeMessageSent()
    {
        return messageData.TimeMessageSent;
    }
    public void SetTimeMessageSent(string s)
    {
        timeMessageSent.SetText(s);
        messageData.TimeMessageSent = s;
    }

    public string GetUsername()
    {
        return messageData.Username;
    }
    public void SetUsername(string s, Color? color = null)
    {
        if (color.HasValue)
        {
            Color c = color.Value;
            c.a = 1f; // Ensure fully opaque
            username.color = c;
        }
        else
        {
            username.color = Color.black;
        }

        username.SetText(s);
        //set data for message history
        messageData.UserColor = username.color;
        messageData.Username = s;
    }

    public string GetMessageText()
    {
        return messageData.MessageText;
    }
    public void SetMessageText(string s)
    {
        messageText.SetText(s);
        messageData.MessageText = s;
    }

}
