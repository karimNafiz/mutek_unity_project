using UnityEngine;
using Utility.Singleton;
using ScriptableObjects;
public class GlobalConfigs: SingletonMonoBehavior<GlobalConfigs> 
{
    [SerializeField] private SO_GlobalConstants globalConstant;

    public string GetBotEndpoint(int id) 
    {
        return $"{globalConstant.bot_endpoint_get}/{id}";
    
    }
}
