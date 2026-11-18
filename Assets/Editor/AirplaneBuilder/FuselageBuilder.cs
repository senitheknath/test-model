using System.Collections.Generic;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class FuselageBuilder
    {
        // 12-point interior arch profile (floor-left to floor-right)
        private static readonly Vector2[] ArchProfile = new Vector2[]
        {
            new Vector2(-2.05f, 0.00f), // 0: Floor left corner
            new Vector2(-2.10f, 0.55f), // 1: Lower wall
            new Vector2(-2.10f, 1.30f), // 2: Mid wall (window level)
            new Vector2(-1.95f, 1.85f), // 3: Upper wall / bin base
            new Vector2(-1.60f, 2.25f), // 4: Shoulder slope
            new Vector2(-0.90f, 2.52f), // 5: Ceiling slope
            new Vector2( 0.00f, 2.60f), // 6: Ceiling peak
            new Vector2( 0.90f, 2.52f), // 7: Right ceiling slope
            new Vector2( 1.60f, 2.25f), // 8: Right shoulder slope
            new Vector2( 1.95f, 1.85f), // 9: Right upper wall
            new Vector2( 2.10f, 1.30f), // 10: Right mid wall
            new Vector2( 2.10f, 0.55f), // 11: Right lower wall
            new Vector2( 2.05f, 0.00f)  // 12: Floor right corner
        };

        public static void Build(Transform parentRoot)
        {
            GameObject fuselageRoot = new GameObject("Fuselage");
            fuselageRoot.transform.SetParent(parentRoot, false);

            BuildShell(fuselageRoot.transform);
            BuildFloorAndAisle(fuselageRoot.transform);
            BuildCeilingLights(fuselageRoot.transform);
            BuildWindows(fuselageRoot.transform);
            BuildBulkheads(fuselageRoot.transform);
        }

        private static void BuildShell(Transform parent)
        {
            // Segmented along Z for crisp facets
            List<float> zSlices = new List<float>
            {
                InteriorConfig.ZCockpitFront,   // +1.4
                0.0f,
                InteriorConfig.ZCockpitRear,    // -1.4
                -3.15f,
                InteriorConfig.ZFrontGalleyRear, // -4.9
                InteriorConfig.ZFrontLavRear     // -6.5
            };

            // 20 cabin rows
            for (int i = 0; i <= InteriorConfig.CabinRowCount; i++)
            {
                zSlices.Add(InteriorConfig.ZFrontLavRear - i * InteriorConfig.CabinRowPitch);
            }

            zSlices.Add(InteriorConfig.ZRearLavRear);    // -25.1
            zSlices.Add(-26.85f);
            zSlices.Add(InteriorConfig.ZRearGalleyRear); // -28.6

            List<Vector3> mainVerts = new List<Vector3>();
            List<int> mainTris = new List<int>();
            List<Vector3> shadowVerts = new List<Vector3>();
            List<int> shadowTris = new List<int>();

            for (int s = 0; s < zSlices.Count - 1; s++)
            {
                float zA = zSlices[s];
                float zB = zSlices[s + 1];

                for (int i = 0; i < ArchProfile.Length - 1; i++)
                {
                    Vector3 p0 = new Vector3(ArchProfile[i].x, ArchProfile[i].y, zA);
                    Vector3 p1 = new Vector3(ArchProfile[i + 1].x, ArchProfile[i + 1].y, zA);
                    Vector3 p2 = new Vector3(ArchProfile[i + 1].x, ArchProfile[i + 1].y, zB);
                    Vector3 p3 = new Vector3(ArchProfile[i].x, ArchProfile[i].y, zB);

                    // Use shadow material on upper ceiling facets (indices 4, 5, 6, 7)
                    if (i == 4 || i == 5 || i == 6 || i == 7)
                        MeshUtil.AddQuad(shadowVerts, shadowTris, p0, p3, p2, p1);
                    else
                        MeshUtil.AddQuad(mainVerts, mainTris, p0, p3, p2, p1);
                }
            }

            Mesh mainMesh = new Mesh { name = "FuselageMain" };
            mainMesh.SetVertices(mainVerts);
            mainMesh.SetTriangles(mainTris, 0);
            mainMesh.RecalculateNormals();
            mainMesh.RecalculateBounds();
            MeshUtil.CreateMeshObject("Shell_Main", mainMesh, MaterialLibrary.Wall, parent);

            Mesh shadowMesh = new Mesh { name = "FuselageShadow" };
            shadowMesh.SetVertices(shadowVerts);
            shadowMesh.SetTriangles(shadowTris, 0);
            shadowMesh.RecalculateNormals();
            shadowMesh.RecalculateBounds();
            MeshUtil.CreateMeshObject("Shell_ShadowFacets", shadowMesh, MaterialLibrary.WallShadow, parent);

            // Wall outer colliders
            float totalLen = InteriorConfig.ZCockpitFront - InteriorConfig.ZRearGalleyRear;
            float midZ = (InteriorConfig.ZCockpitFront + InteriorConfig.ZRearGalleyRear) * 0.5f;

            // Left Wall Collider
            GameObject colL = new GameObject("Collider_Wall_L");
            colL.transform.SetParent(parent, false);
            BoxCollider boxL = colL.AddComponent<BoxCollider>();
            boxL.center = new Vector3(-InteriorConfig.FuselageHalfWidth, InteriorConfig.FuselageHeight * 0.5f, midZ);
            boxL.size = new Vector3(0.2f, InteriorConfig.FuselageHeight, totalLen);

            // Right Wall Collider
            GameObject colR = new GameObject("Collider_Wall_R");
            colR.transform.SetParent(parent, false);
            BoxCollider boxR = colR.AddComponent<BoxCollider>();
            boxR.center = new Vector3(InteriorConfig.FuselageHalfWidth, InteriorConfig.FuselageHeight * 0.5f, midZ);
            boxR.size = new Vector3(0.2f, InteriorConfig.FuselageHeight, totalLen);

            // Ceiling Collider
            GameObject colTop = new GameObject("Collider_Ceiling");
            colTop.transform.SetParent(parent, false);
            BoxCollider boxTop = colTop.AddComponent<BoxCollider>();
            boxTop.center = new Vector3(0f, InteriorConfig.FuselageHeight + 0.1f, midZ);
            boxTop.size = new Vector3(InteriorConfig.FuselageWidth, 0.2f, totalLen);
        }

        private static void BuildFloorAndAisle(Transform parent)
        {
            float totalLen = InteriorConfig.ZCockpitFront - InteriorConfig.ZRearGalleyRear;
            float midZ = (InteriorConfig.ZCockpitFront + InteriorConfig.ZRearGalleyRear) * 0.5f;

            // Main Floor Box
            GameObject floor = MeshUtil.CreateBoxObject("Floor",
                new Vector3(InteriorConfig.FuselageWidth, 0.1f, totalLen),
                MaterialLibrary.Floor, parent,
                new Vector3(0f, -0.05f, midZ), true);

            // Left Aisle Guide Line
            float aisleLen = (-InteriorConfig.ZCockpitRear) - (-InteriorConfig.ZRearGalleyRear) - 0.4f;
            float aisleMidZ = (InteriorConfig.ZCockpitRear + InteriorConfig.ZRearGalleyRear) * 0.5f;

            MeshUtil.CreateBoxObject("Aisle_Stripe_L",
                new Vector3(InteriorConfig.AisleStripeWidth, 0.002f, aisleLen),
                MaterialLibrary.AisleStripe, floor.transform,
                new Vector3(InteriorConfig.AisleStripeLeftX, 0.051f, aisleMidZ));

            // Right Aisle Guide Line
            MeshUtil.CreateBoxObject("Aisle_Stripe_R",
                new Vector3(InteriorConfig.AisleStripeWidth, 0.002f, aisleLen),
                MaterialLibrary.AisleStripe, floor.transform,
                new Vector3(InteriorConfig.AisleStripeRightX, 0.051f, aisleMidZ));
        }

        private static void BuildCeilingLights(Transform parent)
        {
            GameObject lightsRoot = new GameObject("CeilingLights");
            lightsRoot.transform.SetParent(parent, false);

            // Cabin continuous light strip
            float cabinLen = (-InteriorConfig.ZFrontLavRear) - (-InteriorConfig.ZCabinRear);
            float cabinMidZ = (InteriorConfig.ZFrontLavRear + InteriorConfig.ZCabinRear) * 0.5f;

            MeshUtil.CreateBoxObject("Cabin_Light_Strip",
                new Vector3(InteriorConfig.CeilingLightWidth, InteriorConfig.CeilingLightThickness, cabinLen),
                MaterialLibrary.CeilingLight, lightsRoot.transform,
                new Vector3(0f, InteriorConfig.CeilingLightY, cabinMidZ));

            // Galley square flush ceiling lights
            CreateGalleyCeilingLight(lightsRoot.transform, -3.15f, "Galley_Front_Light");
            CreateGalleyCeilingLight(lightsRoot.transform, -26.85f, "Galley_Rear_Light");
        }

        private static void CreateGalleyCeilingLight(Transform parent, float z, string name)
        {
            Vector3 size = new Vector3(InteriorConfig.GalleyLightSize, 0.02f, InteriorConfig.GalleyLightSize);
            MeshUtil.CreateBoxObject(name + "_C", size, MaterialLibrary.CeilingLight, parent, new Vector3(0f, 2.58f, z));
            MeshUtil.CreateBoxObject(name + "_L", size, MaterialLibrary.CeilingLight, parent, new Vector3(-0.9f, 2.45f, z));
            MeshUtil.CreateBoxObject(name + "_R", size, MaterialLibrary.CeilingLight, parent, new Vector3(0.9f, 2.45f, z));
        }

        private static void BuildWindows(Transform parent)
        {
            GameObject windowsRoot = new GameObject("Windows");
            windowsRoot.transform.SetParent(parent, false);

            for (int r = 0; r < InteriorConfig.CabinRowCount; r++)
            {
                float rowZ = InteriorConfig.ZFrontLavRear - (r + 0.5f) * InteriorConfig.CabinRowPitch;
                CreateWindowUnit(windowsRoot.transform, -2.08f, rowZ, true, r + 1);
                CreateWindowUnit(windowsRoot.transform,  2.08f, rowZ, false, r + 1);
            }
        }

        private static void CreateWindowUnit(Transform parent, float posX, float posZ, bool isLeft, int rowNum)
        {
            GameObject winObj = new GameObject($"Window_{(isLeft ? "L" : "R")}_{rowNum:D2}");
            winObj.transform.SetParent(parent, false);
            winObj.transform.localPosition = new Vector3(posX, InteriorConfig.WindowCenterY, posZ);

            float inwardOffset = isLeft ? 0.02f : -0.02f;
            float glassOffset = isLeft ? -0.02f : 0.02f;

            // Recessed Outer Chamfer Frame
            MeshUtil.CreateBoxObject("Frame_Recess",
                new Vector3(InteriorConfig.WindowDepth, InteriorConfig.WindowHeight, InteriorConfig.WindowWidth),
                MaterialLibrary.WallShadow, winObj.transform,
                new Vector3(inwardOffset, 0f, 0f));

            // Dark Glass Pane
            MeshUtil.CreateBoxObject("Glass_Pane",
                new Vector3(0.01f, InteriorConfig.WindowHeight - 0.06f, InteriorConfig.WindowWidth - 0.06f),
                MaterialLibrary.WindowGlass, winObj.transform,
                new Vector3(glassOffset, 0f, 0f));
        }

        private static void BuildBulkheads(Transform parent)
        {
            GameObject bulkheadsRoot = new GameObject("Bulkheads");
            bulkheadsRoot.transform.SetParent(parent, false);

            // Cockpit Rear Bulkhead (Z = -1.4)
            CreateDoorwayBulkhead(bulkheadsRoot.transform, InteriorConfig.ZCockpitRear, "Bulkhead_Cockpit");

            // Front Galley to Cabin Bulkhead (Z = -6.5)
            CreateDoorwayBulkhead(bulkheadsRoot.transform, InteriorConfig.ZFrontLavRear, "Bulkhead_Cabin_Front");

            // Cabin to Rear Lavatory Bulkhead (Z = -23.5)
            CreateDoorwayBulkhead(bulkheadsRoot.transform, InteriorConfig.ZCabinRear, "Bulkhead_Cabin_Rear");

            // Tail wall (Z = -28.6, completely closed panel)
            GameObject tail = MeshUtil.CreateBoxObject("TailWall",
                new Vector3(InteriorConfig.FuselageWidth, InteriorConfig.FuselageHeight, 0.1f),
                MaterialLibrary.Wall, bulkheadsRoot.transform,
                new Vector3(0f, InteriorConfig.FuselageHeight * 0.5f, InteriorConfig.ZRearGalleyRear - 0.05f), true);
        }

        private static void CreateDoorwayBulkhead(Transform parent, float z, string name)
        {
            GameObject bh = new GameObject(name);
            bh.transform.SetParent(parent, false);

            float sideWidth = (InteriorConfig.FuselageWidth - InteriorConfig.DoorwayWidth) * 0.5f;
            float sideCenterX = (InteriorConfig.DoorwayWidth + sideWidth) * 0.5f;
            float headerHeight = InteriorConfig.FuselageHeight - InteriorConfig.DoorwayHeight;
            float headerCenterY = InteriorConfig.DoorwayHeight + headerHeight * 0.5f;

            // Left panel with collider
            MeshUtil.CreateBoxObject("Panel_L",
                new Vector3(sideWidth, InteriorConfig.FuselageHeight, 0.08f),
                MaterialLibrary.Wall, bh.transform,
                new Vector3(-sideCenterX, InteriorConfig.FuselageHeight * 0.5f, z), true);

            // Right panel with collider
            MeshUtil.CreateBoxObject("Panel_R",
                new Vector3(sideWidth, InteriorConfig.FuselageHeight, 0.08f),
                MaterialLibrary.Wall, bh.transform,
                new Vector3(sideCenterX, InteriorConfig.FuselageHeight * 0.5f, z), true);

            // Header panel above doorway with collider
            MeshUtil.CreateBoxObject("Header",
                new Vector3(InteriorConfig.DoorwayWidth, headerHeight, 0.08f),
                MaterialLibrary.Wall, bh.transform,
                new Vector3(0f, headerCenterY, z), true);
        }
    }
}
