using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SO_ScoreMarkerStyle",
    menuName = "Scriptable Objects/SO_ScoreMarkerStyle")]
    public class SO_ScoreMarkerStyle : ScriptableObject
    {
        public Sprite leftNormal, leftSelected;
        public Sprite centerNormal, centerSelected;
        public Sprite rightNormal, rightSelected;
    }
}

