using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UserData : MonoBehaviour
{
    [SerializeField] public UserData_SO userDataSO;

    public List<MessageData> messagesData = new List<MessageData>();
}
