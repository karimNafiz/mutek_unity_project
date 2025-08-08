using Utility.HTTP;
using UnityEngine;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    [SerializeField] private HTTPClient client;
    [SerializeField] private string domain;
    [SerializeField] private string port;
    [SerializeField] private string endpoint;
    private void Start()
    {

        string url = $"{domain}:{port}{endpoint}";
        StartCoroutine(client.GetRequest(url , (string response)=> 
        {
            /*
            Dictionary<string, object> res = HTTPClient.ParseJsonToDictionary(response);
            foreach (KeyValuePair<string , object> pair in res) 
            {
                Debug.Log($"key {pair.Key} value {pair.Value?.ToString()}");
            }
            */
            Debug.Log(response);

        } , 
        (string err)=> 
        {
            Debug.Log($" error {err} ");
        }));
        
    }
}
