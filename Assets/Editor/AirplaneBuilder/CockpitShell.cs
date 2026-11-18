using System.Collections.Generic;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class CockpitShell
    {
        public static void Build(Transform shellParent, Transform windshieldParent)
        {
            BuildFloor(shellParent);
            BuildFacetedShell(shellParent);
            BuildWindshield(windshieldParent);
            BuildSideWindows(windshieldParent);
            BuildNightBackdrop(windshieldParent);
        }

        private static void BuildFloor(Transform parent)
        {
            Vector3 floorSize = new Vector3(InteriorConfig.CockpitWidth, 0.05f, InteriorConfig.CockpitLength);
            Vector3 floorPos = new Vector3(0f, -0.025f, 0f);
            MeshUtil.CreateBoxObject("Floor", floorSize, CockpitMaterials.Floor, parent, floorPos, true);
        }

        private static void BuildFacetedShell(Transform parent)
        {
            GameObject shellGo = new GameObject("FacetedShell");
            shellGo.transform.SetParent(parent, false);

            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();

            float zFront = InteriorConfig.ZCockpitFront;
            float zRear = InteriorConfig.ZCockpitRear;
            float halfW = InteriorConfig.CockpitHalfWidth;

            // Profile nodes per side: (X, Y)
            Vector2[] profileRight = new Vector2[]
            {
                new Vector2(halfW, 0f),
                new Vector2(halfW, 1.10f),
                new Vector2(1.08f, 1.55f),
                new Vector2(0.80f, 1.98f),
                new Vector2(InteriorConfig.CockpitCeilingWidth * 0.5f, InteriorConfig.CockpitHeight)
            };

            // Build Right wall & facets (double-sided)
            for (int i = 0; i < profileRight.Length - 1; i++)
            {
                Vector2 p0 = profileRight[i];
                Vector2 p1 = profileRight[i + 1];

                Vector3 v0 = new Vector3(p0.x, p0.y, zFront);
                Vector3 v1 = new Vector3(p0.x, p0.y, zRear);
                Vector3 v2 = new Vector3(p1.x, p1.y, zRear);
                Vector3 v3 = new Vector3(p1.x, p1.y, zFront);

                MeshUtil.AddDoubleSidedQuad(verts, tris, v0, v1, v2, v3);
            }

            // Build Left wall & facets (mirrored, double-sided)
            for (int i = 0; i < profileRight.Length - 1; i++)
            {
                Vector2 p0 = profileRight[i];
                Vector2 p1 = profileRight[i + 1];

                Vector3 v0 = new Vector3(-p0.x, p0.y, zFront);
                Vector3 v1 = new Vector3(-p1.x, p1.y, zFront);
                Vector3 v2 = new Vector3(-p1.x, p1.y, zRear);
                Vector3 v3 = new Vector3(-p0.x, p0.y, zRear);

                MeshUtil.AddDoubleSidedQuad(verts, tris, v0, v1, v2, v3);
            }

            // Flat central ceiling strip (double-sided)
            float ceilHalfW = InteriorConfig.CockpitCeilingWidth * 0.5f;
            float ceilY = InteriorConfig.CockpitHeight;
            Vector3 c0 = new Vector3(-ceilHalfW, ceilY, zFront);
            Vector3 c1 = new Vector3( ceilHalfW, ceilY, zFront);
            Vector3 c2 = new Vector3( ceilHalfW, ceilY, zRear);
            Vector3 c3 = new Vector3(-ceilHalfW, ceilY, zRear);
            MeshUtil.AddDoubleSidedQuad(verts, tris, c0, c1, c2, c3);

            Mesh mesh = new Mesh { name = "CockpitShellMesh" };
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            MeshFilter mf = shellGo.AddComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            MeshRenderer mr = shellGo.AddComponent<MeshRenderer>();
            mr.sharedMaterial = CockpitMaterials.ShellLit;
        }

        private static void BuildWindshield(Transform parent)
        {
            GameObject wsGo = new GameObject("WindshieldPanes");
            wsGo.transform.SetParent(parent, false);

            float sillY = InteriorConfig.WindshieldSillY;
            float topY = InteriorConfig.WindshieldTopY;
            float centerHW = InteriorConfig.WindshieldCenterHalfWidth;
            float centerZ = 1.35f;

            float topZOffset = -0.20f;
            float cornerX = 1.10f;
            float cornerZ = 1.00f;

            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();

            // Glass Panes (Double-sided)
            Vector3 cp_bl = new Vector3(-centerHW, sillY, centerZ);
            Vector3 cp_br = new Vector3( centerHW, sillY, centerZ);
            Vector3 cp_tr = new Vector3( centerHW, topY, centerZ + topZOffset);
            Vector3 cp_tl = new Vector3(-centerHW, topY, centerZ + topZOffset);
            MeshUtil.AddDoubleSidedQuad(verts, tris, cp_bl, cp_br, cp_tr, cp_tl);

            Vector3 rp_bl = cp_br;
            Vector3 rp_br = new Vector3(cornerX, sillY, cornerZ);
            Vector3 rp_tr = new Vector3(cornerX, topY, cornerZ + topZOffset);
            Vector3 rp_tl = cp_tr;
            MeshUtil.AddDoubleSidedQuad(verts, tris, rp_bl, rp_br, rp_tr, rp_tl);

            Vector3 lp_bl = new Vector3(-cornerX, sillY, cornerZ);
            Vector3 lp_br = cp_bl;
            Vector3 lp_tr = cp_tl;
            Vector3 lp_tl = new Vector3(-cornerX, topY, cornerZ + topZOffset);
            MeshUtil.AddDoubleSidedQuad(verts, tris, lp_bl, lp_br, lp_tr, lp_tl);

            // Lower Forward Bulkhead / Nose Wall below windshield (Y=0 to sillY)
            Vector3 n_c0 = new Vector3(-centerHW, 0f, centerZ);
            Vector3 n_c1 = new Vector3( centerHW, 0f, centerZ);
            MeshUtil.AddDoubleSidedQuad(verts, tris, n_c0, n_c1, cp_br, cp_bl);

            Vector3 n_r1 = new Vector3(cornerX, 0f, cornerZ);
            MeshUtil.AddDoubleSidedQuad(verts, tris, n_c1, n_r1, rp_br, cp_br);

            Vector3 n_l0 = new Vector3(-cornerX, 0f, cornerZ);
            MeshUtil.AddDoubleSidedQuad(verts, tris, n_l0, n_c0, cp_bl, lp_bl);

            Mesh mesh = new Mesh { name = "WindshieldMesh" };
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            MeshFilter mf = wsGo.AddComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            MeshRenderer mr = wsGo.AddComponent<MeshRenderer>();
            mr.sharedMaterial = CockpitMaterials.NightSky;

            // Structural pillars (0.10m thick)
            BuildPillars(parent, cp_bl, cp_br, cp_tr, cp_tl, rp_br, rp_tr, lp_bl, lp_tl);
        }

        private static void BuildPillars(Transform parent, Vector3 cbl, Vector3 cbr, Vector3 ctr, Vector3 ctl, Vector3 rbr, Vector3 rtr, Vector3 lbl, Vector3 ltl)
        {
            GameObject pilGo = new GameObject("Pillars");
            pilGo.transform.SetParent(parent, false);

            float pW = InteriorConfig.WindshieldPillarWidth;

            Vector3 cMidBot = (cbl + cbr) * 0.5f;
            Vector3 cMidTop = (ctl + ctr) * 0.5f;
            CreateBeamBetween(pilGo.transform, cMidBot, cMidTop, pW, CockpitMaterials.ShellShadow);

            CreateBeamBetween(pilGo.transform, cbr, ctr, pW, CockpitMaterials.ShellLit);
            CreateBeamBetween(pilGo.transform, cbl, ctl, pW, CockpitMaterials.ShellLit);
            CreateBeamBetween(pilGo.transform, rbr, rtr, pW, CockpitMaterials.ShellLit);
            CreateBeamBetween(pilGo.transform, lbl, ltl, pW, CockpitMaterials.ShellLit);

            CreateBeamBetween(pilGo.transform, ltl, ctl, pW, CockpitMaterials.ShellLit);
            CreateBeamBetween(pilGo.transform, ctl, ctr, pW, CockpitMaterials.ShellLit);
            CreateBeamBetween(pilGo.transform, ctr, rtr, pW, CockpitMaterials.ShellLit);

            Vector3 compassPos = cMidTop + new Vector3(0f, -0.08f, -0.06f);
            Mesh compassMesh = MeshUtil.CreatePrism(8, InteriorConfig.StandbyCompassSize * 0.5f, InteriorConfig.StandbyCompassSize);
            MeshUtil.CreateMeshObject("StandbyCompass", compassMesh, CockpitMaterials.Panel, pilGo.transform);
            pilGo.transform.Find("StandbyCompass").localPosition = compassPos;
        }

        private static void CreateBeamBetween(Transform parent, Vector3 a, Vector3 b, float thickness, Material mat)
        {
            Vector3 dir = b - a;
            float len = dir.magnitude;
            if (len < 0.01f) return;

            GameObject beam = new GameObject("PillarBeam");
            beam.transform.SetParent(parent, false);
            beam.transform.localPosition = (a + b) * 0.5f;
            beam.transform.localRotation = Quaternion.LookRotation(dir, Vector3.up);

            MeshFilter mf = beam.AddComponent<MeshFilter>();
            mf.sharedMesh = MeshUtil.CreateBox(new Vector3(thickness, thickness, len));
            MeshRenderer mr = beam.AddComponent<MeshRenderer>();
            mr.sharedMaterial = mat;
        }

        private static void BuildSideWindows(Transform parent)
        {
            GameObject swGo = new GameObject("SideWindows");
            swGo.transform.SetParent(parent, false);

            float sideX = InteriorConfig.SideWindowX;
            float z0 = InteriorConfig.SideWindowZStart;
            float z1 = InteriorConfig.SideWindowZEnd;
            float y0 = InteriorConfig.SideWindowYBottom;
            float y1 = InteriorConfig.SideWindowYTop;

            for (int side = -1; side <= 1; side += 2)
            {
                float x = sideX * side;
                string sideName = side > 0 ? "SideWindow_R" : "SideWindow_L";
                GameObject win = new GameObject(sideName);
                win.transform.SetParent(swGo.transform, false);

                Vector3 p0 = new Vector3(x, y0, z1);
                Vector3 p1 = new Vector3(x, y0, z0);
                Vector3 p2 = new Vector3(x, y1, z0);
                Vector3 p3 = new Vector3(x, y1, z1);

                Mesh glassMesh = MeshUtil.CreateQuad(p0, p1, p2, p3, true);
                MeshUtil.CreateMeshObject("Glass", glassMesh, CockpitMaterials.NightSky, win.transform);

                float midZ = (z0 + z1) * 0.5f;
                Vector3 barSize = new Vector3(0.04f, y1 - y0, 0.05f);
                Vector3 barPos = new Vector3(x - 0.01f * side, (y0 + y1) * 0.5f, midZ);
                MeshUtil.CreateBoxObject("DividerBar", barSize, CockpitMaterials.ShellLit, win.transform, barPos);

                Vector3 sillSize = new Vector3(0.08f, 0.04f, z1 - z0 + 0.1f);
                Vector3 sillPos = new Vector3(x - 0.03f * side, y0 - 0.02f, midZ);
                MeshUtil.CreateBoxObject("WindowSill", sillSize, CockpitMaterials.ShellShadow, win.transform, sillPos);
            }
        }

        private static void BuildNightBackdrop(Transform parent)
        {
            GameObject backdropGo = new GameObject("NightBackdrop");
            backdropGo.transform.SetParent(parent, false);

            float sillY = InteriorConfig.WindshieldSillY;
            float topY = InteriorConfig.WindshieldTopY + 0.6f;
            float zBack = 1.70f;
            float wBack = 2.6f;

            // Large curved night dome backdrop
            Vector3 b0 = new Vector3(-wBack, 0.2f, zBack);
            Vector3 b1 = new Vector3( wBack, 0.2f, zBack);
            Vector3 b2 = new Vector3( wBack, topY, zBack - 0.35f);
            Vector3 b3 = new Vector3(-wBack, topY, zBack - 0.35f);
            Mesh backMesh = MeshUtil.CreateQuad(b0, b1, b2, b3, true);
            MeshUtil.CreateMeshObject("SkyDome", backMesh, CockpitMaterials.NightSky, backdropGo.transform);

            // Horizon haze band at sill height
            Vector3 h0 = new Vector3(-wBack, sillY - 0.14f, zBack - 0.05f);
            Vector3 h1 = new Vector3( wBack, sillY - 0.14f, zBack - 0.05f);
            Vector3 h2 = new Vector3( wBack, sillY + 0.16f, zBack - 0.05f);
            Vector3 h3 = new Vector3(-wBack, sillY + 0.16f, zBack - 0.05f);
            Mesh hazeMesh = MeshUtil.CreateQuad(h0, h1, h2, h3, true);
            MeshUtil.CreateMeshObject("HorizonHaze", hazeMesh, CockpitMaterials.HorizonHaze, backdropGo.transform);

            // 25 tiny emissive cubes (0.02m) as distant runway/city lights near horizon
            GameObject lightsGo = new GameObject("DistantLights");
            lightsGo.transform.SetParent(backdropGo.transform, false);

            float[] xOffsets = new float[] { -0.7f, -0.55f, -0.4f, -0.25f, -0.15f, -0.05f, 0.05f, 0.12f, 0.25f, 0.42f, 0.6f, 0.75f, -0.32f, -0.08f, 0.18f, 0.35f, -0.65f, 0.52f, -0.18f, 0.02f, 0.08f, -0.48f, 0.28f, -0.12f, 0.22f };
            float[] yDeltas = new float[] { 0.02f, -0.01f, 0.03f, 0.00f, 0.01f, 0.04f, 0.01f, -0.02f, 0.02f, 0.03f, -0.01f, 0.02f, 0.05f, 0.01f, 0.03f, -0.01f, 0.02f, 0.01f, 0.04f, 0.00f, 0.02f, -0.02f, 0.01f, 0.03f, -0.01f };

            for (int i = 0; i < xOffsets.Length; i++)
            {
                Material lightMat;
                if (i % 5 == 0) lightMat = CockpitMaterials.HorizonOrange;
                else if (i % 5 == 1) lightMat = CockpitMaterials.HorizonBlue;
                else if (i % 5 == 2) lightMat = CockpitMaterials.EmissiveGreen;
                else lightMat = CockpitMaterials.DialFace;

                Vector3 pos = new Vector3(xOffsets[i], sillY + yDeltas[i], zBack - 0.08f);
                MeshUtil.CreateBoxObject($"Light_{i}", Vector3.one * 0.02f, lightMat, lightsGo.transform, pos);
            }
        }
    }
}
