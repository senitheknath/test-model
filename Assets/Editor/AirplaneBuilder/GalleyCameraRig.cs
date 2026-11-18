using System.IO;
using UnityEditor;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class GalleyCameraRig
    {
        public static void BuildCameras(Transform parent)
        {
            CreateCam(parent, "Cam_01_EnterFromAisle", new Vector3(0.15f, 1.55f, -4.75f), new Vector3(0.2f, 1.25f, -1.65f), 80f);
            CreateCam(parent, "Cam_02_CoffeeMakers", new Vector3(-0.95f, 1.45f, -4.40f), new Vector3(-2.0f, 1.50f, -4.45f), 55f);
            CreateCam(parent, "Cam_03_StorageBlock", new Vector3(0.70f, 1.35f, -3.00f), new Vector3(1.30f, 1.25f, -1.65f), 65f);
            CreateCam(parent, "Cam_04_ServiceCart", new Vector3(1.10f, 1.25f, -3.30f), new Vector3(1.00f, 0.70f, -2.45f), 65f);
            CreateCam(parent, "Cam_05_Ovens", new Vector3(0.95f, 1.30f, -4.40f), new Vector3(2.0f, 1.20f, -4.45f), 50f);
            CreateCam(parent, "Cam_06_Lockers", new Vector3(-0.60f, 1.50f, -3.30f), new Vector3(-2.0f, 1.95f, -4.30f), 70f);
            CreateCam(parent, "Cam_07_PrepCounter", new Vector3(-0.85f, 1.55f, -3.25f), new Vector3(-2.0f, 0.95f, -4.45f), 60f);
            CreateCam(parent, "Cam_08_WasteBin", new Vector3(0.90f, 1.15f, -3.20f), new Vector3(1.80f, 0.40f, -2.50f), 60f);
            CreateCam(parent, "Cam_09_Intercom", new Vector3(-1.00f, 1.45f, -2.95f), new Vector3(-2.0f, 1.45f, -1.95f), 55f);
            CreateCam(parent, "Cam_10_ExitJumpseat", new Vector3(-0.70f, 1.35f, -4.20f), new Vector3(-2.0f, 1.05f, -2.95f), 70f);
        }

        private static void CreateCam(Transform parent, string name, Vector3 worldPos, Vector3 worldLookAt, float fov)
        {
            GameObject camGo = new GameObject(name);
            camGo.transform.SetParent(parent, false);
            camGo.transform.position = worldPos;
            camGo.transform.LookAt(worldLookAt);

            Camera cam = camGo.AddComponent<Camera>();
            cam.fieldOfView = fov;
            cam.nearClipPlane = 0.05f;
            cam.farClipPlane = 100f;
            cam.enabled = false;
        }

        [MenuItem("Tools/Airplane/Capture Galley Views")]
        public static void CaptureGalleyViews()
        {
            GameObject galley = GameObject.Find("Galley_Front");
            if (galley == null)
            {
                Debug.LogWarning("Galley_Front object not found in scene. Run 'Tools > Airplane > Build Interior' first.");
                return;
            }

            Transform camRoot = galley.transform.Find("ViewCameras");
            if (camRoot == null)
            {
                Debug.LogWarning("ViewCameras container not found under Galley_Front.");
                return;
            }

            string outDir = "Assets/Reference/Output";
            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);

            // Handle Cam 04: open trolley and drawer
            Transform trolley = galley.transform.Find("MainBlock/LowerBays/Trolley_01");
            Transform drawer = trolley != null ? trolley.Find("Trolley_01_Drawer") : null;
            Vector3 origTrolleyPos = trolley != null ? trolley.localPosition : Vector3.zero;
            Vector3 origDrawerPos = drawer != null ? drawer.localPosition : Vector3.zero;

            RenderTexture rt = new RenderTexture(1024, 576, 24);
            Texture2D tex = new Texture2D(1024, 576, TextureFormat.RGB24, false);

            for (int i = 0; i < camRoot.childCount; i++)
            {
                Camera cam = camRoot.GetChild(i).GetComponent<Camera>();
                if (cam == null) continue;

                bool isCam4 = cam.gameObject.name.Contains("04");
                if (isCam4 && trolley != null && drawer != null)
                {
                    trolley.localPosition = origTrolleyPos + Vector3.back * 0.40f;
                    drawer.localPosition = origDrawerPos + Vector3.back * 0.35f;
                }

                cam.targetTexture = rt;
                cam.Render();
                RenderTexture.active = rt;
                tex.ReadPixels(new Rect(0, 0, 1024, 576), 0, 0);
                tex.Apply();
                cam.targetTexture = null;

                if (isCam4 && trolley != null && drawer != null)
                {
                    trolley.localPosition = origTrolleyPos;
                    drawer.localPosition = origDrawerPos;
                }

                byte[] bytes = tex.EncodeToPNG();
                string path = $"{outDir}/galley_{i + 1:D2}.png";
                File.WriteAllBytes(path, bytes);
            }

            RenderTexture.active = null;
            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(tex);
            AssetDatabase.Refresh();
            Debug.Log($"[GalleyCapture] Successfully captured 10 reference views to {outDir}/");
        }
    }
}
