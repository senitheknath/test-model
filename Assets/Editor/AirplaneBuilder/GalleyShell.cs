using System.Collections.Generic;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class GalleyShell
    {
        public static void Build(Transform parent, GalleyType type, float length)
        {
            BuildFloor(parent, length);
            BuildLiners(parent, length);
            BuildCeilingArch(parent, length);
            BuildCeilingLights(parent, length);

            if (type == GalleyType.Front)
            {
                BuildCockpitSkin(parent);
            }
        }

        private static void BuildFloor(Transform parent, float length)
        {
            Vector3 floorSize = new Vector3(InteriorConfig.FuselageWidth, 0.05f, length);
            Vector3 floorPos = new Vector3(0f, -0.025f, -length * 0.5f);
            MeshUtil.CreateBoxObject("GalleyFloor", floorSize, GalleyMaterials.Floor, parent, floorPos, true);
        }

        private static void BuildLiners(Transform parent, float length)
        {
            GameObject linersGo = new GameObject("SideLiners");
            linersGo.transform.SetParent(parent, false);

            float thick = InteriorConfig.GalleyLinerThickness; // 0.10m
            float h = InteriorConfig.GalleyLinerHeight;        // 1.95m
            float xLiner = InteriorConfig.GalleyLinerX;        // 2.00m

            // Left wall liner
            Vector3 lPos = new Vector3(-xLiner - thick * 0.5f, h * 0.5f, -length * 0.5f);
            MeshUtil.CreateBoxObject("Liner_L", new Vector3(thick, h, length), GalleyMaterials.WallLit, linersGo.transform, lPos, true);

            // Right wall liner
            Vector3 rPos = new Vector3(xLiner + thick * 0.5f, h * 0.5f, -length * 0.5f);
            MeshUtil.CreateBoxObject("Liner_R", new Vector3(thick, h, length), GalleyMaterials.WallLit, linersGo.transform, rPos, true);
        }

        private static void BuildCeilingArch(Transform parent, float length)
        {
            GameObject archGo = new GameObject("CeilingArch");
            archGo.transform.SetParent(parent, false);

            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();

            float z0 = 0f;
            float z1 = -length;
            float xL = InteriorConfig.GalleyLinerX; // 2.00m
            float yL = InteriorConfig.GalleyLinerHeight; // 1.95m

            // Angled facets rising to top ceiling at Y=2.60m, X=0.60m
            Vector2[] profileR = new Vector2[]
            {
                new Vector2(xL, yL),
                new Vector2(1.55f, 2.30f),
                new Vector2(0.85f, 2.52f),
                new Vector2(0.45f, 2.60f)
            };

            // Right arch facets
            for (int i = 0; i < profileR.Length - 1; i++)
            {
                Vector2 p0 = profileR[i];
                Vector2 p1 = profileR[i + 1];

                Vector3 v0 = new Vector3(p0.x, p0.y, z0);
                Vector3 v1 = new Vector3(p0.x, p0.y, z1);
                Vector3 v2 = new Vector3(p1.x, p1.y, z1);
                Vector3 v3 = new Vector3(p1.x, p1.y, z0);
                MeshUtil.AddDoubleSidedQuad(verts, tris, v0, v1, v2, v3);
            }

            // Left arch facets (mirrored)
            for (int i = 0; i < profileR.Length - 1; i++)
            {
                Vector2 p0 = profileR[i];
                Vector2 p1 = profileRightL(profileR[i]);
                Vector2 q0 = profileR[i + 1];
                Vector2 q1 = profileRightL(profileR[i + 1]);

                Vector3 v0 = new Vector3(-p0.x, p0.y, z0);
                Vector3 v1 = new Vector3(-q0.x, q0.y, z0);
                Vector3 v2 = new Vector3(-q0.x, q0.y, z1);
                Vector3 v3 = new Vector3(-p0.x, p0.y, z1);
                MeshUtil.AddDoubleSidedQuad(verts, tris, v0, v1, v2, v3);
            }

            // Top flat central ceiling strip
            Vector3 c0 = new Vector3(-0.45f, 2.60f, z0);
            Vector3 c1 = new Vector3( 0.45f, 2.60f, z0);
            Vector3 c2 = new Vector3( 0.45f, 2.60f, z1);
            Vector3 c3 = new Vector3(-0.45f, 2.60f, z1);
            MeshUtil.AddDoubleSidedQuad(verts, tris, c0, c1, c2, c3);

            Mesh mesh = new Mesh { name = "GalleyCeilingMesh" };
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            MeshFilter mf = archGo.AddComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            MeshRenderer mr = archGo.AddComponent<MeshRenderer>();
            mr.sharedMaterial = GalleyMaterials.CeilingFacet;
        }

        private static Vector2 profileRightL(Vector2 v) => v;

        private static void BuildCockpitSkin(Transform parent)
        {
            GameObject skinGo = new GameObject("CockpitRearSkin");
            skinGo.transform.SetParent(parent, false);

            float skinThick = InteriorConfig.GalleySkinThickness; // 0.25m
            float doorW = InteriorConfig.GalleyDoorWidth;         // 0.75m
            float doorH = InteriorConfig.GalleyDoorHeight;        // 2.00m
            float totalW = InteriorConfig.FuselageWidth;          // 4.20m
            float totalH = InteriorConfig.FuselageHeight;         // 2.60m

            // Left skin wall partition
            float sideW = (totalW - doorW) * 0.5f;
            Vector3 lPos = new Vector3(-totalW * 0.5f + sideW * 0.5f, totalH * 0.5f, skinThick * 0.5f);
            MeshUtil.CreateBoxObject("Skin_Left", new Vector3(sideW, totalH, skinThick), GalleyMaterials.WallLit, skinGo.transform, lPos, true);

            // Right skin wall partition
            Vector3 rPos = new Vector3(totalW * 0.5f - sideW * 0.5f, totalH * 0.5f, skinThick * 0.5f);
            MeshUtil.CreateBoxObject("Skin_Right", new Vector3(sideW, totalH, skinThick), GalleyMaterials.WallLit, skinGo.transform, rPos, true);

            // Header skin partition above doorway
            float topH = totalH - doorH;
            Vector3 topPos = new Vector3(0f, doorH + topH * 0.5f, skinThick * 0.5f);
            MeshUtil.CreateBoxObject("Skin_Header", new Vector3(doorW, topH, skinThick), GalleyMaterials.WallLit, skinGo.transform, topPos, true);

            // Chamfered doorway trim frame
            MeshUtil.CreateBoxObject("Skin_Trim_L", new Vector3(0.05f, doorH, skinThick + 0.02f), GalleyMaterials.WallTrim, skinGo.transform, new Vector3(-doorW * 0.5f - 0.025f, doorH * 0.5f, skinThick * 0.5f));
            MeshUtil.CreateBoxObject("Skin_Trim_R", new Vector3(0.05f, doorH, skinThick + 0.02f), GalleyMaterials.WallTrim, skinGo.transform, new Vector3( doorW * 0.5f + 0.025f, doorH * 0.5f, skinThick * 0.5f));
            MeshUtil.CreateBoxObject("Skin_Trim_Top", new Vector3(doorW + 0.1f, 0.05f, skinThick + 0.02f), GalleyMaterials.WallTrim, skinGo.transform, new Vector3(0f, doorH + 0.025f, skinThick * 0.5f));
        }

        private static void BuildCeilingLights(Transform parent, float length)
        {
            GameObject lightsGo = new GameObject("CeilingLights");
            lightsGo.transform.SetParent(parent, false);

            float[] gPositions = new float[] { 0.6f, 1.7f, 2.8f };
            for (int i = 0; i < gPositions.Length; i++)
            {
                if (gPositions[i] > length) continue;

                Vector3 pos = new Vector3(0f, 2.58f, -gPositions[i]);
                MeshUtil.CreateBoxObject($"LightPlate_{i + 1}", new Vector3(0.14f, 0.02f, 0.14f), GalleyMaterials.CeilingLight, lightsGo.transform, pos);

                GameObject lightObj = new GameObject($"PointLight_{i + 1}");
                lightObj.transform.SetParent(lightsGo.transform, false);
                lightObj.transform.localPosition = pos + Vector3.down * 0.08f;
                Light l = lightObj.AddComponent<Light>();
                l.type = LightType.Point;
                l.color = new Color(1.0f, 0.88f, 0.66f);
                l.range = 3.2f;
                l.intensity = 0.75f;
            }

            // 2 Angled side lamps on sloped ceiling at X = +/- 1.30m, g = 1.7m
            for (int side = -1; side <= 1; side += 2)
            {
                Vector3 sPos = new Vector3(1.30f * side, 2.38f, -1.7f);
                GameObject lamp = MeshUtil.CreateBoxObject($"SlopeLamp_{side}", new Vector3(0.12f, 0.02f, 0.12f), GalleyMaterials.CeilingLight, lightsGo.transform, sPos);
                lamp.transform.localRotation = Quaternion.Euler(0f, 0f, -25f * side);
            }
        }
    }
}
