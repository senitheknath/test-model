using System.Collections.Generic;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class CockpitPanels
    {
        public static void BuildGlareshield(Transform parent)
        {
            GameObject gsGo = new GameObject("GlareshieldSlab");
            gsGo.transform.SetParent(parent, false);

            float w = InteriorConfig.GlareshieldHalfWidth * 2f; // 2.30m
            float z0 = InteriorConfig.GlareshieldZStart;        // 0.75m
            float z1 = InteriorConfig.GlareshieldZEnd;          // 1.20m
            float depth = z1 - z0;
            float y = InteriorConfig.GlareshieldY;              // 1.30m
            float thick = InteriorConfig.GlareshieldThickness;  // 0.07m

            // Main Glareshield brow slab
            Vector3 slabSize = new Vector3(w, thick, depth);
            Vector3 slabPos = new Vector3(0f, y, (z0 + z1) * 0.5f);
            MeshUtil.CreateBoxObject("BrowSlab", slabSize, CockpitMaterials.Dark, gsGo.transform, slabPos);

            // Raised Autopilot MCP Strip
            float apW = InteriorConfig.AutopilotHalfWidth * 2f; // 0.90m
            float apH = InteriorConfig.AutopilotYTop - InteriorConfig.AutopilotYBottom; // 0.08m
            float apDepth = InteriorConfig.AutopilotZEnd - InteriorConfig.AutopilotZStart; // 0.10m
            Vector3 apSize = new Vector3(apW, apH, apDepth);
            Vector3 apPos = new Vector3(0f, InteriorConfig.AutopilotYBottom + apH * 0.5f, (InteriorConfig.AutopilotZStart + InteriorConfig.AutopilotZEnd) * 0.5f);
            MeshUtil.CreateBoxObject("AutopilotStrip", apSize, CockpitMaterials.Panel, gsGo.transform, apPos);

            // 2 rows of button cubes & rotary knobs on Autopilot MCP
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    float bx = -apW * 0.5f + 0.06f + c * 0.087f;
                    float by = apPos.y - 0.02f + r * 0.035f;
                    Vector3 bPos = new Vector3(bx, by, apPos.z - apDepth * 0.5f - 0.012f);
                    Material bMat = (c % 3 == 0) ? CockpitMaterials.HorizonOrange : CockpitMaterials.DialFace;
                    MeshUtil.CreateBoxObject($"MCP_Btn_{r}_{c}", Vector3.one * 0.022f, bMat, gsGo.transform, bPos);
                }
            }

            // Master Warning & Caution ears on glareshield outer ends
            float earX = InteriorConfig.GlareshieldHalfWidth - 0.15f;
            for (int side = -1; side <= 1; side += 2)
            {
                Vector3 warnPos = new Vector3(earX * side, y + thick * 0.5f + 0.02f, z0 + 0.08f);
                MeshUtil.CreateBoxObject($"MasterWarn_{side}", new Vector3(0.04f, 0.03f, 0.03f), MaterialLibrary.IndicatorRed, gsGo.transform, warnPos);

                Vector3 cautPos = new Vector3((earX - 0.06f * side), y + thick * 0.5f + 0.02f, z0 + 0.08f);
                MeshUtil.CreateBoxObject($"MasterCaut_{side}", new Vector3(0.04f, 0.03f, 0.03f), CockpitMaterials.HorizonOrange, gsGo.transform, cautPos);
            }
        }

        public static void BuildMainPanel(Transform parent)
        {
            GameObject mpGo = new GameObject("MainDashboard");
            mpGo.transform.SetParent(parent, false);

            float w = InteriorConfig.MainPanelHalfWidth * 2f; // 2.20m
            float yBot = InteriorConfig.MainPanelYBottom;      // 0.72m
            float yTop = InteriorConfig.MainPanelYTop;          // 1.30m
            float h = yTop - yBot;                             // 0.58m
            float z = InteriorConfig.MainPanelZ;               // 0.78m
            float thick = InteriorConfig.MainPanelThickness;   // 0.12m

            // Main Instrument Panel body
            GameObject panelBody = new GameObject("PanelBody");
            panelBody.transform.SetParent(mpGo.transform, false);
            panelBody.transform.localPosition = new Vector3(0f, yBot + h * 0.5f, z);
            panelBody.transform.localRotation = Quaternion.Euler(-InteriorConfig.MainPanelTiltDeg, 0f, 0f);

            MeshUtil.CreateBoxObject("PanelFace", new Vector3(w, h, thick), CockpitMaterials.Panel, panelBody.transform, Vector3.zero);

            // Lower knee panel / bulkhead down to floor
            Vector3 kneeSize = new Vector3(w, yBot, 0.08f);
            Vector3 kneePos = new Vector3(0f, yBot * 0.5f, z + 0.04f);
            MeshUtil.CreateBoxObject("KneePanel", kneeSize, CockpitMaterials.ShellShadow, mpGo.transform, kneePos);

            // Populate Displays & Dials on Panel Face
            Transform face = panelBody.transform;

            // Captain Cluster (Left)
            BuildPFD(face, -0.62f, 0.05f, 8f); // Banked PFD
            BuildScreen(face, -0.36f, 0.05f, 0.20f, 0.20f, "NavDisplay_Capt");
            BuildDial(face, -0.92f, 0.12f, 0.14f, "AirspeedDial");
            BuildDial(face, -0.92f, -0.10f, 0.12f, "AltimeterDial");
            BuildDial(face, -0.62f, -0.16f, 0.11f, "BackupHorizon");

            // Center Cluster
            BuildScreen(face, 0.0f, 0.12f, 0.22f, 0.20f, "EngineDisplay_Upper");
            BuildScreen(face, 0.0f, -0.12f, 0.20f, 0.18f, "SystemsDisplay_Lower");
            BuildGearLever(face, 0.22f, 0.02f);

            // Co-Pilot Cluster (Right)
            BuildPFD(face, 0.62f, 0.05f, 0f); // Level PFD
            BuildScreen(face, 0.36f, 0.05f, 0.20f, 0.20f, "NavDisplay_FO");
            BuildDial(face, 0.92f, 0.12f, 0.14f, "AirspeedDial_FO");
            BuildDial(face, 0.92f, -0.10f, 0.12f, "AltimeterDial_FO");
            BuildDial(face, 0.62f, -0.16f, 0.11f, "BackupHorizon_FO");
        }

        private static void BuildPFD(Transform parent, float x, float y, float rollDeg)
        {
            GameObject pfd = new GameObject($"PFD_{x:F2}");
            pfd.transform.SetParent(parent, false);
            pfd.transform.localPosition = new Vector3(x, y, -0.065f);

            // Outer dark bezel
            MeshUtil.CreateBoxObject("Bezel", new Vector3(0.24f, 0.24f, 0.02f), CockpitMaterials.Dark, pfd.transform, Vector3.zero);

            // Banked split-screen artificial horizon
            GameObject horizon = new GameObject("ArtificialHorizon");
            horizon.transform.SetParent(pfd.transform, false);
            horizon.transform.localPosition = new Vector3(0f, 0f, -0.015f);
            horizon.transform.localRotation = Quaternion.Euler(0f, 0f, rollDeg);

            // Sky (Blue, emissive)
            MeshUtil.CreateBoxObject("Sky", new Vector3(0.20f, 0.10f, 0.005f), CockpitMaterials.HorizonBlue, horizon.transform, new Vector3(0f, 0.05f, 0f));
            // Ground (Orange, emissive)
            MeshUtil.CreateBoxObject("Ground", new Vector3(0.20f, 0.10f, 0.005f), CockpitMaterials.HorizonOrange, horizon.transform, new Vector3(0f, -0.05f, 0f));
            // Aircraft pitch reticle
            MeshUtil.CreateBoxObject("Reticle", new Vector3(0.08f, 0.01f, 0.008f), CockpitMaterials.DialFace, pfd.transform, new Vector3(0f, 0f, -0.02f));
        }

        private static void BuildScreen(Transform parent, float x, float y, float w, float h, string name)
        {
            GameObject scr = new GameObject(name);
            scr.transform.SetParent(parent, false);
            scr.transform.localPosition = new Vector3(x, y, -0.065f);

            // Bezel & dark glass
            MeshUtil.CreateBoxObject("Bezel", new Vector3(w + 0.03f, h + 0.03f, 0.02f), CockpitMaterials.Dark, scr.transform, Vector3.zero);
            MeshUtil.CreateBoxObject("ScreenGlass", new Vector3(w, h, 0.005f), CockpitMaterials.Floor, scr.transform, new Vector3(0f, 0f, -0.012f));

            // Green status ticks
            MeshUtil.CreateBoxObject("Indicator_1", new Vector3(0.015f, 0.015f, 0.008f), CockpitMaterials.EmissiveGreen, scr.transform, new Vector3(-w * 0.3f, h * 0.25f, -0.015f));
            MeshUtil.CreateBoxObject("Indicator_2", new Vector3(0.015f, 0.015f, 0.008f), CockpitMaterials.EmissiveGreen, scr.transform, new Vector3( w * 0.3f, h * 0.25f, -0.015f));
        }

        private static void BuildDial(Transform parent, float x, float y, float dia, string name)
        {
            GameObject dial = new GameObject(name);
            dial.transform.SetParent(parent, false);
            dial.transform.localPosition = new Vector3(x, y, -0.065f);

            // 10-sided cylindrical bezel
            Mesh bezelMesh = MeshUtil.CreatePrism(10, dia * 0.5f, 0.025f);
            GameObject bGo = MeshUtil.CreateMeshObject("Bezel", bezelMesh, CockpitMaterials.Dark, dial.transform);
            bGo.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            // Dial Face (Tan-pink)
            Mesh faceMesh = MeshUtil.CreatePrism(10, dia * 0.44f, 0.008f);
            GameObject fGo = MeshUtil.CreateMeshObject("Face", faceMesh, CockpitMaterials.DialFace, dial.transform);
            fGo.transform.localPosition = new Vector3(0f, 0f, -0.015f);
            fGo.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            // Needle
            MeshUtil.CreateBoxObject("Needle", new Vector3(0.008f, dia * 0.35f, 0.005f), CockpitMaterials.Dark, dial.transform, new Vector3(0f, dia * 0.15f, -0.02f));
        }

        private static void BuildGearLever(Transform parent, float x, float y)
        {
            GameObject gear = new GameObject("LandingGearLever");
            gear.transform.SetParent(parent, false);
            gear.transform.localPosition = new Vector3(x, y, -0.07f);

            // Lever Arm & White Wheel Knob
            MeshUtil.CreateBoxObject("Arm", new Vector3(0.02f, 0.12f, 0.02f), CockpitMaterials.Dark, gear.transform, new Vector3(0f, 0.02f, -0.02f));
            Mesh wheelMesh = MeshUtil.CreatePrism(8, 0.035f, 0.03f);
            GameObject wGo = MeshUtil.CreateMeshObject("WheelKnob", wheelMesh, CockpitMaterials.DialFace, gear.transform);
            wGo.transform.localPosition = new Vector3(0f, 0.08f, -0.03f);
            wGo.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);

            // 3 Gear Locked Green Lights (Nose, Left, Right)
            MeshUtil.CreateBoxObject("GearLight_Nose", Vector3.one * 0.018f, CockpitMaterials.EmissiveGreen, gear.transform, new Vector3(0f, 0.16f, -0.01f));
            MeshUtil.CreateBoxObject("GearLight_Left", Vector3.one * 0.018f, CockpitMaterials.EmissiveGreen, gear.transform, new Vector3(-0.025f, 0.13f, -0.01f));
            MeshUtil.CreateBoxObject("GearLight_Right", Vector3.one * 0.018f, CockpitMaterials.EmissiveGreen, gear.transform, new Vector3(0.025f, 0.13f, -0.01f));
        }

        public static void BuildOverhead(Transform parent)
        {
            GameObject ohGo = new GameObject("OverheadConsole");
            ohGo.transform.SetParent(parent, false);

            float w = InteriorConfig.OverheadHalfWidth * 2f; // 1.10m
            float z0 = InteriorConfig.OverheadZStart;         // 0.25m
            float z1 = InteriorConfig.OverheadZEnd;           // 1.05m
            float len = z1 - z0;
            float y = InteriorConfig.CockpitHeight - 0.10f;

            // Overhead panel base plate tilted 10 degrees down towards front
            GameObject basePlate = new GameObject("BasePlate");
            basePlate.transform.SetParent(ohGo.transform, false);
            basePlate.transform.localPosition = new Vector3(0f, y, (z0 + z1) * 0.5f);
            basePlate.transform.localRotation = Quaternion.Euler(10f, 0f, 0f);

            MeshUtil.CreateBoxObject("PanelFrame", new Vector3(w, 0.05f, len), CockpitMaterials.Panel, basePlate.transform, Vector3.zero);

            // 4 Dark Sub-panel inserts (Electrical, Fuel, Hydraulics, Lighting)
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    float px = -w * 0.25f + c * (w * 0.5f);
                    float pz = -len * 0.25f + r * (len * 0.5f);
                    Vector3 insPos = new Vector3(px, -0.028f, pz);
                    MeshUtil.CreateBoxObject($"Insert_{r}_{c}", new Vector3(w * 0.44f, 0.01f, len * 0.44f), CockpitMaterials.Dark, basePlate.transform, insPos);

                    // Add colored toggle cubes
                    for (int ti = 0; ti < 6; ti++)
                    {
                        float tx = px - 0.12f + (ti % 3) * 0.12f;
                        float tz = pz - 0.08f + (ti / 3) * 0.16f;
                        Material bMat = (ti % 4 == 0) ? CockpitMaterials.EmissiveGreen : (ti % 4 == 1) ? CockpitMaterials.HorizonOrange : (ti % 4 == 2) ? MaterialLibrary.IndicatorRed : CockpitMaterials.DialFace;
                        MeshUtil.CreateBoxObject($"Switch_{r}_{c}_{ti}", Vector3.one * 0.022f, bMat, basePlate.transform, new Vector3(tx, -0.04f, tz));
                    }
                }
            }
        }
    }
}
