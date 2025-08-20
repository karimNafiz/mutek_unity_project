using UnityEngine;

namespace ScriptableObjects
{
    /// <summary>
    /// Content to be displayed on one page of the flipbook
    /// </summary>
    [CreateAssetMenu(fileName = "SO_FlipbookPageContent", menuName = "Scriptable Objects/Flipbook/SO_FlipbookPageContent")]
    public class SO_FlipbookPageContent : ScriptableObject
    {
        [field: SerializeField]
        public string PageContent { get; set; }
        
        /*
         * Possible extensions:
         * - Text settings, to control how the text should be displayed (font, size, color)
         * Though the text settings could almost all be configured by embedded tags. 
         * - Image content
         */
    }
}

