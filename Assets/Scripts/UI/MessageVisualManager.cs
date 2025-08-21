using UnityEngine;
using Models.Message;
using System.Collections.Generic;
using Utility.Singleton;
public class MessageVisualManager : SingletonMonoBehavior<MessageVisualManager> 
{
    // needs the MessageVisual prefab
    [SerializeField] private MessageVisual messageVisualPrefab;
    private List<MessageVisual> messages;
    // need a function to add
    // need a function to clear stuff
    private void Start()
    {
        messages = new List<MessageVisual>();
    }


    public void AddMessageVisual(Message message) 
    {
        AddMessageVisualUser(message.UserMessage, "user");
        AddMessageVisualBot(message.BotResponse, message.Bot.Name);
    
    }
    public void AddMessageVisualUser(string message, string sender) 
    {
        MessageVisual msgVisual = Instantiate(messageVisualPrefab, new Vector3(0, 0, 0), this.transform.rotation, this.transform);
        msgVisual.SetSenderFieldColor(Color.blue);
        msgVisual.SetSender(sender);
        msgVisual.SetMessage(message);
        messages.Add(msgVisual);
        /*
            get anchor position
         */
        if (msgVisual.TryGetComponent<RectTransform>(out RectTransform rTransform)) 
        { 
            rTransform.anchoredPosition3D = new Vector3(rTransform.anchoredPosition3D.x, rTransform.anchoredPosition3D.y, 0); // Adjust the Y position based on the number of messages
        }

    
    }
    public void AddMessageVisualBot(string message, string sender) 
    {
        MessageVisual msgVisual = Instantiate(messageVisualPrefab, new Vector3(0, 0, 0), this.transform.rotation, this.transform);
        msgVisual.SetSenderFieldColor(Color.red);
        msgVisual.SetSender(sender);
        msgVisual.SetMessage(message);
        messages.Add(msgVisual);
        if (msgVisual.TryGetComponent<RectTransform>(out RectTransform rTransform))
        {
            rTransform.anchoredPosition3D = new Vector3(rTransform.anchoredPosition3D.x, rTransform.anchoredPosition3D.y, 0); // Adjust the Y position based on the number of messages
        }

    }


    public void AddMessageVisuals(List<Message> messages) 
    {
        foreach (Message message in messages) 
        {
            AddMessageVisual(message);
        
        }
    
    }
    public void Clear() 
    {
        foreach (MessageVisual messageVisual in messages) 
        {
            Destroy(messageVisual.gameObject);
        
        }
        messages.Clear();
    
    }



}
