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


public class ChatRoomVisual : SingletonMonoBehavior<ChatRoomVisual>
{
   
    // have a script for this
    [SerializeField] private MessageVisualManager messageContainer;
    private ChatBotVisual currentChatBotVisual;
    private EventBinding<OnChatBotVisualClicked> eventBinding_OnChatBotVisualClicked;
    private void Awake()
    {

        eventBinding_OnChatBotVisualClicked = new EventBinding<OnChatBotVisualClicked>(EventBus_OnChatBotVisualClicked);
        EventBus<OnChatBotVisualClicked>.Register(eventBinding_OnChatBotVisualClicked);
    }
    

    // TODO try to remove http logic from this class mainly concerned UI
    private void EventBus_OnChatBotVisualClicked(OnChatBotVisualClicked eventArgs) 
    {
        if (currentChatBotVisual != null && currentChatBotVisual == eventArgs._chatBotVisual) return;
        // we need to do a web request
        WebServerClient.Instance.GetMessages(GlobalConfigs.Instance.GetServerUrl(), GlobalConfigs.Instance.globalConstant.message_endpoint_get, eventArgs._chatBotVisual.Bot, OnGetMessageSuccess, OnGetMessageErr);
    
    }

    private void OnGetMessageSuccess(List<Message> messages) 
    {
        messageContainer.AddMessageVisuals(messages);          
    
    }
    private void OnGetMessageErr(Exception e) 
    {
        Debug.Log($"encountered exception while trying to read messages from the webser exception -> {e.Message}");
    
    }





}
