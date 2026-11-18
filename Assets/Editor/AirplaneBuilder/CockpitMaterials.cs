using UnityEditor;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class CockpitMaterials
    {
        private const string MaterialFolderPath = "Assets/Materials/Airplane/Cockpit";

        public static Material ShellLit { get; private set; }
        public static Material ShellShadow { get; private set; }
        public static Material Panel { get; private set; }
        public static Material Dark { get; private set; }
        public static Material Floor { get; private set; }
        public static Material Seat { get; private set; }
        public static Material NightSky { get; private set; }
        public static Material HorizonHaze { get; private set; }
        public static Material DialFace { get; private set; }
        public static Material HorizonBlue { get; private set; }
        public static Material HorizonOrange { get; private set; }
        public static Material EmissiveGreen { get; private set; }

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
                AssetDatabase.CreateFolder("Assets/Materials/Airplane", "Cockpit");
            }

            Shader litShader = FindAppropriateShader();

            ShellLit = GetOrCreateMaterial("Mat_Cockpit_ShellLit", litShader, InteriorConfig.CockpitWallLit);
            ShellShadow = GetOrCreateMaterial("Mat_Cockpit_ShellShadow", litShader, InteriorConfig.CockpitWallShadow);
            Panel = GetOrCreateMaterial("Mat_Cockpit_Panel", litShader, InteriorConfig.CockpitPanelBody);
            Dark = GetOrCreateMaterial("Mat_Cockpit_Dark", litShader, InteriorConfig.CockpitDarkInsert);
            Floor = GetOrCreateMaterial("Mat_Cockpit_Floor", litShader, InteriorConfig.CockpitFloor);
            Seat = GetOrCreateMaterial("Mat_Cockpit_Seat", litShader, InteriorConfig.CockpitSeatCharcoal);
            NightSky = GetOrCreateMaterial("Mat_Cockpit_NightSky", litShader, InteriorConfig.CockpitNightSky);
            HorizonHaze = GetOrCreateMaterial("Mat_Cockpit_HorizonHaze", litShader, InteriorConfig.CockpitHorizonHaze);
            DialFace = GetOrCreateMaterial("Mat_Cockpit_DialFace", litShader, InteriorConfig.CockpitDialFace);
            HorizonBlue = GetOrCreateMaterial("Mat_Cockpit_HorizonBlue", litShader, InteriorConfig.CockpitHorizonBlue, true, InteriorConfig.CockpitHorizonBlue * 2.0f);
            HorizonOrange = GetOrCreateMaterial("Mat_Cockpit_HorizonOrange", litShader, InteriorConfig.CockpitHorizonOrange, true, InteriorConfig.CockpitHorizonOrange * 2.0f);
            EmissiveGreen = GetOrCreateMaterial("Mat_Cockpit_EmissiveGreen", litShader, InteriorConfig.CockpitBtnGreen, true, InteriorConfig.CockpitBtnGreen * 2.2f);

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
