

using UnityEngine;
using System.Collections.Generic;

namespace ScriptableObjects
{

    [CreateAssetMenu(fileName = "SO_FakeInteriorMaterialCollection", 
        menuName = "Scriptable Objects/SO_FakeInteriorMaterialCollection")]
    public class SO_FakeInteriorMaterialCollection : ScriptableObject
    {
        [SerializeField] private Shader shaderToFind;
        public List<Material> fakeInteriorMaterialList = new();

        public float targetWindowSmoothness = 1;
        public float targetEmissionValue = 0.02f;

#if UNITY_EDITOR
        [ContextMenu("Find Materials")]
        public void FindMaterials()
        {
            fakeInteriorMaterialList.Clear();

            string[] materialGUIDs = UnityEditor.AssetDatabase.FindAssets("t:Material");
            foreach (string guid in materialGUIDs)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                Material material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(path);

                if (material != null && material.shader == shaderToFind)
                {
                    fakeInteriorMaterialList.Add(material);
                }
            }
        }


        public Material FindRealMaterial(Material mat)
        {
            //Debug.Log(mat.name);
            string path = UnityEditor.AssetDatabase.GetAssetPath(mat);
            Material realMaterialAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(path);
            //Debug.Log(path);
            return realMaterialAsset;
        }
#endif

        [ContextMenu("Set All Window Smoothness")]
        public void SetAllWindowSmoothness()
        {
            foreach (Material material in fakeInteriorMaterialList)
            {
                material.SetFloat("_WindowSmoothness", targetWindowSmoothness);
            }
        }


        [ContextMenu("Set All Window Emission Values")]
        public void SetAllEmissionValues()
        {
            foreach (Material material in fakeInteriorMaterialList)
            {
                material.SetFloat("_WindowEmissionFactor", targetEmissionValue);
            }
        }

    }
}