using Utility.Singleton;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;



/*
    TODO: need better error handling, at this point in time its a bit hard to tell whats the reason for the failure in the web request
 
 */



namespace Utility.HTTP
{
    public class HTTPClient : SingletonMonoBehavior<HTTPClient> 
    {
        // GET Request
        public IEnumerator GetRequest(string url, Action<string> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    onError?.Invoke(request.error);
                }
                else
                {
                    onSuccess?.Invoke(request.downloadHandler.text);
                }
            }
        }

        // POST Request with JSON body
        public IEnumerator PostRequest(string url, string jsonBody, Action<string> onSuccess, Action<string> onError)
        {
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(request.error);
            }
            else
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
        }
        public string SerializeDict(Dictionary<string, string> dict) 
        {
            return JsonConvert.SerializeObject(dict);
        }
        public Dictionary<string, object> ParseJsonToDictionary(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
        public List<Dictionary<string, object>> ParseJsonToDictionaryList(string json) 
        {
            return JsonConvert.DeserializeObject<List<Dictionary<string , object>>>(json);
        }
    }
}