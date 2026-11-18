using System.Collections.Generic;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class GalleyProps
    {
        public static void BuildWasteBin(Transform parent)
        {
            GameObject binGo = new GameObject("WasteBin");
            binGo.transform.SetParent(parent, false);

            float w = 0.42f, d = 0.42f, h = 0.75f;
            float midX = 1.79f, midZ = -0.85f;
            Vector3 basePos = new Vector3(midX, h * 0.5f, midZ);

            MeshUtil.CreateBoxObject("InnerBox", new Vector3(w - 0.04f, h - 0.06f, d - 0.04f), GalleyMaterials.Trolley, binGo.transform, basePos + Vector3.up * 0.03f, true);

            float hw = (w - 0.03f) * 0.5f, hd = (d - 0.03f) * 0.5f;
            Vector3[] corners = new Vector3[] { new Vector3(-hw, 0f, -hd), new Vector3(hw, 0f, -hd), new Vector3(-hw, 0f, hd), new Vector3(hw, 0f, hd) };
            for (int i = 0; i < 4; i++)
            {
                MeshUtil.CreateBoxObject($"CornerRail_{i + 1}", new Vector3(0.03f, h, 0.03f), GalleyMaterials.WallTrim, binGo.transform, basePos + corners[i]);
                Vector3 wPos = new Vector3(midX + corners[i].x, 0.025f, midZ + corners[i].z);
                Mesh wheel = MeshUtil.CreatePrism(8, 0.025f, 0.02f);
                GameObject wh = MeshUtil.CreateMeshObject($"Wheel_{i + 1}", wheel, GalleyMaterials.GlassDark, binGo.transform);
                wh.transform.localPosition = wPos;
                wh.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            }

            MeshUtil.CreateBoxObject("Frame_Bottom", new Vector3(w, 0.04f, d), GalleyMaterials.WallTrim, binGo.transform, new Vector3(midX, 0.04f, midZ));
            MeshUtil.CreateBoxObject("Frame_Top", new Vector3(w, 0.04f, d), GalleyMaterials.WallTrim, binGo.transform, new Vector3(midX, h, midZ));
            MeshUtil.CreateBoxObject("Pedal_L", new Vector3(0.08f, 0.03f, 0.06f), GalleyMaterials.CounterEdge, binGo.transform, new Vector3(midX - 0.08f, 0.03f, midZ - hd - 0.04f));
            MeshUtil.CreateBoxObject("Pedal_R", new Vector3(0.08f, 0.03f, 0.06f), GalleyMaterials.CounterEdge, binGo.transform, new Vector3(midX + 0.08f, 0.03f, midZ - hd - 0.04f));

            GameObject lidHinge = new GameObject("WasteBin_Lid");
            lidHinge.transform.SetParent(binGo.transform, false);
            lidHinge.transform.localPosition = new Vector3(midX, h + 0.02f, midZ + hd);
            GameObject lidLeaf = new GameObject("LidLeaf");
            lidLeaf.transform.SetParent(lidHinge.transform, false);
            lidLeaf.transform.localPosition = new Vector3(0f, 0f, -d * 0.5f);
            MeshUtil.CreateBoxObject("FlapPanel", new Vector3(w - 0.02f, 0.02f, d - 0.02f), GalleyMaterials.DoorInset, lidLeaf.transform, Vector3.zero, true);

            HingeInteractable hi = lidHinge.AddComponent<HingeInteractable>();
            hi.SetAxis(Vector3.right);
            hi.SetOpenAngle(-100f);
            hi.SetOpenImmediate(true);

            Vector3 lockPos = new Vector3(midX, 1.975f, midZ);
            MeshUtil.CreateBoxObject("Locker_WasteBin", new Vector3(w, 0.35f, d), GalleyMaterials.Cabinet, binGo.transform, lockPos, true);
            MeshUtil.CreateBoxObject("Locker_WasteBin_Tab", new Vector3(0.06f, 0.03f, 0.02f), GalleyMaterials.Maroon, binGo.transform, lockPos + new Vector3(0f, -0.16f, -d * 0.5f - 0.01f));
        }

        public static void BuildOvenWall(Transform parent, float length)
        {
            GameObject ovenWallGo = new GameObject("OvenWall");
            ovenWallGo.transform.SetParent(parent, false);

            float midZ = -(length - 0.45f), midX = 1.725f, depth = 0.55f, totalW = 0.90f;
            MeshUtil.CreateBoxObject("OvenBaseCabinet", new Vector3(depth, 0.90f, totalW), GalleyMaterials.Cabinet, ovenWallGo.transform, new Vector3(midX, 0.45f, midZ), true);
            MeshUtil.CreateBoxObject("BaseCabTab_1", new Vector3(0.02f, 0.03f, 0.06f), GalleyMaterials.Maroon, ovenWallGo.transform, new Vector3(midX - depth * 0.5f - 0.01f, 0.75f, midZ - 0.22f));
            MeshUtil.CreateBoxObject("BaseCabTab_2", new Vector3(0.02f, 0.03f, 0.06f), GalleyMaterials.Maroon, ovenWallGo.transform, new Vector3(midX - depth * 0.5f - 0.01f, 0.75f, midZ + 0.22f));

            float ovenH = 0.55f, ovenW = 0.42f, ovenD = 0.50f;
            float[] ovenZOffsets = new float[] { -0.22f, 0.22f };

            for (int i = 0; i < 2; i++)
            {
                GameObject ovenObj = new GameObject($"Oven_{i + 1:D2}");
                ovenObj.transform.SetParent(ovenWallGo.transform, false);
                ovenObj.transform.localPosition = new Vector3(midX, 0.95f + ovenH * 0.5f, midZ + ovenZOffsets[i]);

                MeshUtil.CreateBoxObject("Body", new Vector3(ovenD, ovenH, ovenW), GalleyMaterials.OvenBody, ovenObj.transform, Vector3.zero, true);
                Vector3 ctrlPos = new Vector3(-ovenD * 0.5f - 0.01f, ovenH * 0.5f - 0.05f, 0f);
                MeshUtil.CreateBoxObject("ControlStrip", new Vector3(0.02f, 0.10f, ovenW), GalleyMaterials.CounterTop, ovenObj.transform, ctrlPos);
                MeshUtil.CreateBoxObject("Display", new Vector3(0.025f, 0.03f, 0.10f), GalleyMaterials.ExitGlow, ovenObj.transform, ctrlPos + new Vector3(0f, 0f, -0.10f));

                Mesh knobMesh = MeshUtil.CreatePrism(8, 0.02f, 0.02f);
                GameObject knob = MeshUtil.CreateMeshObject("ControlKnob", knobMesh, GalleyMaterials.CounterEdge, ovenObj.transform);
                knob.transform.localPosition = ctrlPos + new Vector3(0f, 0f, 0.10f);
                knob.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

                GameObject doorHinge = new GameObject("OvenDoor");
                doorHinge.transform.SetParent(ovenObj.transform, false);
                doorHinge.transform.localPosition = new Vector3(-ovenD * 0.5f, -0.05f, -ovenW * 0.5f);

                GameObject doorLeaf = new GameObject("Leaf");
                doorLeaf.transform.SetParent(doorHinge.transform, false);
                doorLeaf.transform.localPosition = new Vector3(0f, 0f, ovenW * 0.5f);

                MeshUtil.CreateBoxObject("DoorFrame", new Vector3(0.03f, 0.38f, ovenW), GalleyMaterials.OvenBody, doorLeaf.transform, Vector3.zero, true);
                MeshUtil.CreateBoxObject("DarkGlass", new Vector3(0.035f, 0.22f, 0.30f), GalleyMaterials.GlassDark, doorLeaf.transform, Vector3.zero);
                MeshUtil.CreateBoxObject("HandleBar", new Vector3(0.03f, 0.28f, 0.03f), GalleyMaterials.CounterEdge, doorLeaf.transform, new Vector3(-0.025f, 0f, ovenW * 0.5f - 0.03f));
                MeshUtil.CreateBoxObject("LatchTab", new Vector3(0.02f, 0.03f, 0.06f), GalleyMaterials.Maroon, ovenObj.transform, new Vector3(-ovenD * 0.5f - 0.01f, ovenH * 0.5f + 0.05f, 0f));

                HingeInteractable hi = doorHinge.AddComponent<HingeInteractable>();
                hi.SetAxis(Vector3.up);
                hi.SetOpenAngle(-85f);
            }
        }

        public static void BuildPrepCounter(Transform parent, float length)
        {
            GameObject prepGo = new GameObject("PrepCounter");
            prepGo.transform.SetParent(parent, false);

            float midZ = -(length - 0.45f), midX = -1.725f, depth = 0.55f, totalW = 0.90f;
            MeshUtil.CreateBoxObject("CounterSlab", new Vector3(depth, 0.05f, totalW), GalleyMaterials.CounterTop, prepGo.transform, new Vector3(midX, 0.875f, midZ), true);
            MeshUtil.CreateBoxObject("FrontEdge", new Vector3(0.015f, 0.05f, totalW), GalleyMaterials.CounterEdge, prepGo.transform, new Vector3(midX + depth * 0.5f + 0.007f, 0.875f, midZ));
            MeshUtil.CreateBoxObject("BackLip", new Vector3(0.015f, 0.06f, totalW), GalleyMaterials.CounterEdge, prepGo.transform, new Vector3(midX - depth * 0.5f - 0.007f, 0.93f, midZ));

            // Lower base cabinets with 2 doors & vertical slat grooves
            MeshUtil.CreateBoxObject("BaseCabinet", new Vector3(depth, 0.85f, totalW), GalleyMaterials.Cabinet, prepGo.transform, new Vector3(midX, 0.425f, midZ), true);
            MeshUtil.CreateBoxObject("CabTab_1", new Vector3(0.02f, 0.03f, 0.06f), GalleyMaterials.Maroon, prepGo.transform, new Vector3(midX + depth * 0.5f + 0.01f, 0.72f, midZ - 0.22f));
            MeshUtil.CreateBoxObject("CabTab_2", new Vector3(0.02f, 0.03f, 0.06f), GalleyMaterials.Maroon, prepGo.transform, new Vector3(midX + depth * 0.5f + 0.01f, 0.72f, midZ + 0.22f));

            // On top: 2 tray sheet stacks, 1 white paper stack, 1 printed flat sheet
            Vector3 s1 = new Vector3(midX, 0.91f, midZ - 0.25f);
            MeshUtil.CreateBoxObject("TraySheetStack_1", new Vector3(0.24f, 0.02f, 0.35f), GalleyMaterials.DoorLeaf, prepGo.transform, s1);
            Vector3 s2 = new Vector3(midX, 0.91f, midZ);
            MeshUtil.CreateBoxObject("PaperStack", new Vector3(0.28f, 0.02f, 0.20f), GalleyMaterials.DoorLeaf, prepGo.transform, s2);
            Vector3 s3 = new Vector3(midX, 0.902f, midZ + 0.25f);
            MeshUtil.CreateBoxObject("PrintedSheet", new Vector3(0.20f, 0.003f, 0.28f), GalleyMaterials.DoorLeaf, prepGo.transform, s3);
            MeshUtil.CreateBoxObject("PrintedLine_1", new Vector3(0.16f, 0.005f, 0.01f), GalleyMaterials.CounterEdge, prepGo.transform, s3 + new Vector3(0f, 0.002f, -0.06f));
            MeshUtil.CreateBoxObject("PrintedLine_2", new Vector3(0.16f, 0.005f, 0.01f), GalleyMaterials.CounterEdge, prepGo.transform, s3 + new Vector3(0f, 0.002f, 0.02f));
        }

        public static void BuildCoffeeShelf(Transform parent, float length)
        {
            GameObject csGo = new GameObject("CoffeeShelf");
            csGo.transform.SetParent(parent, false);

            float midZ = -(length - 0.45f), midX = -1.80f, depth = 0.40f, totalW = 0.90f;
            MeshUtil.CreateBoxObject("ShelfBoard", new Vector3(depth, 0.03f, totalW), GalleyMaterials.CounterTop, csGo.transform, new Vector3(midX, 1.25f, midZ), true);
            MeshUtil.CreateBoxObject("BackPanel", new Vector3(0.02f, 0.55f, totalW), GalleyMaterials.GlassDark, csGo.transform, new Vector3(-1.99f, 1.525f, midZ));

            // 2 Coffee makers
            float[] cmZ = new float[] { midZ - 0.22f, midZ + 0.12f };
            for (int i = 0; i < 2; i++)
            {
                Vector3 cmPos = new Vector3(midX, 1.425f, cmZ[i]);
                MeshUtil.CreateBoxObject($"CoffeeMaker_{i + 1}", new Vector3(0.30f, 0.35f, 0.22f), GalleyMaterials.OvenBody, csGo.transform, cmPos, true);
                MeshUtil.CreateBoxObject($"ControlStrip_{i + 1}", new Vector3(0.02f, 0.08f, 0.22f), GalleyMaterials.CounterTop, csGo.transform, cmPos + new Vector3(0.16f, 0.12f, 0f));
                MeshUtil.CreateBoxObject($"WarmingPlate_{i + 1}", new Vector3(0.16f, 0.02f, 0.16f), GalleyMaterials.Floor, csGo.transform, cmPos + new Vector3(0.06f, -0.16f, 0f));

                // Octagonal glass pot with black handle & coffee
                Vector3 potPos = cmPos + new Vector3(0.06f, -0.07f, 0f);
                Mesh potMesh = MeshUtil.CreatePrism(8, 0.06f, 0.16f);
                GameObject pot = MeshUtil.CreateMeshObject($"GlassPot_{i + 1}", potMesh, GalleyMaterials.WallLit, csGo.transform);
                pot.transform.localPosition = potPos;
                MeshUtil.CreateBoxObject($"PotHandle_{i + 1}", new Vector3(0.02f, 0.10f, 0.04f), GalleyMaterials.GlassDark, csGo.transform, potPos + new Vector3(0.07f, 0f, 0f));
            }

            // Cup Stacks: 5 cups on left, 9 on right
            BuildCupStack(csGo.transform, new Vector3(midX + 0.08f, 1.265f, midZ - 0.38f), 5);
            BuildCupStack(csGo.transform, new Vector3(midX + 0.08f, 1.265f, midZ + 0.34f), 9);

            // Light gray tray leaning against back panel
            GameObject tray = MeshUtil.CreateBoxObject("Tray_Leaning", new Vector3(0.02f, 0.25f, 0.35f), GalleyMaterials.Cabinet, csGo.transform, new Vector3(-1.95f, 1.40f, midZ));
            tray.transform.localRotation = Quaternion.Euler(0f, 0f, -12f);
        }

        private static void BuildCupStack(Transform parent, Vector3 basePos, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Mesh cupMesh = MeshUtil.CreatePrism(8, 0.025f, 0.05f);
                GameObject cup = MeshUtil.CreateMeshObject($"Cup_{i + 1}", cupMesh, GalleyMaterials.DoorLeaf, parent);
                cup.transform.localPosition = basePos + Vector3.up * (i * 0.022f);
            }
        }

        public static void BuildIntercom(Transform parent)
        {
            GameObject icGo = new GameObject("IntercomStation");
            icGo.transform.SetParent(parent, false);

            float x = -1.98f, y = 1.45f, z = -0.275f;
            Vector3 mainPos = new Vector3(x, y, z);

            MeshUtil.CreateBoxObject("Plate_Main", new Vector3(0.04f, 0.50f, 0.35f), GalleyMaterials.CounterTop, icGo.transform, mainPos);

            // Handset & cradle on forward half
            Vector3 hsPos = mainPos + new Vector3(0.035f, 0.02f, 0.08f);
            MeshUtil.CreateBoxObject("Handset_Body", new Vector3(0.04f, 0.20f, 0.05f), GalleyMaterials.GlassDark, icGo.transform, hsPos);
            MeshUtil.CreateBoxObject("Handset_Ear", new Vector3(0.05f, 0.04f, 0.05f), GalleyMaterials.GlassDark, icGo.transform, hsPos + Vector3.up * 0.10f);
            MeshUtil.CreateBoxObject("Handset_Mouth", new Vector3(0.05f, 0.04f, 0.05f), GalleyMaterials.GlassDark, icGo.transform, hsPos - Vector3.up * 0.10f);

            // Coiled cord zigzag below handset
            for (int i = 0; i < 4; i++)
            {
                float cy = hsPos.y - 0.15f - i * 0.04f;
                float cz = hsPos.z + ((i % 2 == 0) ? 0.015f : -0.015f);
                MeshUtil.CreateBoxObject($"CordSeg_{i + 1}", new Vector3(0.02f, 0.03f, 0.02f), GalleyMaterials.GlassDark, icGo.transform, new Vector3(x + 0.03f, cy, cz));
            }

            // Keypad & LEDs on aft half
            Vector3 kpBase = mainPos + new Vector3(0.025f, 0.05f, -0.08f);
            MeshUtil.CreateBoxObject("DisplayStrip", new Vector3(0.005f, 0.03f, 0.12f), GalleyMaterials.GlassDark, icGo.transform, kpBase + Vector3.up * 0.12f);
            MeshUtil.CreateBoxObject("LED_Green", new Vector3(0.01f, 0.015f, 0.015f), GalleyMaterials.DoorLeaf, icGo.transform, kpBase + new Vector3(0f, 0.12f, 0.08f));
            MeshUtil.CreateBoxObject("LED_Amber", new Vector3(0.01f, 0.015f, 0.015f), GalleyMaterials.ExitGlow, icGo.transform, kpBase + new Vector3(0f, 0.12f, -0.08f));

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    Vector3 kPos = kpBase + new Vector3(0f, -r * 0.04f, (c == 0 ? 0.03f : -0.03f));
                    MeshUtil.CreateBoxObject($"Key_{r}_{c}", new Vector3(0.01f, 0.025f, 0.025f), GalleyMaterials.CounterEdge, icGo.transform, kPos);
                }
            }

            // Lower emergency button plate
            Vector3 lowPos = mainPos + new Vector3(0f, -0.32f, 0f);
            MeshUtil.CreateBoxObject("Plate_Lower", new Vector3(0.03f, 0.18f, 0.28f), GalleyMaterials.WallTrim, icGo.transform, lowPos);
            for (int i = 0; i < 3; i++)
            {
                Vector3 bPos = lowPos + new Vector3(0.02f, 0f, -0.07f + i * 0.07f);
                MeshUtil.CreateBoxObject($"RedBtn_{i + 1}", new Vector3(0.02f, 0.03f, 0.03f), GalleyMaterials.Maroon, icGo.transform, bPos);
            }
        }

        public static void BuildUpperLockers(Transform parent, float length)
        {
            GameObject lockGo = new GameObject("UpperLockers");
            lockGo.transform.SetParent(parent, false);

            float aftStart = length - 0.90f;
            float midZ = -(aftStart + length) * 0.5f;
            float midX = -1.75f;
            float w = 0.28f, h = 0.35f, d = 0.40f;

            for (int i = 0; i < 3; i++)
            {
                float z = midZ - 0.28f + i * 0.28f;
                Vector3 pos = new Vector3(midX, 1.975f, z);
                MeshUtil.CreateBoxObject($"Locker_{i + 1}", new Vector3(d, h, w - 0.02f), GalleyMaterials.Cabinet, lockGo.transform, pos, true);
                MeshUtil.CreateBoxObject($"Locker_{i + 1}_Tab", new Vector3(0.02f, 0.03f, 0.06f), GalleyMaterials.Maroon, lockGo.transform, pos + new Vector3(d * 0.5f + 0.01f, -0.16f, 0f));
            }
        }
    }
}
