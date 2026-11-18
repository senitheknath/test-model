using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AirplaneBuilder
{
    public enum CabinSeatColor { Red, Blue, Indigo, Crew }

    public static class SeatMeshBuilder
    {
        private const string PrefabDir = "Assets/Prefabs";
        private static Mesh _passengerSeatMesh, _crewSeatMesh, _trayMesh, _armrestMesh;

        public static void InitializeMeshes()
        {
            if (_passengerSeatMesh == null) _passengerSeatMesh = BuildPassengerMesh();
            if (_crewSeatMesh == null) _crewSeatMesh = BuildCrewMesh();
            if (_trayMesh == null) _trayMesh = BuildTrayMesh();
            if (_armrestMesh == null) _armrestMesh = BuildArmrestMesh();
        }

        public static GameObject CreateSeat(string name, CabinSeatColor color, int seatIndex, bool isLeft, Transform parent)
        {
            InitializeMeshes();
            CabinMaterials.Initialize();

            GameObject seatObj = new GameObject(name);
            if (parent != null) seatObj.transform.SetParent(parent, false);

            bool isCrew = color == CabinSeatColor.Crew;
            seatObj.AddComponent<MeshFilter>().sharedMesh = isCrew ? _crewSeatMesh : _passengerSeatMesh;
            seatObj.AddComponent<MeshRenderer>().sharedMaterials = GetSeatMaterials(color);

            BoxCollider col = seatObj.AddComponent<BoxCollider>();
            col.center = isCrew ? new Vector3(0f, 0.38f, 0.02f) : new Vector3(0f, 0.59f, 0.02f);
            col.size = isCrew ? new Vector3(0.45f, 0.75f, 0.48f) : new Vector3(0.45f, 1.18f, 0.50f);

            seatObj.AddComponent<SeatInteractable>().SeatId = name;
            if (!isCrew) AttachTray(seatObj.transform);

            AttachArmrests(seatObj.transform, color, seatIndex, isLeft);
            return seatObj;
        }

        public static void BuildPrefabs()
        {
            InitializeMeshes();
            CabinMaterials.Initialize();
            if (!AssetDatabase.IsValidFolder(PrefabDir)) AssetDatabase.CreateFolder("Assets", "Prefabs");

            CreatePrefabVariant("Seat_Red", CabinSeatColor.Red);
            CreatePrefabVariant("Seat_Blue", CabinSeatColor.Blue);
            CreatePrefabVariant("Seat_Indigo", CabinSeatColor.Indigo);
            CreatePrefabVariant("Seat_Crew", CabinSeatColor.Crew);
            AssetDatabase.SaveAssets();
        }

        private static void CreatePrefabVariant(string prefabName, CabinSeatColor color)
        {
            GameObject temp = CreateSeat(prefabName, color, 1, true, null);
            PrefabUtility.SaveAsPrefabAsset(temp, $"{PrefabDir}/{prefabName}.prefab");
            Object.DestroyImmediate(temp);
        }

        private static Material[] GetSeatMaterials(CabinSeatColor color)
        {
            Material upholsteryMat = color switch {
                CabinSeatColor.Red => CabinMaterials.SeatRed,
                CabinSeatColor.Blue => CabinMaterials.SeatBlue,
                CabinSeatColor.Indigo => CabinMaterials.SeatIndigo,
                _ => CabinMaterials.CrewSeat
            };

            if (color == CabinSeatColor.Crew)
                return new Material[] { CabinMaterials.SeatFrame, upholsteryMat };

            return new Material[] { CabinMaterials.SeatFrame, upholsteryMat, CabinMaterials.TrayPanel, CabinMaterials.MeshNet };
        }

        private static void AttachTray(Transform seatRoot)
        {
            GameObject trayObj = new GameObject("Tray");
            trayObj.transform.SetParent(seatRoot, false);
            trayObj.transform.localPosition = new Vector3(0f, 0.86f, -0.235f);
            trayObj.transform.localRotation = Quaternion.Euler(-8f, 0f, 0f);

            trayObj.AddComponent<MeshFilter>().sharedMesh = _trayMesh;
            trayObj.AddComponent<MeshRenderer>().sharedMaterial = CabinMaterials.TrayPanel;

            BoxCollider trigger = trayObj.AddComponent<BoxCollider>();
            trigger.isTrigger = true;
            trigger.center = new Vector3(0f, -0.11f, 0.008f);
            trigger.size = new Vector3(0.36f, 0.22f, 0.02f);

            HingeInteractable hinge = trayObj.AddComponent<HingeInteractable>();
            hinge.SetAxis(Vector3.right);
            hinge.SetOpenAngle(-90f);
        }

        private static void AttachArmrests(Transform seatRoot, CabinSeatColor color, int seatIndex, bool isLeft)
        {
            Material trimMat = color switch {
                CabinSeatColor.Red => CabinMaterials.SeatRedDark,
                CabinSeatColor.Blue => CabinMaterials.SeatBlueDark,
                CabinSeatColor.Indigo => CabinMaterials.SeatIndigoDark,
                _ => CabinMaterials.CrewSeat
            };
            Material capMat = CabinMaterials.MeshNet;
            float dir = isLeft ? 1f : -1f;

            if (seatIndex == 1) // Window seat has 2 armrests
            {
                CreateArmrestInstance(seatRoot, new Vector3(-0.255f * dir, 0f, 0f), trimMat, capMat);
                CreateArmrestInstance(seatRoot, new Vector3(0.255f * dir, 0f, 0f), trimMat, capMat);
            }
            else // Middle (2) or Aisle (3) has 1 armrest
            {
                CreateArmrestInstance(seatRoot, new Vector3(0.255f * dir, 0f, 0f), trimMat, capMat);
            }
        }

        private static void CreateArmrestInstance(Transform parent, Vector3 localPos, Material trimMat, Material capMat)
        {
            GameObject arm = new GameObject("Armrest");
            arm.transform.SetParent(parent, false);
            arm.transform.localPosition = localPos;
            arm.AddComponent<MeshFilter>().sharedMesh = _armrestMesh;
            arm.AddComponent<MeshRenderer>().sharedMaterials = new Material[] { trimMat, capMat };
        }

        private static Mesh BuildPassengerMesh()
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> trisFrame = new List<int>(), trisUph = new List<int>(), trisTray = new List<int>(), trisPocket = new List<int>();

            // Frame & Legs (Submesh 0)
            AddBox(verts, trisFrame, new Vector3(-0.15f, 0.175f, 0.05f), new Vector3(0.04f, 0.35f, 0.04f));
            AddBox(verts, trisFrame, new Vector3( 0.15f, 0.175f, 0.05f), new Vector3(0.04f, 0.35f, 0.04f));
            AddBox(verts, trisFrame, new Vector3( 0.00f, 0.18f,  0.05f), new Vector3(0.34f, 0.03f, 0.03f));

            // Cushion, Backrest, Headrest (Submesh 1)
            AddBox(verts, trisUph, new Vector3(0f, 0.40f, 0.05f), new Vector3(0.45f, 0.10f, 0.46f));
            MeshUtil.AddQuad(verts, trisUph, new Vector3(-0.225f, 0.40f, 0.28f), new Vector3(0.225f, 0.40f, 0.28f),
                new Vector3(0.225f, 0.45f, 0.25f), new Vector3(-0.225f, 0.45f, 0.25f)); // Chamfer front

            Quaternion recline = Quaternion.Euler(-8f, 0f, 0f);
            AddRotatedBox(verts, trisUph, new Vector3(0f, 0.725f, -0.16f), new Vector3(0.45f, 0.55f, 0.11f), recline);
            AddRotatedBox(verts, trisUph, new Vector3(0f, 1.09f, -0.21f), new Vector3(0.32f, 0.18f, 0.13f), recline);

            // Tray Panel & Latch on rear face (Submesh 2)
            AddRotatedBox(verts, trisTray, new Vector3(0f, 0.75f, -0.225f), new Vector3(0.36f, 0.22f, 0.02f), recline);
            AddRotatedBox(verts, trisTray, new Vector3(0f, 0.85f, -0.235f), new Vector3(0.06f, 0.03f, 0.015f), recline);

            // Mesh Pocket & Grid Bars (Submesh 3)
            AddRotatedBox(verts, trisPocket, new Vector3(0f, 0.53f, -0.19f), new Vector3(0.36f, 0.14f, 0.05f), recline);
            for (int i = -1; i <= 1; i++)
                AddRotatedBox(verts, trisPocket, new Vector3(i * 0.08f, 0.53f, -0.218f), new Vector3(0.015f, 0.12f, 0.01f), recline);
            AddRotatedBox(verts, trisPocket, new Vector3(0f, 0.49f, -0.218f), new Vector3(0.32f, 0.015f, 0.01f), recline);
            AddRotatedBox(verts, trisPocket, new Vector3(0f, 0.57f, -0.218f), new Vector3(0.32f, 0.015f, 0.01f), recline);

            Mesh m = new Mesh { name = "Mesh_Seat_Passenger" };
            m.SetVertices(verts);
            m.subMeshCount = 4;
            m.SetTriangles(trisFrame, 0);
            m.SetTriangles(trisUph, 1);
            m.SetTriangles(trisTray, 2);
            m.SetTriangles(trisPocket, 3);
            m.RecalculateNormals();
            m.RecalculateBounds();
            return m;
        }

        private static Mesh BuildCrewMesh()
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> trisFrame = new List<int>(), trisUph = new List<int>();

            AddBox(verts, trisFrame, new Vector3(-0.15f, 0.175f, 0.05f), new Vector3(0.04f, 0.35f, 0.04f));
            AddBox(verts, trisFrame, new Vector3( 0.15f, 0.175f, 0.05f), new Vector3(0.04f, 0.35f, 0.04f));
            AddBox(verts, trisFrame, new Vector3( 0.00f, 0.18f,  0.05f), new Vector3(0.34f, 0.03f, 0.03f));

            AddBox(verts, trisUph, new Vector3(0f, 0.40f, 0.05f), new Vector3(0.45f, 0.10f, 0.46f));
            Quaternion recline = Quaternion.Euler(-8f, 0f, 0f);
            AddRotatedBox(verts, trisUph, new Vector3(0f, 0.60f, -0.15f), new Vector3(0.45f, 0.30f, 0.11f), recline);

            Mesh m = new Mesh { name = "Mesh_Seat_Crew" };
            m.SetVertices(verts);
            m.subMeshCount = 2;
            m.SetTriangles(trisFrame, 0);
            m.SetTriangles(trisUph, 1);
            m.RecalculateNormals();
            m.RecalculateBounds();
            return m;
        }

        private static Mesh BuildTrayMesh()
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            AddBox(verts, tris, new Vector3(0f, -0.11f, 0.008f), new Vector3(0.36f, 0.22f, 0.015f));

            Mesh m = new Mesh { name = "Mesh_TrayTable" };
            m.SetVertices(verts);
            m.SetTriangles(tris, 0);
            m.RecalculateNormals();
            m.RecalculateBounds();
            return m;
        }

        private static Mesh BuildArmrestMesh()
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> trisMain = new List<int>(), trisCap = new List<int>();

            AddBox(verts, trisMain, new Vector3(0f, 0.525f, 0.05f), new Vector3(0.04f, 0.15f, 0.04f));
            AddBox(verts, trisMain, new Vector3(0f, 0.63f, 0.025f), new Vector3(0.06f, 0.06f, 0.39f));
            AddBox(verts, trisCap, new Vector3(0f, 0.63f, 0.235f), new Vector3(0.06f, 0.06f, 0.03f));

            Mesh m = new Mesh { name = "Mesh_Armrest" };
            m.SetVertices(verts);
            m.subMeshCount = 2;
            m.SetTriangles(trisMain, 0);
            m.SetTriangles(trisCap, 1);
            m.RecalculateNormals();
            m.RecalculateBounds();
            return m;
        }

        private static void AddBox(List<Vector3> verts, List<int> tris, Vector3 center, Vector3 size) =>
            AddRotatedBox(verts, tris, center, size, Quaternion.identity);

        private static void AddRotatedBox(List<Vector3> verts, List<int> tris, Vector3 center, Vector3 size, Quaternion rot)
        {
            Vector3 h = size * 0.5f;
            Vector3 p000 = center + rot * new Vector3(-h.x, -h.y, -h.z), p100 = center + rot * new Vector3( h.x, -h.y, -h.z);
            Vector3 p110 = center + rot * new Vector3( h.x,  h.y, -h.z), p010 = center + rot * new Vector3(-h.x,  h.y, -h.z);
            Vector3 p001 = center + rot * new Vector3(-h.x, -h.y,  h.z), p101 = center + rot * new Vector3( h.x, -h.y,  h.z);
            Vector3 p111 = center + rot * new Vector3( h.x,  h.y,  h.z), p011 = center + rot * new Vector3(-h.x,  h.y,  h.z);

            MeshUtil.AddQuad(verts, tris, p001, p101, p111, p011); // Front (+Z)
            MeshUtil.AddQuad(verts, tris, p100, p000, p010, p110); // Back (-Z)
            MeshUtil.AddQuad(verts, tris, p000, p001, p011, p010); // Left (-X)
            MeshUtil.AddQuad(verts, tris, p101, p100, p110, p111); // Right (+X)
            MeshUtil.AddQuad(verts, tris, p011, p111, p110, p010); // Top (+Y)
            MeshUtil.AddQuad(verts, tris, p000, p100, p101, p001); // Bottom (-Y)
        }
    }
}
