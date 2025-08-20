using UnityEngine;
using System.Collections.Generic;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewPrefabCollection", 
        menuName = "Scriptable Objects/SO_PrefabCollection")]
    public class SO_PrefabCollection : ScriptableObject
    {
        public List<GameObject> prefabs;
    }
}