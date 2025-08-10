using UnityEngine;
using Utility.Singleton;
using EventHandling;
using EventHandling.Events;
using Models.Bots;
using System.Collections.Generic;
public class ChatBotVisualManager : SingletonMonoBehavior<ChatBotVisualManager> 
{
    [SerializeField] private ChatBotVisual chatBotVisualPrefab;
    [SerializeField] private Transform chatBotVisualContainer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private EventBinding<OnBotManagerInitialized> eventBinding_OnBotManagerInitialized;
    private Dictionary<Bot, ChatBotVisual> botToChatBotVisualMapping;
    
    
    
    private void Awake()
    {
        eventBinding_OnBotManagerInitialized = new EventBinding<OnBotManagerInitialized>(EventBus_OnBotManagerInitialized);
        EventBus<OnBotManagerInitialized>.Register(eventBinding_OnBotManagerInitialized);
    }

    void Start()
    {
        
    }

    private void EventBus_OnBotManagerInitialized(OnBotManagerInitialized eventArgs) 
    {
        botToChatBotVisualMapping = new Dictionary<Bot, ChatBotVisual>();
        // this is for test, remove this later
        Debug.Log($"inside the callback and eventArgs._bots.Count -> {eventArgs._bots.Count}");
        // if not bot in the web server, then we do not do anything
        if (eventArgs._bots.Count == 0) return;
        foreach (KeyValuePair<int, Bot> pair in eventArgs._bots) 
        {
            botToChatBotVisualMapping.Add(pair.Value, InitializeChatBotVisual(pair.Value));
        
        }


    }

    private void OnDestroy()
    {
        EventBus<OnBotManagerInitialized>.Deregister(eventBinding_OnBotManagerInitialized);
    }


    private ChatBotVisual InitializeChatBotVisual(Bot bot) 
    {
        ChatBotVisual chatBotVisual = Instantiate(chatBotVisualPrefab,new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, chatBotVisualContainer);
        chatBotVisual.Bot = bot;
        chatBotVisual.OnChatBatVisualClicked += ChatBotVisual_OnChatBatVisualClicked;
        chatBotVisual.Show();
        return chatBotVisual;    
    
    }

    private void ChatBotVisual_OnChatBatVisualClicked(object obj, System.EventArgs e)
    {
        ChatBotVisual sender = (obj as ChatBotVisual);
        // if the sender isn't a chat bot visual we just return
        if (sender == null) return;

        EventBus<OnChatBotVisualClicked>.Raise(new OnChatBotVisualClicked()
        {
            _chatBotVisual = sender
        }) ;



        
        
    }
}
