using UnityEngine;
using Utility.Singleton;
using ScriptableObjects;
public class GlobalConfigs: SingletonMonoBehavior<GlobalConfigs> 
{
    public SO_GlobalConstants globalConstant;

    public string GetBotEndpoint(int id) 
    {
        return $"{globalConstant.bot_endpoint_get}/{id}";
    
    }

    public Url GetServerUrl() 
    {
        return new Url()
        {
            domain = globalConstant.domain,
            port = globalConstant.port
        };
    }
}
