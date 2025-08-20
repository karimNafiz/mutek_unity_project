using UnityEngine;

namespace ScriptableObjects
{
    //TODO: come up with a more generic solution for the color palette management system using SO
    [CreateAssetMenu(fileName = "SO_OutlineColorPalette", 
        menuName = "Scriptable Objects/Color Config/SO_OutlineColorPalette")]
    public class SO_OutlineColorPalette: ScriptableObject
    {
        [field: SerializeField]
        public Color NewObjColor { get; private set; }

        [field: SerializeField]
        public Color RemoveObjColor { get; private set; }
    }
}