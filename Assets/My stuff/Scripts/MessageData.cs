using UnityEngine;

//Stores the data for a single message
public class MessageData
{
    public string Username = "null";
    public string MessageText = "null";
    public string TimeMessageSent = "null";
    public Color UserColor = Color.black;

    public MessageData(string username, string messageText, string timeMessageSent, Color userColor)
    {
        Username = username;
        MessageText = messageText;
        TimeMessageSent = timeMessageSent;
        UserColor = userColor;
    }

    public override string ToString()
    {
        return $"{Username}: {MessageText} ({TimeMessageSent})";
    }
}
