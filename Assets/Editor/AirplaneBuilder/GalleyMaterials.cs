using UnityEditor;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class GalleyMaterials
    {
        private const string MaterialFolderPath = "Assets/Materials/Airplane/Galley";

        public static Material WallLit { get; private set; }
        public static Material WallShadow { get; private set; }
        public static Material CeilingFacet { get; private set; }
        public static Material WallTrim { get; private set; }
        public static Material Floor { get; private set; }
        public static Material Cabinet { get; private set; }
        public static Material DoorInset { get; private set; }
        public static Material CounterTop { get; private set; }
        public static Material CounterEdge { get; private set; }
        public static Material Trolley { get; private set; }
        public static Material OvenBody { get; private set; }
        public static Material GlassDark { get; private set; }
        public static Material Maroon { get; private set; }
        public static Material ExitGlow { get; private set; }
        public static Material CeilingLight { get; private set; }
        public static Material DoorLeaf { get; private set; }

        public static void Initialize()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Materials/Airplane"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Materials"))
                    AssetDatabase.CreateFolder("Assets", "Materials");
                AssetDatabase.CreateFolder("Assets/Materials", "Airplane");
            }
            if (!AssetDatabase.IsValidFolder(MaterialFolderPath))
            {
                AssetDatabase.CreateFolder("Assets/Materials/Airplane", "Galley");
            }

            Shader litShader = FindAppropriateShader();

            WallLit = GetOrCreateMaterial("Mat_Galley_WallLit", litShader, InteriorConfig.GalleyWallLit);
            WallShadow = GetOrCreateMaterial("Mat_Galley_WallShadow", litShader, InteriorConfig.GalleyWallShadow);
            CeilingFacet = GetOrCreateMaterial("Mat_Galley_CeilingFacet", litShader, InteriorConfig.GalleyCeilingFacet);
            WallTrim = GetOrCreateMaterial("Mat_Galley_WallTrim", litShader, InteriorConfig.GalleyWallTrim);
            Floor = GetOrCreateMaterial("Mat_Galley_Floor", litShader, InteriorConfig.GalleyFloor);
            Cabinet = GetOrCreateMaterial("Mat_Galley_Cabinet", litShader, InteriorConfig.GalleyCabinet);
            DoorInset = GetOrCreateMaterial("Mat_Galley_DoorInset", litShader, InteriorConfig.GalleyDoorInset);
            CounterTop = GetOrCreateMaterial("Mat_Galley_CounterTop", litShader, InteriorConfig.GalleyCounterTop);
            CounterEdge = GetOrCreateMaterial("Mat_Galley_CounterEdge", litShader, InteriorConfig.GalleyCounterEdge);
            Trolley = GetOrCreateMaterial("Mat_Galley_Trolley", litShader, InteriorConfig.GalleyTrolley);
            OvenBody = GetOrCreateMaterial("Mat_Galley_OvenBody", litShader, InteriorConfig.GalleyOvenBody);
            GlassDark = GetOrCreateMaterial("Mat_Galley_GlassDark", litShader, InteriorConfig.GalleyGlass);
            Maroon = GetOrCreateMaterial("Mat_Galley_Maroon", litShader, InteriorConfig.GalleyMaroon);
            ExitGlow = GetOrCreateMaterial("Mat_Galley_ExitGlow", litShader, InteriorConfig.GalleyExitGlow, true, InteriorConfig.GalleyExitGlow * 2.5f);
            CeilingLight = GetOrCreateMaterial("Mat_Galley_CeilingLight", litShader, InteriorConfig.ColorCeilingLight, true, InteriorConfig.ColorCeilingLight * 2.0f);
            DoorLeaf = GetOrCreateMaterial("Mat_Galley_DoorLeaf", litShader, InteriorConfig.GalleyDoorLeaf);

            AssetDatabase.SaveAssets();
        }

        private static Shader FindAppropriateShader()
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
                shader = Shader.Find("HDRP/Lit");
            if (shader == null)
                shader = Shader.Find("Standard");
            if (shader == null)
                shader = Shader.Find("Diffuse");
            return shader;
        }

        private static Material GetOrCreateMaterial(string name, Shader shader, Color color, bool isEmissive = false, Color emissionColor = default, float smoothness = 0.1f)
        {
            string path = $"{MaterialFolderPath}/{name}.mat";
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat == null)
            {
                mat = new Material(shader);
                AssetDatabase.CreateAsset(mat, path);
            }
            else if (mat.shader != shader)
            {
                mat.shader = shader;
            }

            if (mat.HasProperty("_BaseColor"))
                mat.SetColor("_BaseColor", color);
            else if (mat.HasProperty("_Color"))
                mat.SetColor("_Color", color);

            if (mat.HasProperty("_Smoothness"))
                mat.SetFloat("_Smoothness", smoothness);
            else if (mat.HasProperty("_Glossiness"))
                mat.SetFloat("_Glossiness", smoothness);

            if (mat.HasProperty("_Metallic"))
                mat.SetFloat("_Metallic", InteriorConfig.DefaultMetallic);

            if (isEmissive)
            {
                mat.EnableKeyword("_EMISSION");
                if (mat.HasProperty("_EmissionColor"))
                    mat.SetColor("_EmissionColor", emissionColor);
            }

            EditorUtility.SetDirty(mat);
            return mat;
        }
    }
}
