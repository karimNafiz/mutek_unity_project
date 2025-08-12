using UnityEngine;
using Utility.Singleton;
using EventHandling;
using EventHandling.Events;
using WebServer;
using System.Collections.Generic;
using Models.Message;
using System;

/*
    I hate this class, no seperate between UI logic and backend stuff
    don't judge my ass based on this code
 
 */
/*
    current plan of action 
    we need a script for the TextInputVisual
    we neet to listen to its event OnTextInput -> this event is run when the submit button is run
    we get the string from the OnTextInput
    we add it to the MessageContainer
    
 */


public class ChatRoomVisual : SingletonMonoBehavior<ChatRoomVisual>
{
   
    // this will handle all the message visual pop ups
    [SerializeField] private MessageVisualManager messageContainer;
    // this handles getting new text input from the user
    [SerializeField] private TextInputVisual textInput;

    // this represents the current chat bot that we are speaking to 
    private ChatBotVisual currentChatBotVisual;
    // we use this to keep track of which chat bot the player is trying to speak
    private EventBinding<OnChatBotVisualClicked> eventBinding_OnChatBotVisualClicked;
    private void Awake()
    {

        eventBinding_OnChatBotVisualClicked = new EventBinding<OnChatBotVisualClicked>(EventBus_OnChatBotVisualClicked);
        EventBus<OnChatBotVisualClicked>.Register(eventBinding_OnChatBotVisualClicked);
    }
    private void Start()
    {
        textInput.OnTextEnter += TextInput_OnTextEnter;
    }

    private void TextInput_OnTextEnter(object sender, string message)
    {
        // take the text, and add it to the message container
        messageContainer.AddMessageVisualUser(message);
        // I need to send this information back to the server 
        WebServerClient.Instance.SendMessageToBot(GlobalConfigs.Instance.GetServerUrl(), GlobalConfigs.Instance.globalConstant.message_endpoint_post, message , currentChatBotVisual.Bot,OnPostMessageSuccess, OnPostMessageErr );
      
    }
    private void OnPostMessageSuccess(string responseFrmBot) 
    {
        messageContainer.AddMessageVisualBot(responseFrmBot);
    }
    private void OnPostMessageErr(Exception e) 
    {
        Debug.LogWarning($" exception encountered while trying to send message to bot exception ->{e.Message}");

    
    }


    // TODO try to remove http logic from this class mainly concerned UI
    private void EventBus_OnChatBotVisualClicked(OnChatBotVisualClicked eventArgs) 
    {
        if (currentChatBotVisual != null && currentChatBotVisual == eventArgs._chatBotVisual) return;
        // we first clear everything
        messageContainer.Clear();
        // we need to do a web request
        Debug.Log($"getting the messaeges for bot id {eventArgs._chatBotVisual.Bot.ID}");
        WebServerClient.Instance.GetMessages(GlobalConfigs.Instance.GetServerUrl(), GlobalConfigs.Instance.globalConstant.message_endpoint_get, eventArgs._chatBotVisual.Bot, OnGetMessageSuccess, OnGetMessageErr);
    
    }

    private void OnGetMessageSuccess(List<Message> messages) 
    {
        Debug.Log($"adding messages in the message container ");
        messageContainer.AddMessageVisuals(messages);          
    
    }
    private void OnGetMessageErr(Exception e) 
    {
        Debug.Log($"encountered exception while trying to read messages from the webser exception -> {e.Message}");
    
    }





}
