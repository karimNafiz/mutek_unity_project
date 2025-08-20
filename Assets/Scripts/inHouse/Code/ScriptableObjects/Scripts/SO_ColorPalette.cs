using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_ColorPalette", 
        menuName = "Scriptable Objects/Color Config/SO_ColorPalette")]
    public class SO_ColorPalette : ScriptableObject
    {
        // Color palette supposed to contain the following for the game to use:
        // The following colors should be common to all color palette and all game mode
        // White Color
        // Deselected Color
        [field: SerializeField]
        public Color VanillaColor { get; private set; }
        [field: SerializeField]
        public Color NormalColor { get; private set; }

        [field: SerializeField]
        public Color AccentColor { get; private set; }
        [field: SerializeField]
        public Color HighlightColor { get; private set; }
        

        [field: SerializeField]
        public Material NormalMat { get; private set; }
        [field: SerializeField]
        public Material AccentMat { get; private set; }

        [field: SerializeField]
        public Material HighlightMat { get; private set; }

        [field: SerializeField]
        public Material CompletedMat { get; private set; }
    }
}