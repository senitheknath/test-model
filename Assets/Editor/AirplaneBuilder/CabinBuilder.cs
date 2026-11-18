using UnityEngine;

namespace AirplaneBuilder
{
    public static class CabinBuilder
    {
        public static void Build(Transform parentRoot)
        {
            CabinMaterials.Initialize();

            // Destroy existing Cabin if rebuilding (idempotent)
            Transform existing = parentRoot.Find("Cabin");
            if (existing != null)
            {
                Object.DestroyImmediate(existing.gameObject);
            }

            GameObject cabinRoot = new GameObject("Cabin");
            cabinRoot.transform.SetParent(parentRoot, false);

            // Create standard hierarchy containers
            GameObject floorGroup = new GameObject("Floor");
            floorGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject linersGroup = new GameObject("Liners");
            linersGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject ceilingGroup = new GameObject("Ceiling");
            ceilingGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject windowsGroup = new GameObject("Windows");
            windowsGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject binsGroup = new GameObject("Bins");
            binsGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject rowsGroup = new GameObject("Rows");
            rowsGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject midBulkheadGroup = new GameObject("MidBulkhead");
            midBulkheadGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject aftModuleGroup = new GameObject("AftModule");
            aftModuleGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject passengersGroup = new GameObject("Passengers");
            passengersGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject signsGroup = new GameObject("Signs");
            signsGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject lightsGroup = new GameObject("Lights");
            lightsGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject camerasGroup = new GameObject("ViewCameras");
            camerasGroup.transform.SetParent(cabinRoot.transform, false);

            // Build seat prefabs
            SeatMeshBuilder.BuildPrefabs();

            // Build all 17 passenger rows (102 seats: Rows 1-4 Red, 5-10 Blue, 11-17 Indigo)
            SeatRowBuilder.BuildAllRows(rowsGroup.transform);
        }
    }
}
