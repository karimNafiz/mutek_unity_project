using Utility.HTTP;
using UnityEngine;
using System.Collections.Generic;
using Models.Bots;
using System;
using Utility;
using WebServer;

public class Test : MonoBehaviour
{
    [SerializeField] private HTTPClient client;
    [SerializeField] private string domain;
    [SerializeField] private string port;
    [SerializeField] private string endpoint;
    private void Start()
    {

        //string url = $"{domain}:{port}{endpoint}";
        //StartCoroutine(client.GetRequest(url , (string response)=> 
        //{
        //    /*
        //    Dictionary<string, object> res = HTTPClient.ParseJsonToDictionary(response);
        //    foreach (KeyValuePair<string , object> pair in res) 
        //    {
        //        Debug.Log($"key {pair.Key} value {pair.Value?.ToString()}");
        //    }
        //    */
        //    Debug.Log(response);

        //} , 
        //(string err)=> 
        //{
        //    Debug.Log($" error {err} ");
        //}));
        //string url = $"{domain}:{port}{endpoint}";
        Url url = new Url();
        url.domain = domain;
        url.port = port;



        WebServerClient.Instance.GetBots(url, endpoint, OnSuccessTest, OnErrorTest);
        

        
    }
    private void OnSuccessTest(HashSet<Bot> bots) 
    {
        foreach (Bot bot in bots) 
        {
            Debug.Log(bot);
        }
    }

    private void OnErrorTest(Exception e) 
    {
        Debug.Log(e.Message);
    }

}
