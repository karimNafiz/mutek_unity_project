using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MessageHandler : MonoBehaviour
{

    [SerializeField] private Object messageTemplate;    //contains the components of a message.
    [SerializeField] private Transform chatHistoryContainer;     //chat history container
    [SerializeField] private TextMeshProUGUI emailTMP;  //Email.
    [SerializeField] private Image profilePicture;      //pfp.
    [SerializeField] private TMP_InputField inputField;     //enter text
    [SerializeField] private Button enterButton;     //enter button
    [SerializeField] private UserData_SO playerUserData;     //player user data for colors and stuff
    private UserData currentUserData;


    void Start()
    {
        enterButton.onClick.AddListener(UserSendsMessage);
    }

    void OnDestroy()
    {
        enterButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Load all the user's data into the respective sections
    /// </summary>
    public void OnContactSelected(UserData userData)
    {
        //set current user . HOPEFULLY a reference and not a copy
        currentUserData = userData;

        //load profile picture
        profilePicture.sprite = userData.userDataSO.profilePicture;

        //load email adress
        emailTMP.text = userData.userDataSO.email;

        //remove previous messages
        foreach (Transform child in chatHistoryContainer)
        {
            Destroy(child.gameObject);
        }

        //load new messages : for each message in userData
        print("Loading number of messages: " + userData.messagesData.Count);
        foreach (MessageData m in userData.messagesData)
        {
            print("Adding message: " + m.ToString());
            AddMessageToChatHistory(m);
        }
    }

/// <summary>
/// A general function that loads both player and contact messages into the chat history
/// </summary>
    private void AddMessageToChatHistory(MessageData m)
    {
        // Instantiate the template as a GameObject
        GameObject messageGO = Instantiate(messageTemplate, chatHistoryContainer) as GameObject;
        // Access the Message script and set the parameters
        MessageUI messageScript = messageGO.GetComponent<MessageUI>();


        if (messageScript != null) //fill properties
        {
            messageScript.SetMessageData(m);
            messageScript.SetUsername(m.Username, m.UserColor);
            messageScript.SetTimeMessageSent(m.TimeMessageSent);
            messageScript.SetMessageText(m.MessageText);
        }
    }

    /// <summary>
    /// A function that handles adding comments as the player types them
    /// </summary>
    private void UserSendsMessage()
    {
        // Instantiate the template as a GameObject
        GameObject newMessageObject = Instantiate(messageTemplate, chatHistoryContainer) as GameObject;
        // Access the Message script and set the parameters
        MessageUI messageScript = newMessageObject.GetComponent<MessageUI>();

        MessageData newMessageData = new MessageData(
            playerUserData.otherPersonsUsername,
            inputField.text,
            System.DateTime.Now.ToString("hh:mm tt"),
            playerUserData.profileColor
        );

        //Set the message in the UI
        messageScript.SetMessageData(newMessageData); //add messageData
        messageScript.SetUsername(playerUserData.otherPersonsUsername, playerUserData.profileColor);
        messageScript.SetTimeMessageSent(System.DateTime.Now.ToString("hh:mm tt"));
        messageScript.SetMessageText(inputField.text);

        //Save the info in the UserData message history
        currentUserData.messagesData.Add(newMessageData);
        ContactRespondsTemp();
    }

/// <summary>
/// A temporary function to show the other contact is responding
/// </summary>
    private void ContactRespondsTemp()
    {
        GameObject newMessageObject = Instantiate(messageTemplate, chatHistoryContainer) as GameObject;
        MessageUI messageScript = newMessageObject.GetComponent<MessageUI>();

        MessageData newMessageData = new MessageData(
            currentUserData.userDataSO.otherPersonsUsername,
            "Responding!",
            System.DateTime.Now.ToString("hh:mm tt"),
            currentUserData.userDataSO.profileColor
        );

        //Set the message in the UI
        messageScript.SetMessageData(newMessageData); //add messageData
        messageScript.SetUsername(currentUserData.userDataSO.otherPersonsUsername, currentUserData.userDataSO.profileColor);
        messageScript.SetTimeMessageSent(System.DateTime.Now.ToString("hh:mm tt"));
        messageScript.SetMessageText("Responding!");

        //Save the info in the UserData message history
        currentUserData.messagesData.Add(newMessageData);

    }


}
