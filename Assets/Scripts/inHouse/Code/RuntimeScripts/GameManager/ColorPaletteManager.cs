using UnityEngine;
using Utility;
using ScriptableObjects;

namespace GameManager
{
    // TODO: change implementation of this when switching to new UI. 

    /// <summary>
    /// Singleton manager responsible for providing global access to predefined color palettes.
    /// Typically used by UI components to maintain visual consistency across game systems.
    /// </summary>
    public class ColorPaletteManager : SingletonMonoBehaviour<ColorPaletteManager>
    {
        //[SerializeField] private SO_ColorPalette[] colorPaletteCollection;

        [SerializeField] private SO_ColorPalette standardColorPalette;

        /// <summary>
        /// Gets the standard color palette used for general UI elements.
        /// </summary>
        public SO_ColorPalette StandardColorPalette
        {
            get
            {
                return standardColorPalette;
            }
        }
        
        [SerializeField] private SO_OutlineColorPalette outlineColorPalette;
        public SO_OutlineColorPalette OutlineColorPalette
        {
            get
            {
                return outlineColorPalette;
            }
        }
    }
}