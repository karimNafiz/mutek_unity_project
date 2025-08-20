using UnityEngine;
using GameManager;
using UnityEditor;

namespace Appearable
{
    public class AppearableOutline: AppearableBaseLerp
    {
        [SerializeField] 
        // private Renderer renderer; 
        // supporting multiple renderers
        private Renderer[] renderers;
        
        [SerializeField] Color originalSelectionColor = Color.white;
        
        // cached MaterialPropertyBlock of the renderer
        // Make sure this is the only script that updates this renderer's block, 
        // otherwise we will need to Get-Modify-Write everytime to make sure the changes are in sync. 
        private MaterialPropertyBlock propertyBlock;
        
        // cached color IDs for efficient access, because:
        /*
         * https://docs.unity3d.com/ScriptReference/Shader.PropertyToID.html
         * Using property identifiers is more efficient than passing strings to all material property functions.
         * For example if you are calling Material.SetColor a lot, or using MaterialPropertyBlock,
         * then it is better to get the identifiers of the properties you need just once.
         */
        private const string PropertyNameSelectionColor = "_SelectionColor";
        private int propertyIDSelectionColor;
        
        // hardcoded const outline layer name
        private const string OutlineLayerName = "Outline";
        private int outlineLayer;
        // private int originalLayer;
        private int[] originalLayers;
        
        private void Awake()
        {
            // Get and Cache the IDs for the colors
            propertyIDSelectionColor = Shader.PropertyToID(PropertyNameSelectionColor);
            propertyBlock = new MaterialPropertyBlock();
            
            // convert the layer name to layer ID
            outlineLayer = LayerMask.NameToLayer(OutlineLayerName);
            // originalLayer = renderer.gameObject.layer;
            originalLayers = new int[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                originalLayers[i] = renderers[i].gameObject.layer;
            }
        }

        private void Start()
        {
            // if (ColorPaletteManager.Instance)
            // {
            //     originalSelectionColor = ColorPaletteManager.Instance.OutlineColorPalette.NewObjColor;
            // }
            
            ApplyColor(originalSelectionColor);
        }
        
        public override void Show(float delay = 0)
        {
            if (!activeState)
            {
                SetOutlineLayer();
            }
            base.Show(delay);
        }

        public override void ShowImmediate()
        {
            SetOutlineLayer();
            base.ShowImmediate();
        }

        // function overload baby 
        public void ShowImmediate(Color outlineColor) 
        {
            // this function will just set the color 
            SetOutlineColor(outlineColor);
            // and call the original ShowImmediate function 
            ShowImmediate();
        }





        [ContextMenu("Show")]
        public void TestShow()
        {
            Show();
        }

        
        [ContextMenu("Hide")]
        public override void Hide()
        {
            if (activeState) 
            {
                SetOutlineLayer();
            }
            base.Hide();
        }

        public override void HideImmediate()
        {
            SetOriginalLayer();
            base.HideImmediate();
        }

        protected override void OnCompleteHide()
        {
            SetOriginalLayer();
        }

        protected override void ApplyLerpFactor(float factor)
        {
            Color color = new Color(originalSelectionColor.r, originalSelectionColor.g, 
                originalSelectionColor.b, factor);
            ApplyColor(color);
        }

        public void ChangeOutlineColor(Color outlineColor)
        {
            SetOutlineColor(outlineColor);
            ApplyColor(outlineColor);
        }
        
        /// <summary>
        /// Apply a new border and fill color to the material property block.
        /// </summary>
        /// <param name="color"></param>
        private void ApplyColor(Color color)
        {
            // propertyBlock.SetColor(propertyIDSelectionColor, color);
            // renderer.SetPropertyBlock(propertyBlock);
            foreach (Renderer r in renderers)
            {
                r.GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor(propertyIDSelectionColor, color);
                r.SetPropertyBlock(propertyBlock);
            }
        }

        private void SetOutlineLayer()
        {
            // renderer.gameObject.layer = outlineLayer;
            foreach (Renderer r in renderers)
            {
                r.gameObject.layer = outlineLayer;
            }
        }

        private void SetOriginalLayer()
        {
            // renderer.gameObject.layer = originalLayer;
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].gameObject.layer = originalLayers[i];
            }
        }

        private void SetOutlineColor(Color outlineColor) 
        {
            // if (outlineColor == Color.green) 
            // {
            //     
            //     originalSelectionColor = ColorPaletteManager.Instance.OutlineColorPalette.NewObjColor;
            //     return;
            // }
            //
            // originalSelectionColor = ColorPaletteManager.Instance.OutlineColorPalette.RemoveObjColor;

            originalSelectionColor = outlineColor;
        }
    }
}