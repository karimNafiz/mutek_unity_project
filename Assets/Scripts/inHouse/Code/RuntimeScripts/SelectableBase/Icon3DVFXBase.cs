using System.Collections.Generic;
using UnityEngine;
using GameManager;

namespace SelectableBase
{
    /// <summary>
    /// Base class for 3D icons that use particle systems and mesh renderers to represent visual states of the selectable icon.
    /// Derived classes can override methods to customize the colors and materials used for different states.
    /// </summary>
    public class Icon3DVFXBase : MonoBehaviour
    {
        /// <summary>
        /// Defines the visual states for the icon.
        /// </summary>
        public enum State
        {
            Normal,
            Highlighted,
            Selected
        }

        private bool isCompleted = false;

        [Header("Mesh")]
        [SerializeField] private MeshRenderer meshRenderer;


        [Header("Particle System")]
        // base particle systems.
        [SerializeField] private ParticleSystem[] normalParticleSystems;
        private Dictionary<State, List<ParticleSystemRenderer>> stateRendererDict = new();

        [SerializeField] private List<ParticleSystemRenderer> particleRenderersToDisableOnComplete;


        [Header("Animation")]
        // animation time for any type of dotween animation
        [SerializeField] protected float animationTime = 0.5f;

        protected virtual void Start()
        {
            InitParticleSystems();
        }

        /// <summary>
        /// Initializes particle systems for all icon states and configures their colors and visibility.
        /// </summary>
        protected void InitParticleSystems()
        {
            // initialize the dictionary with 3 states and 3 lists
            stateRendererDict.Add(State.Normal, new List<ParticleSystemRenderer>());
            stateRendererDict.Add(State.Highlighted, new List<ParticleSystemRenderer>());
            stateRendererDict.Add(State.Selected, new List<ParticleSystemRenderer>());
            //stateRendererDict.Add(State.Complete, new List<ParticleSystemRenderer>());

            for (int i = 0; i < normalParticleSystems.Length; i++)
            {
                ParticleSystemRenderer normalPSRenderer = normalParticleSystems[i].gameObject.GetComponent<ParticleSystemRenderer>();

                // set default color to the normalPS
                ParticleSystem.MainModule normalPSMainModule = normalParticleSystems[i].main;
                //normalPSMainModule.startColor = GetNormalColor();
                SetParticleStartColor(normalPSMainModule, GetNormalColor());

                // add the normalPS to the key=Normal List
                stateRendererDict[State.Normal].Add(normalPSRenderer);


                //if PS is part of the disable list, add it's clones to the disable list
                //if the clone matches something on our list, add it to the list.
                bool isAddCloneToDisableList = particleRenderersToDisableOnComplete.Contains(normalPSRenderer);

                // get the normal PS transform
                Transform normalPSTransform = normalParticleSystems[i].transform;

                // duplicate normalPS as highlightedPS
                ParticleSystem highlightedPS = Instantiate(normalPSTransform, normalPSTransform.parent).GetComponent<ParticleSystem>();
                // set the start color of the highlightedPS
                ParticleSystem.MainModule highlightedPSMainModule = highlightedPS.main;
                //highlightedPSMainModule.startColor = GetHighlightedColor();
                SetParticleStartColor(highlightedPSMainModule, GetHighlightedColor());
                // add the renderer to the key=Highlighted List
                ParticleSystemRenderer highlightedPSRenderer = highlightedPS.gameObject.GetComponent<ParticleSystemRenderer>();
                stateRendererDict[State.Highlighted].Add(highlightedPSRenderer);
                // turn off the highlightedPSRenderer
                highlightedPSRenderer.enabled = false;
                if (isAddCloneToDisableList)
                {
                    particleRenderersToDisableOnComplete.Add(highlightedPSRenderer);
                }

                // duplicate normalPS as selectedPS
                ParticleSystem selectedPS = Instantiate(normalPSTransform, normalPSTransform.parent).GetComponent<ParticleSystem>();
                // set the start color of the selectedPS
                ParticleSystem.MainModule selectedPSMainModule = selectedPS.main;
                //selectedPSMainModule.startColor = GetSelectedColor();
                SetParticleStartColor(selectedPSMainModule, GetSelectedColor());
                // add the renderer to the key=Selected List
                ParticleSystemRenderer selectedPSRenderer = selectedPS.gameObject.GetComponent<ParticleSystemRenderer>();
                stateRendererDict[State.Selected].Add(selectedPSRenderer);
                // turn off the selectedPSRenderer
                selectedPSRenderer.enabled = false;
                if (isAddCloneToDisableList)
                {
                    particleRenderersToDisableOnComplete.Add(selectedPSRenderer);
                }
            }
        }

