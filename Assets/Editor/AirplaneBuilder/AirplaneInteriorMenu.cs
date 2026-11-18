using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class AirplaneInteriorMenu
    {
        private const string RootName = "AirplaneInterior";
        private const string PrefabFolderPath = "Assets/Prefabs";
        private const string PrefabPath = "Assets/Prefabs/AirplaneInterior.prefab";

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
            GalleyBuilder.BuildFront(root.transform);
            LavatoryBuilder.BuildLav01(root.transform);
            CabinBuilder.Build(root.transform);
            LavatoryBuilder.BuildLav02(root.transform);
            LavatoryBuilder.BuildLav03(root.transform);
            GalleyBuilder.BuildRear(root.transform);
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
    }
}
