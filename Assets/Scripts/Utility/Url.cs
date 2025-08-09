using UnityEngine;

public struct Url 
{
    public string domain;
    public string port;
    public string GetHostUrl() 
    {
        return $"{domain}:{port}";
    }
}