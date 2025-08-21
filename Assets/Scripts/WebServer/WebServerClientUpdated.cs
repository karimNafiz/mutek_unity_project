using System;
using Utility.Singleton;
using System.Collections.Generic;
using Utility.HTTP;
using Exceptions;
using UnityEngine;

namespace WebServer
{
    public class WebServerClientUpdated : SingletonMonoBehavior<WebServerClientUpdated>
    {
        // keep the current session id returned by /session/new
        public string CurrentSessionId { get; private set; } = null;

        /*
            the placement of code here is dogshit ik don't judge me
         
         */
        private void Start()
        {
            CreateSession(GlobalConfigs.Instance.GetServerUrl(), GlobalConfigs.Instance.globalConstant.session_endpoint_post, (string session) => { CurrentSessionId = session; Debug.Log($"created a new session -> {CurrentSessionId} "); }, (Exception e) => 
            { 
                Debug.LogWarning("failed to create session exception -> " + e.Message); 
            });
        }




        // ---- 1) Create a new session ----
        // POST {url}{endpoint}  -> expects JSON: { "session_id": "...", "message": "...", "log_file": "..." }
        public void CreateSession(Url url, string endpoint, Action<string> onSuccess, Action<Exception> onErr)
        {
            string finalEndpoint = $"{url.GetHostUrl()}{endpoint}"; // e.g., http://127.0.0.1:8080/session/new
            Debug.Log($"Creating session at {finalEndpoint}");
            // server expects POST; no body required. If your HTTPClient needs a body, send "{}".
            string body = "{}";

            Action<string> onSuccCallback = (string json) =>
            {
                try
                {
                    // parse into a single Dictionary (NOT list)
                    Dictionary<string, object> dict = HTTPClient.Instance.ParseJsonToDictionary(json);

                    if (!dict.ContainsKey("session_id"))
                        throw new BadRequestException("response missing 'session_id'");

                    CurrentSessionId = dict["session_id"].ToString();

                    // optional: log file path if you want to show it in UI
                    if (dict.ContainsKey("log_file"))
                        Debug.Log($"Log file: {dict["log_file"]}");

                    onSuccess?.Invoke(CurrentSessionId);
                }
                catch (Exception ex)
                {
                    onErr?.Invoke(ex);
                }
            };

            Action<string> onErrCallback = (string err) =>
            {
                onErr?.Invoke(new BadConnectionException(err));
            };

            StartCoroutine(HTTPClient.Instance.PostRequest(finalEndpoint, body, onSuccCallback, onErrCallback));
        }

        // ---- 2) Send a message ----
        // POST {url}{endpoint} with body { "session_id": "...", "message": "..." }
        // Expects JSON: { "session_id", "friend_reply", "suspicion_score", "reasoning", "history_length" }
        public void SendMessage(Url url, string endpoint, string userMessage,
                                Action<FriendSurveillanceResponse> onSuccess,
                                Action<Exception> onErr)
        {
            if (string.IsNullOrEmpty(CurrentSessionId))
            {
                onErr?.Invoke(new InvalidOperationException("No session. Call CreateSession first."));
                return;
            }

            string finalEndpoint = $"{url.GetHostUrl()}{endpoint}"; // e.g., http://127.0.0.1:8080/message

            // build request body
            var bodyDict = new Dictionary<string, object>
            {
                { "session_id", CurrentSessionId },
                { "message", userMessage }
            };
            string jsonBody = HTTPClient.Instance.SerializeDict(bodyDict);

            Action<string> onSuccCallback = (string json) =>
            {
                try
                {
                    Dictionary<string, object> dict = HTTPClient.Instance.ParseJsonToDictionary(json);

                    // basic validation
                    if (!dict.ContainsKey("session_id") || !dict.ContainsKey("friend_reply") ||
                        !dict.ContainsKey("suspicion_score") || !dict.ContainsKey("reasoning"))
                    {
                        throw new BadRequestException("response missing required keys");
                    }

                    // update session id if backend rotated it (it shouldn't, but just in case)
                    CurrentSessionId = dict["session_id"].ToString();

                    var resp = new FriendSurveillanceResponse
                    {
                        FriendReply = dict["friend_reply"]?.ToString() ?? "",
                        Reasoning = dict["reasoning"]?.ToString() ?? "",
                        SuspicionScore = SafeToFloat(dict, "suspicion_score"),
                        HistoryLength = SafeToInt(dict, "history_length")
                    };

                    onSuccess?.Invoke(resp);
                }
                catch (Exception ex)
                {
                    onErr?.Invoke(ex);
                }
            };

            Action<string> onErrCallback = (string err) =>
            {
                onErr?.Invoke(new BadConnectionException(err));
            };

            StartCoroutine(HTTPClient.Instance.PostRequest(finalEndpoint, jsonBody, onSuccCallback, onErrCallback));
        }

        // --- helpers to parse numbers safely from Dictionary<string, object> ---
        private static float SafeToFloat(Dictionary<string, object> dict, string key, float fallback = 0f)
        {
            try
            {
                if (!dict.ContainsKey(key) || dict[key] == null) return fallback;
                return Convert.ToSingle(dict[key]);
            }
            catch { return fallback; }
        }

        private static int SafeToInt(Dictionary<string, object> dict, string key, int fallback = 0)
        {
            try
            {
                if (!dict.ContainsKey(key) || dict[key] == null) return fallback;
                return Convert.ToInt32(dict[key]);
            }
            catch { return fallback; }
        }
    }

    // response DTO you can pass around your UI/gameplay
    public class FriendSurveillanceResponse
    {
        public string FriendReply;
        public float SuspicionScore;
        public string Reasoning;
        public int HistoryLength;
    }
}
