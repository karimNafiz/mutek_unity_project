using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UserData_SO", menuName = "Scriptable Objects/UserData_SO")]
public class UserData_SO : ScriptableObject
{
    public string email;

    public Sprite profilePicture;

    public string otherPersonsUsername;

    public Color profileColor;
}
