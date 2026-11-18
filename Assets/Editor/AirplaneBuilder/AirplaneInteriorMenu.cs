using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AirplaneBuilder
{
    public static class AirplaneInteriorMenu
    {
        private const string RootName = "AirplaneInterior";
        private const string PrefabFolderPath = "Assets/Prefabs";
        private const string PrefabPath = "Assets/Prefabs/AirplaneInterior.prefab";
        private const string SceneFolderPath = "Assets/Scenes";
        private const string WalkTestScenePath = "Assets/Scenes/WalkTest.unity";

        [MenuItem("Tools/Airplane/Build Interior")]
        public static void BuildInterior()
        {
            // 1. Delete any existing AirplaneInterior in the scene (Idempotent)
            GameObject existing = GameObject.Find(RootName);
            while (existing != null)
            {
                Object.DestroyImmediate(existing);
                existing = GameObject.Find(RootName);
            }

            // 2. Initialize Shared Materials
            MaterialLibrary.InitializeMaterials();

            // 3. Create Root
            GameObject root = new GameObject(RootName);
            root.transform.position = Vector3.zero;
            root.transform.rotation = Quaternion.identity;

            // 4. Build Sections in requested Hierarchy order:
            // AirplaneInterior > Cockpit / Galley_Front / Lav_01 / Cabin / Lav_02 / Lav_03 / Galley_Rear
            CockpitBuilder.Build(root.transform);
            GalleyBuilder.Build(root.transform, GalleyType.Front);
            LavatoryBuilder.BuildLav01(root.transform);
            CabinBuilder.Build(root.transform);
            LavatoryBuilder.BuildLav02(root.transform);
            LavatoryBuilder.BuildLav03(root.transform);
            GalleyBuilder.Build(root.transform, GalleyType.Rear);
            FuselageBuilder.Build(root.transform);

            // 5. Save as Prefab
            if (!Directory.Exists(PrefabFolderPath))
            {
                Directory.CreateDirectory(PrefabFolderPath);
                AssetDatabase.Refresh();
            }

            GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(root, PrefabPath, InteractionMode.AutomatedAction);
            if (prefab != null)
            {
                Debug.Log($"[AirplaneBuilder] Successfully saved prefab to {PrefabPath}");
            }

            // 6. Select root and mark scene dirty
            Selection.activeGameObject = root;
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            Debug.Log("[AirplaneBuilder] Interior build complete!");
        }

        [MenuItem("Tools/Airplane/Create WalkTest Scene")]
        public static void CreateWalkTestScene()
        {
            if (!Directory.Exists(SceneFolderPath))
            {
                Directory.CreateDirectory(SceneFolderPath);
                AssetDatabase.Refresh();
            }

            // Create new single empty scene
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // 1. Directional Light
            GameObject lightObj = new GameObject("Directional Light");
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(0.98f, 0.95f, 0.90f);
            light.intensity = 1.0f;
            lightObj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            RenderSettings.ambientLight = new Color(0.35f, 0.35f, 0.38f);

            // 2. Build Airplane Interior
            BuildInterior();

            // 3. Create Player with CharacterController & FirstPersonController
            GameObject player = new GameObject("Player");
            player.transform.position = new Vector3(0f, 0.05f, 0.0f); // Cockpit center floor

            CharacterController cc = player.AddComponent<CharacterController>();
            cc.radius = 0.3f;
            cc.height = 1.8f;
            cc.center = new Vector3(0f, 0.9f, 0f);
            cc.slopeLimit = 45f;
            cc.stepOffset = 0.3f;

            // Player Camera
            GameObject camObj = new GameObject("PlayerCamera");
            camObj.transform.SetParent(player.transform, false);
            camObj.transform.localPosition = new Vector3(0f, 1.65f, 0f); // Eye height

            Camera cam = camObj.AddComponent<Camera>();
            cam.nearClipPlane = 0.05f;
            cam.fieldOfView = 75f;
            camObj.AddComponent<AudioListener>();

            // FirstPersonController
            player.AddComponent<FirstPersonController>();

            // 4. Save Scene
            EditorSceneManager.SaveScene(scene, WalkTestScenePath);
            Debug.Log($"[AirplaneBuilder] Successfully created and saved WalkTest scene to {WalkTestScenePath}");
            EditorUtility.DisplayDialog("WalkTest Scene Ready", $"Created WalkTest scene at {WalkTestScenePath}.\n\nPress Play in Unity to walk through the airplane with WASD + Mouse, and press E / Left Click to open/close doors!", "OK");
        }
    }
}
