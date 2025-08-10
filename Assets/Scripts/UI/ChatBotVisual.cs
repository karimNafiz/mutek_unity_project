using UnityEngine;
using Models.Bots;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using TMPro;
public class ChatBotVisual : MonoBehaviour
{
    private Bot bot;

    [SerializeField] private TMP_Text nameTextField; 
    [SerializeField] private TMP_Text statusTextField;
    // this gameobject will hold will raycasttarget so that we can detece clicks
    [SerializeField] private ChatBotVisualClickCatcher rayCastTarget;


    // events
    public event EventHandler OnChatBatVisualClicked;

    public Bot Bot
    {
        get { return bot; }
        set 
        {
            this.bot = value;
            this.nameTextField.text = this.bot.Name;
            this.statusTextField.text = this.bot.Status.ToString();
        }
    }



    public void Show() 
    {
        this.gameObject.SetActive(true);
        this.rayCastTarget.OnUIClicked += RayCastTarget_OnUIClicked;
    
    }

    private void RayCastTarget_OnUIClicked(object sender, EventArgs e)
    {
        OnChatBatVisualClicked?.Invoke(this, EventArgs.Empty);
    }

    public void Hide() 
    {
        this.rayCastTarget.OnUIClicked -= RayCastTarget_OnUIClicked;
        this.gameObject.SetActive(false);
    }
}
