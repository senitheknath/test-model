using System.Collections.Generic;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class GalleyWallUnits
    {
        public static void BuildBulkhead(Transform parent, GalleyType type)
        {
            GameObject bhGo = new GameObject("Bulkhead");
            bhGo.transform.SetParent(parent, false);

            float totalH = InteriorConfig.FuselageHeight;
            float doorW = InteriorConfig.GalleyDoorWidth;
            float doorH = InteriorConfig.GalleyDoorHeight;

            // Bulkhead main face wall at g = 0 (z = 0 in local space)
            // Left of closet (X from -2.10 to -1.30)
            float lW = 0.80f;
            Vector3 lPos = new Vector3(-1.70f, totalH * 0.5f, 0.02f);
            MeshUtil.CreateBoxObject("Bulkhead_Wall_L", new Vector3(lW, totalH, 0.04f), GalleyMaterials.WallLit, bhGo.transform, lPos, true);

            // Between closet and center door (X from -0.60 to -0.375)
            float mW = 0.225f;
            Vector3 mPos = new Vector3(-0.4875f, totalH * 0.5f, 0.02f);
            MeshUtil.CreateBoxObject("Bulkhead_Wall_M", new Vector3(mW, totalH, 0.04f), GalleyMaterials.WallLit, bhGo.transform, mPos, true);

            // Right of center door (X from 0.375 to 2.10)
            float rW = 1.725f;
            Vector3 rPos = new Vector3(0.375f + rW * 0.5f, totalH * 0.5f, 0.02f);
            MeshUtil.CreateBoxObject("Bulkhead_Wall_R", new Vector3(rW, totalH, 0.04f), GalleyMaterials.WallLit, bhGo.transform, rPos, true);

            // Header wall above doorway (Y from 2.00 to 2.60)
            float hH = totalH - doorH;
            Vector3 hPos = new Vector3(0f, doorH + hH * 0.5f, 0.02f);
            MeshUtil.CreateBoxObject("Bulkhead_Header", new Vector3(doorW, hH, 0.04f), GalleyMaterials.WallLit, bhGo.transform, hPos, true);

            // Door frame around center opening
            MeshUtil.CreateBoxObject("DoorFrame_L", new Vector3(0.05f, doorH, 0.06f), GalleyMaterials.WallTrim, bhGo.transform, new Vector3(-doorW * 0.5f - 0.025f, doorH * 0.5f, -0.01f));
            MeshUtil.CreateBoxObject("DoorFrame_R", new Vector3(0.05f, doorH, 0.06f), GalleyMaterials.WallTrim, bhGo.transform, new Vector3( doorW * 0.5f + 0.025f, doorH * 0.5f, -0.01f));
            MeshUtil.CreateBoxObject("DoorFrame_Top", new Vector3(doorW + 0.1f, 0.05f, 0.06f), GalleyMaterials.WallTrim, bhGo.transform, new Vector3(0f, doorH + 0.025f, -0.01f));

            // Rear Galley Door (if Rear Galley)
            if (type == GalleyType.Rear)
            {
                BuildRearPanelDoor(bhGo.transform, doorW, doorH);
            }

            // Closet Door & Recess at X = -0.95m
            BuildClosetDoor(bhGo.transform);

            // Dark Stacked Boxes at X = -1.95..-1.45, g = 0..0.5
            BuildDarkStack(bhGo.transform);
        }

        private static void BuildRearPanelDoor(Transform parent, float w, float h)
        {
            GameObject doorRoot = new GameObject("Door_Panel_Rear");
            doorRoot.transform.SetParent(parent, false);
            doorRoot.transform.localPosition = new Vector3(-w * 0.5f, 0f, 0.01f);

            GameObject leaf = new GameObject("Leaf");
            leaf.transform.SetParent(doorRoot.transform, false);
            leaf.transform.localPosition = new Vector3(w * 0.5f, h * 0.5f, 0f);

            MeshUtil.CreateBoxObject("DoorPanel", new Vector3(w - 0.02f, h - 0.02f, 0.04f), GalleyMaterials.DoorLeaf, leaf.transform, Vector3.zero, true);
            MeshUtil.CreateBoxObject("LatchPlate", new Vector3(0.06f, 0.14f, 0.05f), GalleyMaterials.WallTrim, leaf.transform, new Vector3(w * 0.5f - 0.08f, 0f, 0.01f));

            DoorInteractable di = doorRoot.AddComponent<DoorInteractable>();
            di.SetOpenAngle(-85f);
        }

        private static void BuildClosetDoor(Transform parent)
        {
            GameObject closetGo = new GameObject("StorageCloset");
            closetGo.transform.SetParent(parent, false);

            float cx = InteriorConfig.GalleyClosetX; // -0.95m
            float cw = InteriorConfig.GalleyClosetWidth; // 0.70m
            float ch = InteriorConfig.GalleyClosetHeight; // 2.00m
            float cr = InteriorConfig.GalleyClosetRecess; // 0.20m

            // Shallow recess back & sides
            Vector3 backPos = new Vector3(cx, ch * 0.5f, cr);
            MeshUtil.CreateBoxObject("ClosetBack", new Vector3(cw, ch, 0.02f), GalleyMaterials.Floor, closetGo.transform, backPos, true);
            MeshUtil.CreateBoxObject("ClosetSide_L", new Vector3(0.02f, ch, cr), GalleyMaterials.Floor, closetGo.transform, new Vector3(cx - cw * 0.5f, ch * 0.5f, cr * 0.5f), true);
            MeshUtil.CreateBoxObject("ClosetSide_R", new Vector3(0.02f, ch, cr), GalleyMaterials.Floor, closetGo.transform, new Vector3(cx + cw * 0.5f, ch * 0.5f, cr * 0.5f), true);

            // Shelf & 2 dark boxes inside recess
            Vector3 shelfPos = new Vector3(cx, 0.90f, cr * 0.5f);
            MeshUtil.CreateBoxObject("ClosetShelf", new Vector3(cw - 0.04f, 0.03f, cr), GalleyMaterials.Cabinet, closetGo.transform, shelfPos);
            MeshUtil.CreateBoxObject("Box_Inside_1", new Vector3(0.28f, 0.28f, 0.16f), GalleyMaterials.Trolley, closetGo.transform, new Vector3(cx - 0.15f, 1.06f, cr * 0.5f));
            MeshUtil.CreateBoxObject("Box_Inside_2", new Vector3(0.28f, 0.32f, 0.16f), GalleyMaterials.Trolley, closetGo.transform, new Vector3(cx + 0.15f, 1.08f, cr * 0.5f));

            // Hinged Closet Door leaf
            GameObject hingeRoot = new GameObject("Door_Closet");
            hingeRoot.transform.SetParent(closetGo.transform, false);
            hingeRoot.transform.localPosition = new Vector3(cx - cw * 0.5f, 0f, 0f);

            GameObject leaf = new GameObject("Leaf");
            leaf.transform.SetParent(hingeRoot.transform, false);
            leaf.transform.localPosition = new Vector3(cw * 0.5f, ch * 0.5f, 0f);

            MeshUtil.CreateBoxObject("DoorPanel", new Vector3(cw - 0.02f, ch - 0.02f, 0.04f), GalleyMaterials.DoorLeaf, leaf.transform, Vector3.zero, true);
            MeshUtil.CreateBoxObject("InsetLine", new Vector3(cw - 0.12f, ch - 0.12f, 0.042f), GalleyMaterials.DoorInset, leaf.transform, Vector3.zero);
            MeshUtil.CreateBoxObject("FlushHandle", new Vector3(0.05f, 0.14f, 0.045f), GalleyMaterials.CounterEdge, leaf.transform, new Vector3(cw * 0.5f - 0.08f, 0f, 0.005f));

            DoorInteractable di = hingeRoot.AddComponent<DoorInteractable>();
            di.SetOpenAngle(-90f);
        }

        private static void BuildDarkStack(Transform parent)
        {
            GameObject stackGo = new GameObject("DarkStack");
            stackGo.transform.SetParent(parent, false);

            float sx = -1.70f;
            float sw = 0.50f;
            float sd = 0.50f;
            float gz = -sd * 0.5f;

            float[] heights = new float[] { 0.40f, 0.38f, 0.36f };
            float currY = 0f;

            for (int i = 0; i < heights.Length; i++)
            {
                float h = heights[i];
                Vector3 pos = new Vector3(sx, currY + h * 0.5f, gz);
                MeshUtil.CreateBoxObject($"CargoBox_{i + 1}", new Vector3(sw, h - 0.015f, sd), GalleyMaterials.Floor, stackGo.transform, pos, true);

                // Thin lighter seam line around box
                MeshUtil.CreateBoxObject($"Seam_{i + 1}", new Vector3(sw + 0.005f, 0.015f, sd + 0.005f), GalleyMaterials.CounterTop, stackGo.transform, new Vector3(sx, currY + h, gz));
                currY += h;
            }
        }

        public static void BuildJumpseat(Transform parent)
        {
            GameObject jsGo = new GameObject("Jumpseat");
            jsGo.transform.SetParent(parent, false);

            float g = 0.815f; // Center between 0.58 and 1.05m
            float xLiner = InteriorConfig.GalleyLinerX; // 2.00m
            float alcW = InteriorConfig.GalleyJumpseatAlcoveW; // 0.50m
            float alcH = InteriorConfig.GalleyJumpseatAlcoveH; // 0.70m
            float alcD = InteriorConfig.GalleyJumpseatAlcoveD; // 0.12m

            // Recessed rectangular alcove on Left wall (X = -2.00m)
            Vector3 alcPos = new Vector3(-xLiner - alcD * 0.5f, 0.85f, -g);
            MeshUtil.CreateBoxObject("AlcoveFrame", new Vector3(alcD, alcH, alcW), GalleyMaterials.WallTrim, jsGo.transform, alcPos);

            // Backrest inside alcove
            Vector3 backPos = new Vector3(-xLiner + 0.04f, 0.90f, -g);
            MeshUtil.CreateBoxObject("Backrest", new Vector3(0.07f, InteriorConfig.GalleyJumpseatBackH, InteriorConfig.GalleyJumpseatW), GalleyMaterials.Trolley, jsGo.transform, backPos);

            // Folding cushion assembly with HingeInteractable
            GameObject hingeRoot = new GameObject("CushionHinge");
            hingeRoot.transform.SetParent(jsGo.transform, false);
            hingeRoot.transform.localPosition = new Vector3(-xLiner + 0.07f, 0.45f, -g);

            GameObject seatLeaf = new GameObject("CushionLeaf");
            seatLeaf.transform.SetParent(hingeRoot.transform, false);
            seatLeaf.transform.localPosition = new Vector3(InteriorConfig.GalleyJumpseatD * 0.5f, 0f, 0f);

            MeshUtil.CreateBoxObject("Cushion", new Vector3(InteriorConfig.GalleyJumpseatD, InteriorConfig.GalleyJumpseatCushH, InteriorConfig.GalleyJumpseatW), GalleyMaterials.Trolley, seatLeaf.transform, Vector3.zero, true);

            HingeInteractable hi = hingeRoot.AddComponent<HingeInteractable>();
            hi.SetAxis(Vector3.forward);
            hi.SetOpenAngle(85f);
        }

        public static void BuildEntryDoors(Transform parent, GalleyType type)
        {
            GameObject doorsGo = new GameObject("EntryDoors");
            doorsGo.transform.SetParent(parent, false);

            float gMid = 1.70f;
            float xLiner = InteriorConfig.GalleyLinerX; // 2.00m

            if (type == GalleyType.Front)
            {
                // Front Galley (Yaw 0): Local -X is World -X (Left), Local +X is World +X (Right)
                BuildSingleEntryDoor(doorsGo.transform, -xLiner, gMid, true, "Door_Entry_L");
                BuildSingleEntryDoor(doorsGo.transform,  xLiner, gMid, false, "Door_Entry_R");
            }
            else
            {
                // Rear Galley (Yaw 180): Local -X is World +X (Right), Local +X is World -X (Left)
                BuildSingleEntryDoor(doorsGo.transform, -xLiner, gMid, true, "Door_Rear_R");
                BuildSingleEntryDoor(doorsGo.transform,  xLiner, gMid, false, "Door_Rear_L");
            }
        }

        private static void BuildSingleEntryDoor(Transform parent, float x, float g, bool isLeft, string name)
        {
            GameObject doorRoot = new GameObject(name);
            doorRoot.transform.SetParent(parent, false);

            float frameThick = 0.10f;
            float doorW = InteriorConfig.GalleyEntryDoorLeafW; // 1.00m
            float doorH = InteriorConfig.GalleyEntryDoorLeafH; // 1.90m
            float rotY = isLeft ? 90f : -90f;

            // Doorway container positioned on wall line
            doorRoot.transform.localPosition = new Vector3(x, 0f, -g);
            doorRoot.transform.localRotation = Quaternion.Euler(0f, rotY, 0f);

            // Heavy Chamfered Frame (outer g 1.1..2.3)
            MeshUtil.CreateBoxObject("Frame_Top", new Vector3(doorW + 0.20f, frameThick, 0.12f), GalleyMaterials.WallTrim, doorRoot.transform, new Vector3(0f, doorH + frameThick * 0.5f, 0f));
            MeshUtil.CreateBoxObject("Frame_Left", new Vector3(frameThick, doorH, 0.12f), GalleyMaterials.WallTrim, doorRoot.transform, new Vector3(-doorW * 0.5f - frameThick * 0.5f, doorH * 0.5f, 0f));
            MeshUtil.CreateBoxObject("Frame_Right", new Vector3(frameThick, doorH, 0.12f), GalleyMaterials.WallTrim, doorRoot.transform, new Vector3( doorW * 0.5f + frameThick * 0.5f, doorH * 0.5f, 0f));

            // Flat black outer recess & invisible blocking collider
            Vector3 recPos = new Vector3(0f, doorH * 0.5f, -0.06f);
            MeshUtil.CreateBoxObject("OuterRecess", new Vector3(doorW, doorH, 0.02f), GalleyMaterials.GlassDark, doorRoot.transform, recPos, true);

            // Hinged Door Leaf
            GameObject hinge = new GameObject("HingeRoot");
            hinge.transform.SetParent(doorRoot.transform, false);
            hinge.transform.localPosition = new Vector3(-doorW * 0.5f, 0f, 0f);

            GameObject leaf = new GameObject("Leaf");
            leaf.transform.SetParent(hinge.transform, false);
            leaf.transform.localPosition = new Vector3(doorW * 0.5f, doorH * 0.5f, 0f);

            MeshUtil.CreateBoxObject("DoorPanel", new Vector3(doorW, doorH, 0.06f), GalleyMaterials.DoorLeaf, leaf.transform, Vector3.zero, true);

            // 10-sided Round Window (0.22 dia) at Y = 1.45m
            Mesh winMesh = MeshUtil.CreatePrism(10, InteriorConfig.GalleyEntryDoorWinDia * 0.5f, 0.07f);
            GameObject winGo = MeshUtil.CreateMeshObject("RoundWindow", winMesh, GalleyMaterials.GlassDark, leaf.transform);
            winGo.transform.localPosition = new Vector3(0f, InteriorConfig.GalleyEntryDoorWinY - doorH * 0.5f, 0f);
            winGo.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            // Small square viewing port (0.10m) beside handle
            MeshUtil.CreateBoxObject("ViewingPort", new Vector3(0.10f, 0.10f, 0.07f), GalleyMaterials.GlassDark, leaf.transform, new Vector3(0.18f, InteriorConfig.GalleyEntryDoorHandleY - doorH * 0.5f, 0f));

            // Lever Handle in Recessed Housing at Y = 0.95m
            Vector3 hPos = new Vector3(0f, InteriorConfig.GalleyEntryDoorHandleY - doorH * 0.5f, 0.035f);
            MeshUtil.CreateBoxObject("HandleHousing", new Vector3(0.24f, 0.12f, 0.02f), GalleyMaterials.WallTrim, leaf.transform, hPos);
            MeshUtil.CreateBoxObject("HandleLever", new Vector3(0.18f, 0.04f, 0.04f), GalleyMaterials.CounterEdge, leaf.transform, hPos + new Vector3(0f, 0f, 0.02f));

            // Red curved arrow decal around handle
            MeshUtil.CreateBoxObject("ArrowDecal_Top", new Vector3(0.22f, 0.02f, 0.005f), GalleyMaterials.Maroon, leaf.transform, hPos + new Vector3(0f, 0.10f, 0.035f));
            MeshUtil.CreateBoxObject("ArrowDecal_Side", new Vector3(0.02f, 0.18f, 0.005f), GalleyMaterials.Maroon, leaf.transform, hPos + new Vector3(0.12f, 0.01f, 0.035f));

            // Exit Sign above door in maroon with white letter cubes
            Vector3 exitPos = new Vector3(0f, doorH + 0.12f, 0.06f);
            MeshUtil.CreateBoxObject("ExitSign_Body", new Vector3(0.30f, 0.10f, 0.03f), GalleyMaterials.ExitGlow, doorRoot.transform, exitPos);

            // Vertical Handrail (0.03 octagonal, 0.9m long) on forward side
            Vector3 railPos = new Vector3(-doorW * 0.5f - 0.14f, 0.95f, 0.06f);
            Mesh railMesh = MeshUtil.CreatePrism(8, InteriorConfig.GalleyHandrailDia * 0.5f, InteriorConfig.GalleyHandrailH);
            GameObject railGo = MeshUtil.CreateMeshObject("Handrail", railMesh, GalleyMaterials.CounterEdge, doorRoot.transform);
            railGo.transform.localPosition = railPos;

            DoorInteractable di = hinge.AddComponent<DoorInteractable>();
            di.SetOpenAngle(-90f);
        }
    }
}
