using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class ConnectivityCheck
    {
        private const float CapsuleRadius = 0.3f;
        private const float CapsuleHeight = 1.8f;
        private const int TriangleBudget = 150000;

        [MenuItem("Tools/Airplane/Validate")]
        public static void ValidateInterior()
        {
            GameObject root = GameObject.Find("AirplaneInterior");
            if (root == null)
            {
                Debug.LogError("[Validation] 'AirplaneInterior' root not found in the active scene! Please run 'Tools > Airplane > Build Interior' first.");
                EditorUtility.DisplayDialog("Validation Failed", "AirplaneInterior root not found in the current scene.", "OK");
                return;
            }

            Debug.Log("========== AIRPLANE INTERIOR VALIDATION ==========");

            // 1. Triangle Count Audit
            int totalTriangles = CountTriangles(root);
            bool triBudgetPassed = totalTriangles <= TriangleBudget;
            Debug.Log($"[Validation] Total Triangles: {totalTriangles:N0} / {TriangleBudget:N0} max budget -> {(triBudgetPassed ? "PASSED" : "FAILED")}");

            // 2. Shared Material Count Audit
            HashSet<Material> uniqueMaterials = CollectMaterials(root);
            Debug.Log($"[Validation] Unique Shared Materials: {uniqueMaterials.Count} (Budget target: ~12-16 materials)");

            // 3. Central Aisle Capsule Sweep (Cockpit to Tail)
            List<string> blockageLog = new List<string>();
            SweepAisle(root, blockageLog);

            // 4. Doorway Clearance Audits
            CheckDoorwayClearance("Doorway_Cockpit", new Vector3(0f, 0.9f, InteriorConfig.ZCockpitRear), blockageLog);
            CheckDoorwayClearance("Doorway_Cabin_Front", new Vector3(0f, 0.9f, InteriorConfig.ZFrontLavRear), blockageLog);
            CheckDoorwayClearance("Doorway_Cabin_Rear", new Vector3(0f, 0.9f, InteriorConfig.ZCabinRear), blockageLog);

            // 5. Final Report
            bool allPassed = triBudgetPassed && (blockageLog.Count == 0);
            if (allPassed)
            {
                string msg = $"Validation SUCCESSFUL!\n\n• Triangles: {totalTriangles:N0} (Budget: {TriangleBudget:N0})\n• Materials: {uniqueMaterials.Count}\n• Passage: 100% Unobstructed from Cockpit to Tail!";
                Debug.Log($"[Validation] {msg}");
                EditorUtility.DisplayDialog("Validation PASSED", msg, "Great!");
            }
            else
            {
                string msg = $"Validation WARNINGS/BLOCKAGES Detected:\n\n• Triangles: {totalTriangles:N0}\n• Materials: {uniqueMaterials.Count}\n• Blockages Found: {blockageLog.Count}\n\nCheck Console for details.";
                Debug.LogWarning($"[Validation] {msg}");
                foreach (string b in blockageLog)
                {
                    Debug.LogWarning($"[Validation Blockage] {b}");
                }
                EditorUtility.DisplayDialog("Validation Reported Blockages", msg, "Review");
            }
        }

        private static int CountTriangles(GameObject root)
        {
            int count = 0;
            MeshFilter[] filters = root.GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter mf in filters)
            {
                if (mf.sharedMesh != null)
                {
                    count += mf.sharedMesh.triangles.Length / 3;
                }
            }
            return count;
        }

        private static HashSet<Material> CollectMaterials(GameObject root)
        {
            HashSet<Material> mats = new HashSet<Material>();
            MeshRenderer[] renderers = root.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mr in renderers)
            {
                foreach (Material m in mr.sharedMaterials)
                {
                    if (m != null) mats.Add(m);
                }
            }
            return mats;
        }

        private static void SweepAisle(GameObject root, List<string> blockageLog)
        {
            float zStart = 0.5f;   // Inside Cockpit
            float zEnd = -27.5f;   // Near Tail Wall
            float step = 0.25f;

            float halfCyl = (CapsuleHeight - 2f * CapsuleRadius) * 0.5f;

            for (float z = zStart; z >= zEnd; z -= step)
            {
                Vector3 center = new Vector3(0f, CapsuleHeight * 0.5f + 0.05f, z);
                Vector3 point1 = center + Vector3.up * halfCyl;
                Vector3 point2 = center - Vector3.up * halfCyl;

                Collider[] overlaps = Physics.OverlapCapsule(point1, point2, CapsuleRadius);
                foreach (Collider c in overlaps)
                {
                    if (c.isTrigger) continue;
                    if (c.name.Contains("Floor")) continue;
                    if (c.transform.IsChildOf(root.transform))
                    {
                        // Check if it is a closed door panel or obstruction
                        if (c.GetComponentInParent<DoorInteractable>() != null)
                        {
                            // Door panel is expected if closed, but aisle itself should not have fixed wall colliders
                            continue;
                        }
                        blockageLog.Add($"Obstruction at Z={z:F2}m by '{c.gameObject.name}' (parent: '{c.transform.parent.name}')");
                    }
                }
            }
        }

        private static void CheckDoorwayClearance(string name, Vector3 doorwayCenter, List<string> blockageLog)
        {
            float halfCyl = (CapsuleHeight - 2f * CapsuleRadius) * 0.5f;
            Vector3 point1 = doorwayCenter + Vector3.up * halfCyl;
            Vector3 point2 = doorwayCenter - Vector3.up * halfCyl;

            Collider[] overlaps = Physics.OverlapCapsule(point1, point2, CapsuleRadius);
            foreach (Collider c in overlaps)
            {
                if (c.isTrigger) continue;
                if (c.name.Contains("Floor")) continue;
                if (c.GetComponentInParent<DoorInteractable>() != null) continue; // Door panel swings open

                // If a static wall collider blocks the doorway opening
                blockageLog.Add($"Doorway '{name}' obstructed by static collider: '{c.gameObject.name}'");
            }
        }
    }
}
