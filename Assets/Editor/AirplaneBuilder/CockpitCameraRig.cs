using System.IO;
using UnityEditor;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class CockpitCameraRig
    {
        public static void BuildCameras(Transform parent)
        {
            CreateRefCam(parent, "Cam_01_CaptainAhead", new Vector3(-0.55f, 1.35f, -0.05f), new Vector3(0f, 0f, 0f), 60f);
            CreateRefCam(parent, "Cam_02_CopilotView", new Vector3(0.55f, 1.35f, -0.05f), new Vector3(8f, -22f, 0f), 60f);
            CreateRefCam(parent, "Cam_03_OverheadPOV", new Vector3(0f, 0.95f, 0.10f), new Vector3(-55f, 0f, 0f), 65f);
            CreateRefCam(parent, "Cam_04_PedestalThrottles", new Vector3(0f, 1.30f, 0.10f), new Vector3(48f, 0f, 0f), 60f);
            CreateRefCam(parent, "Cam_05_SideWindow", new Vector3(-0.20f, 1.30f, -0.10f), new Vector3(0f, -75f, 0f), 60f);
            CreateRefCam(parent, "Cam_06_InstrumentPanel", new Vector3(0f, 1.20f, 0.20f), new Vector3(22f, 0f, 0f), 62f);
            CreateRefCam(parent, "Cam_07_JumpseatPOV", new Vector3(-0.20f, 1.35f, -1.05f), new Vector3(6f, 0f, 0f), 60f);
            CreateRefCam(parent, "Cam_08_SideConsoleLeft", new Vector3(-0.55f, 1.25f, 0.15f), new Vector3(45f, -55f, 0f), 60f);
            CreateRefCam(parent, "Cam_09_GlareshieldAutopilot", new Vector3(0f, 1.35f, 0.45f), new Vector3(15f, 0f, 0f), 58f);
            CreateRefCam(parent, "Cam_10_RearWallDoor", new Vector3(0f, 1.25f, 0.20f), new Vector3(0f, 180f, 0f), 65f);
        }

        private static void CreateRefCam(Transform parent, string name, Vector3 localPos, Vector3 localEuler, float fov)
        {
            GameObject camGo = new GameObject(name);
            camGo.transform.SetParent(parent, false);
            camGo.transform.localPosition = localPos;
            camGo.transform.localEulerAngles = localEuler;

            Camera cam = camGo.AddComponent<Camera>();
            cam.fieldOfView = fov;
            cam.nearClipPlane = 0.05f;
            cam.farClipPlane = 100f;
            cam.enabled = false; // Kept disabled for user selection
        }

        [MenuItem("Tools/Airplane/Capture Cockpit Views")]
        public static void CaptureCockpitViews()
        {
            GameObject cockpit = GameObject.Find("Cockpit");
            if (cockpit == null)
            {
                Debug.LogWarning("Cockpit object not found in scene. Run 'Tools > Airplane > Build Interior' first.");
                return;
            }

            Transform camRoot = cockpit.transform.Find("ViewCameras");
            if (camRoot == null)
            {
                Debug.LogWarning("ViewCameras container not found under Cockpit.");
                return;
            }

            string outDir = "Assets/Reference/Output";
            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);

            RenderTexture rt = new RenderTexture(1024, 576, 24);
            Texture2D tex = new Texture2D(1024, 576, TextureFormat.RGB24, false);

            for (int i = 0; i < camRoot.childCount; i++)
            {
                Camera cam = camRoot.GetChild(i).GetComponent<Camera>();
                if (cam == null) continue;

                cam.targetTexture = rt;
                cam.Render();
                RenderTexture.active = rt;
                tex.ReadPixels(new Rect(0, 0, 1024, 576), 0, 0);
                tex.Apply();
                cam.targetTexture = null;

                byte[] bytes = tex.EncodeToPNG();
                string path = $"{outDir}/cockpit_{i + 1:D2}.png";
                File.WriteAllBytes(path, bytes);
            }

            RenderTexture.active = null;
            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(tex);
            AssetDatabase.Refresh();
            Debug.Log($"[CockpitCapture] Successfully captured 10 reference views to {outDir}/");
        }
    }
}
