using UnityEditor;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class CabinMaterials
    {
        private const string FolderPath = "Assets/Materials/Airplane/Cabin";

        public static Material WallUpper { get; private set; }
        public static Material WallShadow { get; private set; }
        public static Material WallLower { get; private set; }
        public static Material Ceiling { get; private set; }
        public static Material CeilingCenter { get; private set; }
        public static Material Floor { get; private set; }
        public static Material AisleGuide { get; private set; }
        public static Material BinDoor { get; private set; }
        public static Material BinUnderside { get; private set; }
        public static Material BinLatch { get; private set; }
        public static Material BinService { get; private set; }
        public static Material CeilingLight { get; private set; }

        public static Material SeatRed { get; private set; }
        public static Material SeatRedDark { get; private set; }
        public static Material SeatBlue { get; private set; }
        public static Material SeatBlueDark { get; private set; }
        public static Material SeatIndigo { get; private set; }
        public static Material SeatIndigoDark { get; private set; }
        public static Material SeatFrame { get; private set; }
        public static Material TrayPanel { get; private set; }
        public static Material TrayLatch { get; private set; }
        public static Material MeshNet { get; private set; }
        public static Material CrewSeat { get; private set; }
        public static Material ExitSign { get; private set; }

        public static void Initialize()
        {
            EnsureFolderExists();
            Shader shader = FindLitShader();

            WallUpper = GetOrCreate("Mat_Cabin_WallUpper", shader, InteriorConfig.Hex("#D9D3B8"));
            WallShadow = GetOrCreate("Mat_Cabin_WallShadow", shader, InteriorConfig.Hex("#B5AF95"));
            WallLower = GetOrCreate("Mat_Cabin_WallLower", shader, InteriorConfig.Hex("#CFC9AE"));
            Ceiling = GetOrCreate("Mat_Cabin_Ceiling", shader, InteriorConfig.Hex("#CFC9B0"));
            CeilingCenter = GetOrCreate("Mat_Cabin_CeilingCenter", shader, InteriorConfig.Hex("#E4DFC6"));
            Floor = GetOrCreate("Mat_Cabin_Floor", shader, InteriorConfig.Hex("#5A5A5E"));
            AisleGuide = GetOrCreate("Mat_Cabin_AisleGuide", shader, InteriorConfig.Hex("#E6E6E0"));
            BinDoor = GetOrCreate("Mat_Cabin_BinDoor", shader, InteriorConfig.Hex("#E0DAC0"));
            BinUnderside = GetOrCreate("Mat_Cabin_BinUnderside", shader, InteriorConfig.Hex("#A9A38A"));
            BinLatch = GetOrCreate("Mat_Cabin_BinLatch", shader, InteriorConfig.Hex("#8C8670"));
            BinService = GetOrCreate("Mat_Cabin_BinService", shader, InteriorConfig.Hex("#3A3A3E"));
            CeilingLight = GetOrCreate("Mat_Cabin_CeilingLight", shader, InteriorConfig.Hex("#FFF3C4"), true, InteriorConfig.Hex("#FFF3C4") * 2.5f);

            SeatRed = GetOrCreate("Mat_Cabin_SeatRed", shader, InteriorConfig.Hex("#A3123A"));
            SeatRedDark = GetOrCreate("Mat_Cabin_SeatRedDark", shader, InteriorConfig.Hex("#7A0E2C"));
            SeatBlue = GetOrCreate("Mat_Cabin_SeatBlue", shader, InteriorConfig.Hex("#3F7FB8"));
            SeatBlueDark = GetOrCreate("Mat_Cabin_SeatBlueDark", shader, InteriorConfig.Hex("#2F5F8F"));
            SeatIndigo = GetOrCreate("Mat_Cabin_SeatIndigo", shader, InteriorConfig.Hex("#3A2F8F"));
            SeatIndigoDark = GetOrCreate("Mat_Cabin_SeatIndigoDark", shader, InteriorConfig.Hex("#2A2268"));
            SeatFrame = GetOrCreate("Mat_Cabin_SeatFrame", shader, InteriorConfig.Hex("#9A9A96"));
            TrayPanel = GetOrCreate("Mat_Cabin_TrayPanel", shader, InteriorConfig.Hex("#A9A9A4"));
            TrayLatch = GetOrCreate("Mat_Cabin_TrayLatch", shader, InteriorConfig.Hex("#6B6F76"));
            MeshNet = GetOrCreate("Mat_Cabin_MeshNet", shader, InteriorConfig.Hex("#26262B"));
            CrewSeat = GetOrCreate("Mat_Cabin_CrewSeat", shader, InteriorConfig.Hex("#2F2F33"));
            ExitSign = GetOrCreate("Mat_Cabin_ExitSign", shader, InteriorConfig.Hex("#8A1530"), true, InteriorConfig.Hex("#B3213F") * 2.2f);

            AssetDatabase.SaveAssets();
        }

        public static Material GetOrCreate(string name, Shader shader, Color color, bool isEmissive = false, Color emissionColor = default)
        {
            EnsureFolderExists();
            string path = $"{FolderPath}/{name}.mat";
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat == null)
            {
                mat = new Material(shader);
                mat.enableInstancing = true;
                AssetDatabase.CreateAsset(mat, path);
            }
            else if (mat.shader != shader)
            {
                mat.shader = shader;
            }

            mat.enableInstancing = true;

            if (mat.HasProperty("_BaseColor"))
                mat.SetColor("_BaseColor", color);
            else if (mat.HasProperty("_Color"))
                mat.SetColor("_Color", color);

            if (mat.HasProperty("_Smoothness"))
                mat.SetFloat("_Smoothness", InteriorConfig.DefaultSmoothness);
            else if (mat.HasProperty("_Glossiness"))
                mat.SetFloat("_Glossiness", InteriorConfig.DefaultSmoothness);

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

        private static Shader FindLitShader()
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

        private static void EnsureFolderExists()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Materials"))
                AssetDatabase.CreateFolder("Assets", "Materials");
            if (!AssetDatabase.IsValidFolder("Assets/Materials/Airplane"))
                AssetDatabase.CreateFolder("Assets/Materials", "Airplane");
            if (!AssetDatabase.IsValidFolder(FolderPath))
                AssetDatabase.CreateFolder("Assets/Materials/Airplane", "Cabin");
        }
    }
}
