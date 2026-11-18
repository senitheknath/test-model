using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace AirplaneBuilder
{
    public static class MaterialLibrary
    {
        private const string MaterialFolderPath = "Assets/Materials/Airplane";

        public static Material Wall { get; private set; }
        public static Material WallShadow { get; private set; }
        public static Material Floor { get; private set; }
        public static Material AisleStripe { get; private set; }
        public static Material CeilingLight { get; private set; }

        public static Material SeatRed { get; private set; }
        public static Material SeatRedDark { get; private set; }
        public static Material SeatBlue { get; private set; }
        public static Material SeatIndigo { get; private set; }

        public static Material GalleyCabinet { get; private set; }
        public static Material GalleyTrolley { get; private set; }
        public static Material GalleyAccent { get; private set; }

        public static Material CockpitWall { get; private set; }
        public static Material CockpitPanel { get; private set; }
        public static Material CockpitSeat { get; private set; }

        public static Material HorizonOrange { get; private set; }
        public static Material HorizonBlue { get; private set; }

        public static Material Toilet { get; private set; }
        public static Material Sink { get; private set; }
        public static Material Mirror { get; private set; }
        public static Material IndicatorGreen { get; private set; }
        public static Material IndicatorRed { get; private set; }
        public static Material WindowGlass { get; private set; }

        public static Shader DetectShader()
        {
            // Check URP
            Shader s = Shader.Find("Universal Render Pipeline/Lit");
            if (s != null) return s;

            // Check HDRP
            s = Shader.Find("HDRP/Lit");
            if (s != null) return s;

            // Built-in Standard
            s = Shader.Find("Standard");
            if (s != null) return s;

            return Shader.Find("Diffuse") ?? Shader.Find("Mobile/Diffuse");
        }

        public static void InitializeMaterials()
        {
            if (!Directory.Exists(MaterialFolderPath))
            {
                Directory.CreateDirectory(MaterialFolderPath);
                AssetDatabase.Refresh();
            }

            Shader litShader = DetectShader();

            Wall = GetOrCreateMaterial("Mat_Wall", litShader, InteriorConfig.ColorWall);
            WallShadow = GetOrCreateMaterial("Mat_WallShadow", litShader, InteriorConfig.ColorWallShadow);
            Floor = GetOrCreateMaterial("Mat_Floor", litShader, InteriorConfig.ColorFloor);
            AisleStripe = GetOrCreateMaterial("Mat_AisleStripe", litShader, InteriorConfig.ColorAisleStripe);
            CeilingLight = GetOrCreateMaterial("Mat_CeilingLight", litShader, InteriorConfig.ColorCeilingLight, true, InteriorConfig.ColorCeilingLight * 1.5f);

            SeatRed = GetOrCreateMaterial("Mat_SeatRed", litShader, InteriorConfig.ColorSeatRed);
            SeatRedDark = GetOrCreateMaterial("Mat_SeatRedDark", litShader, InteriorConfig.ColorSeatRedDark);
            SeatBlue = GetOrCreateMaterial("Mat_SeatBlue", litShader, InteriorConfig.ColorSeatBlue);
            SeatIndigo = GetOrCreateMaterial("Mat_SeatIndigo", litShader, InteriorConfig.ColorSeatIndigo);

            GalleyCabinet = GetOrCreateMaterial("Mat_GalleyCabinet", litShader, InteriorConfig.ColorGalleyCabinet);
            GalleyTrolley = GetOrCreateMaterial("Mat_GalleyTrolley", litShader, InteriorConfig.ColorGalleyTrolley);
            GalleyAccent = GetOrCreateMaterial("Mat_GalleyAccent", litShader, InteriorConfig.ColorGalleyAccent);

            CockpitWall = GetOrCreateMaterial("Mat_CockpitWall", litShader, InteriorConfig.CockpitWallLit);
            CockpitPanel = GetOrCreateMaterial("Mat_CockpitPanel", litShader, InteriorConfig.CockpitPanelBody);
            CockpitSeat = GetOrCreateMaterial("Mat_CockpitSeat", litShader, InteriorConfig.CockpitSeatCharcoal);

            HorizonOrange = GetOrCreateMaterial("Mat_HorizonOrange", litShader, InteriorConfig.CockpitHorizonOrange, true, InteriorConfig.CockpitHorizonOrange * 1.8f);
            HorizonBlue = GetOrCreateMaterial("Mat_HorizonBlue", litShader, InteriorConfig.CockpitHorizonBlue, true, InteriorConfig.CockpitHorizonBlue * 1.8f);

            Toilet = GetOrCreateMaterial("Mat_Toilet", litShader, InteriorConfig.ColorToilet);
            Sink = GetOrCreateMaterial("Mat_Sink", litShader, InteriorConfig.ColorSink);
            Mirror = GetOrCreateMaterial("Mat_Mirror", litShader, InteriorConfig.ColorMirror, false, Color.black, 0.9f);
            IndicatorGreen = GetOrCreateMaterial("Mat_IndicatorGreen", litShader, InteriorConfig.ColorIndicatorGreen, true, InteriorConfig.ColorIndicatorGreen * 2.0f);
            IndicatorRed = GetOrCreateMaterial("Mat_IndicatorRed", litShader, InteriorConfig.ColorIndicatorRed, true, InteriorConfig.ColorIndicatorRed * 2.0f);
            WindowGlass = GetOrCreateMaterial("Mat_WindowGlass", litShader, InteriorConfig.ColorWindowGlass, false, Color.black, 0.4f);

            AssetDatabase.SaveAssets();
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

            // Universal/Standard properties
            if (mat.HasProperty("_BaseColor"))
                mat.SetColor("_BaseColor", color);
            if (mat.HasProperty("_Color"))
                mat.SetColor("_Color", color);

            if (mat.HasProperty("_Smoothness"))
                mat.SetFloat("_Smoothness", smoothness);
            if (mat.HasProperty("_Glossiness"))
                mat.SetFloat("_Glossiness", smoothness);

            if (mat.HasProperty("_Metallic"))
                mat.SetFloat("_Metallic", 0.0f);

            if (isEmissive)
            {
                Color em = (emissionColor != default) ? emissionColor : color;
                mat.EnableKeyword("_EMISSION");
                if (mat.HasProperty("_EmissionColor"))
                    mat.SetColor("_EmissionColor", em);
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }

            EditorUtility.SetDirty(mat);
            return mat;
        }
    }
}
