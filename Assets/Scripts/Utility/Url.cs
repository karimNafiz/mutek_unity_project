using UnityEngine;

public struct Url 
{
    string domain;
    string port;
    public string GetHostUrl() 
    {
        return $"{domain}:{port}";
    }
}