        /// <summary>
        /// Gets the transform of the icon's mesh.
        /// </summary>
        /// <returns>Transform of the mesh renderer.</returns>
        public Transform GetMeshTransform() 
        { 
            return meshRenderer.transform;
        }

        public void TurnOffMeshRenderer()
        {
            meshRenderer.gameObject.SetActive(false);
        }


        /// <summary>
        /// Returns the color used for the normal state particle system from the game manager's color palette manager
        /// </summary>
        protected virtual Color GetNormalColor()
        {
            return ColorPaletteManager.Instance.StandardColorPalette.NormalColor;
        }

        /// <summary>
        /// Returns the color used for the highlighted state particle system from the game manager's color palette manager
        /// </summary>
        protected virtual Color GetHighlightedColor()
        {
            return ColorPaletteManager.Instance.StandardColorPalette.HighlightColor;
        }

        /// <summary>
        /// Returns the color used for the selected state particle system from the game manager's color palette manager
        /// </summary>
        protected virtual Color GetSelectedColor()
        {
            return ColorPaletteManager.Instance.StandardColorPalette.AccentColor;
        }


        /// <summary>
        /// Returns the material used for the normal state mesh from the game manager's color palette manager
        /// </summary>
        protected virtual Material GetNormalMat()
        {
            return ColorPaletteManager.Instance.StandardColorPalette.NormalMat;
        }

        /// <summary>
        /// Returns the material used for the highlighted state mesh from the game manager's color palette manager
        /// </summary>
        protected virtual Material GetHighlightedMat()
        {
            return ColorPaletteManager.Instance.StandardColorPalette.HighlightMat;
        }

        /// <summary>
        /// Returns the material used for the selected state mesh from the game manager's color palette manager
        /// </summary>
        protected virtual Material GetSelectedMat()
        {
            return ColorPaletteManager.Instance.StandardColorPalette.AccentMat;
        }

        /// <summary>
        /// Returns the material used for the completed state mesh from the game manager's color palette manager
        /// </summary>
        protected virtual Material GetCompletedMat()
        {
            return ColorPaletteManager.Instance.StandardColorPalette.CompletedMat;
        }

        /// <summary>
        /// Enables particle renderers corresponding to the specified state, disabling others.
        /// Respects the completed state by excluding designated renderers.
        /// </summary>
        /// <param name="targetState">The state to display.</param>
        public void ToggleHighlightPS(State targetState)
        {
            foreach (var kvp in stateRendererDict)
            {
                foreach (ParticleSystemRenderer renderer in kvp.Value)
                {
                    //if it is not completed, enable everything
                    if (!isCompleted) 
                    { 
                        renderer.enabled = (targetState == kvp.Key);
                    }
                    else //if it IS completed, enable only the things NOT on our no-no list
                    {
                        if(!particleRenderersToDisableOnComplete.Contains(renderer))
                        {
                            renderer.enabled = (targetState == kvp.Key);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Toggles the mesh renderer material between highlighted and normal.
        /// </summary>
        /// <param name="isHighlight">If true, use highlighted material; otherwise, normal.</param>
        public virtual void ToggleHighlightMesh(bool isHighlight)
        {
            meshRenderer.sharedMaterial = isHighlight ? GetHighlightedMat() : GetNormalMat();
        }


        /// <summary>
        /// Sets the mesh of the icon.
        /// </summary>
        /// <param name="mesh">The mesh to assign.</param>
        public void SetIconMesh(Mesh mesh)
        {
            meshRenderer.gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        /// <summary>
        /// Initializes the icon's material to the normal state.
        /// </summary>
        public void InitIconMat()
        {
            meshRenderer.sharedMaterial = GetNormalMat();
        }


        /// <summary>
        /// Marks the icon as completed, disabling specified particle renderers and applying a completed material.
        /// </summary>
        public virtual void SetComplete() 
        {
            // toggle the PS renderer to show the completed state set.
            foreach (ParticleSystemRenderer r in particleRenderersToDisableOnComplete)
            {
                r.enabled = false;
            }

            // set the mesh renderer to the completed material
            meshRenderer.sharedMaterial = GetCompletedMat();

            isCompleted = true;
        }


        /// <summary>
        /// Set the color of the particle system main module preserving the alpha
        /// </summary>
        /// <param name="mainModule">The main module of the particle system.</param>
        /// <param name="color">The color to apply.</param>
        private void SetParticleStartColor(ParticleSystem.MainModule mainModule, Color color)
        {
            Color mainModuleColor = mainModule.startColor.color;

            mainModule.startColor = new Color(color.r, color.g, color.b, mainModuleColor.a);
        }
    }
}

