using UnityEngine;

namespace ScriptableObjects
{

    [CreateAssetMenu(fileName = "SO_GlobalConstants", menuName = "Scriptable Objects/SO_GlobalConstants")]
    public class SO_GlobalConstants : ScriptableObject
    {
        public string domain;
        public string port;
        public string session_endpoint_post; 
        public string message_endpoint_post;


    }
}