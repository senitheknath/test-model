using UnityEngine;

namespace AirplaneBuilder
{
    public static class GalleyBuilder
    {
        public static void BuildFront(Transform parentRoot)
        {
            GameObject frontGalley = new GameObject("Galley_Front");
            frontGalley.transform.SetParent(parentRoot, false);

            GameObject doorsGroup = new GameObject("Doors");
            doorsGroup.transform.SetParent(frontGalley.transform, false);

            GameObject propsGroup = new GameObject("Props");
            propsGroup.transform.SetParent(frontGalley.transform, false);

            // Left & Right Entry Doors
            float fwdZ = -3.15f + InteriorConfig.EntryDoorWidth * 0.5f;
            DoorBuilder.CreateEntryDoor("Door_Entry_L", doorsGroup.transform, true, new Vector3(-2.06f, 0f, fwdZ));
            DoorBuilder.CreateEntryDoor("Door_Entry_R", doorsGroup.transform, false, new Vector3(2.06f, 0f, fwdZ));

            // Storage Closet Door and Plain Panel Door
            DoorBuilder.CreateHingedDoor("Door_Closet", new Vector3(0.68f, 1.95f, 0.04f), MaterialLibrary.Wall, MaterialLibrary.GalleyCabinet,
                doorsGroup.transform, new Vector3(-1.15f, 0f, -4.86f), true);

            DoorBuilder.CreateHingedDoor("Door_Panel", new Vector3(0.55f, 1.95f, 0.04f), MaterialLibrary.Wall, MaterialLibrary.GalleyCabinet,
                doorsGroup.transform, new Vector3(-0.55f, 0f, -4.86f), true);

            // Galley Service Unit on Right Side
            CreateGalleyServiceUnit(propsGroup.transform, new Vector3(1.25f, 0f, -4.60f), false);

            // Attendant Jump Seat on Left Wall
            CreateAttendantSeat(propsGroup.transform, new Vector3(-1.95f, 0f, -4.20f), true);
        }

        public static void BuildRear(Transform parentRoot)
        {
            GameObject rearGalley = new GameObject("Galley_Rear");
            rearGalley.transform.SetParent(parentRoot, false);

            GameObject doorsGroup = new GameObject("Doors");
            doorsGroup.transform.SetParent(rearGalley.transform, false);

            GameObject propsGroup = new GameObject("Props");
            propsGroup.transform.SetParent(rearGalley.transform, false);

            // Left & Right Rear Doors
            float fwdZ = -26.85f + InteriorConfig.EntryDoorWidth * 0.5f;
            DoorBuilder.CreateEntryDoor("Door_Rear_L", doorsGroup.transform, true, new Vector3(-2.06f, 0f, fwdZ));
            DoorBuilder.CreateEntryDoor("Door_Rear_R", doorsGroup.transform, false, new Vector3(2.06f, 0f, fwdZ));

            // Rear Galley Service Unit against Tail Wall
            CreateGalleyServiceUnit(propsGroup.transform, new Vector3(1.25f, 0f, -28.25f), false);

            // Attendant Jump Seat
            CreateAttendantSeat(propsGroup.transform, new Vector3(-1.25f, 0f, -28.25f), false);
        }

        private static void CreateGalleyServiceUnit(Transform parent, Vector3 localPos, bool facingNorth)
        {
            GameObject unit = new GameObject("Galley_Unit");
            unit.transform.SetParent(parent, false);
            unit.transform.localPosition = localPos;
            if (facingNorth) unit.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

            float w = InteriorConfig.GalleyCabinetWidth;
            float h = InteriorConfig.GalleyCabinetHeight;
            float d = InteriorConfig.GalleyCounterDepth;

            // Main Cabinet Framework
            MeshUtil.CreateBoxObject("Framework", new Vector3(w, h, d), MaterialLibrary.GalleyCabinet, unit.transform,
                new Vector3(0f, h * 0.5f, 0f), true);

            // 2 Charcoal Meal Trolleys stowed under counter
            CreateTrolley(unit.transform, new Vector3(-0.35f, 0f, 0.04f));
            CreateTrolley(unit.transform, new Vector3(0.02f, 0f, 0.04f));

            // Lower Right Cabinet Drawer
            MeshUtil.CreateBoxObject("LowerDrawer", new Vector3(0.44f, 0.84f, 0.04f), MaterialLibrary.GalleyCabinet, unit.transform,
                new Vector3(0.40f, 0.44f, d * 0.5f + 0.02f));
            MeshUtil.CreateBoxObject("DrawerLatch", new Vector3(0.08f, 0.04f, 0.02f), MaterialLibrary.GalleyAccent, unit.transform,
                new Vector3(0.40f, 0.72f, d * 0.5f + 0.04f));

            // Convection Oven
            CreateOven(unit.transform, new Vector3(0.40f, 1.15f, d * 0.5f));

            // Microwave Oven
            CreateMicrowave(unit.transform, new Vector3(0.40f, 1.55f, d * 0.5f));

            // Upper Cabinets (Left & Center)
            CreateUpperCabinet(unit.transform, new Vector3(-0.35f, 1.82f, d * 0.5f + 0.01f), 0.34f, 0.48f);
            CreateUpperCabinet(unit.transform, new Vector3(0.02f, 1.82f, d * 0.5f + 0.01f), 0.34f, 0.48f);
            CreateUpperCabinet(unit.transform, new Vector3(-0.35f, 1.30f, d * 0.5f + 0.01f), 0.34f, 0.44f);
            CreateUpperCabinet(unit.transform, new Vector3(0.02f, 1.30f, d * 0.5f + 0.01f), 0.34f, 0.44f);

            // Open Shelf with Maroon Cups
            CreateCupShelf(unit.transform, new Vector3(0.40f, 1.88f, d * 0.5f));
        }

        private static void CreateTrolley(Transform parent, Vector3 localPos)
        {
            GameObject trolley = new GameObject("Trolley");
            trolley.transform.SetParent(parent, false);
            trolley.transform.localPosition = localPos;

            float tw = InteriorConfig.TrolleyWidth;
            float th = InteriorConfig.TrolleyHeight;
            float td = InteriorConfig.TrolleyDepth;

            // Trolley Body
            MeshUtil.CreateBoxObject("Body", new Vector3(tw, th, td), MaterialLibrary.GalleyTrolley, trolley.transform,
                new Vector3(0f, th * 0.5f, 0f), true);

            // Top Handle Bar
            MeshUtil.CreateBoxObject("Handle", new Vector3(tw - 0.06f, 0.03f, 0.04f), MaterialLibrary.WallShadow, trolley.transform,
                new Vector3(0f, th + 0.02f, td * 0.45f));

            // Front Red Latch Accent
            MeshUtil.CreateBoxObject("LatchAccent", new Vector3(0.06f, 0.08f, 0.02f), MaterialLibrary.GalleyAccent, trolley.transform,
                new Vector3(0f, th * 0.82f, td * 0.5f + 0.01f));

            // Lower Pedal / Foot Latch
            MeshUtil.CreateBoxObject("FootPedal", new Vector3(0.08f, 0.04f, 0.04f), MaterialLibrary.GalleyAccent, trolley.transform,
                new Vector3(0f, 0.04f, td * 0.5f + 0.02f));
        }

        private static void CreateMicrowave(Transform parent, Vector3 localPos)
        {
            GameObject mw = new GameObject("Microwave");
            mw.transform.SetParent(parent, false);
            mw.transform.localPosition = localPos;

            float mwW = InteriorConfig.MicrowaveWidth;
            float mwH = InteriorConfig.MicrowaveHeight;
            float mwD = 0.04f;

            // Front Faceplate
            MeshUtil.CreateBoxObject("Faceplate", new Vector3(mwW, mwH, mwD), MaterialLibrary.WallShadow, mw.transform, Vector3.zero);

            // Dark Glass Door Window
            MeshUtil.CreateBoxObject("Glass", new Vector3(mwW * 0.65f, mwH * 0.70f, mwD + 0.01f), MaterialLibrary.WindowGlass, mw.transform,
                new Vector3(-mwW * 0.12f, 0f, 0.005f));

            // Control Keypad
            MeshUtil.CreateBoxObject("Keypad", new Vector3(mwW * 0.20f, mwH * 0.70f, mwD + 0.01f), MaterialLibrary.CockpitPanel, mw.transform,
                new Vector3(mwW * 0.35f, 0f, 0.005f));
        }

        private static void CreateOven(Transform parent, Vector3 localPos)
        {
            GameObject oven = new GameObject("Oven");
            oven.transform.SetParent(parent, false);
            oven.transform.localPosition = localPos;

            float oW = InteriorConfig.OvenWidth;
            float oH = InteriorConfig.OvenHeight;
            float oD = 0.04f;

            // Oven Body Plate
            MeshUtil.CreateBoxObject("Faceplate", new Vector3(oW, oH, oD), MaterialLibrary.WallShadow, oven.transform, Vector3.zero);

            // Oven Window
            MeshUtil.CreateBoxObject("Window", new Vector3(oW * 0.75f, oH * 0.45f, oD + 0.01f), MaterialLibrary.WindowGlass, oven.transform,
                new Vector3(0f, -0.02f, 0.005f));

            // Handle Bar
            MeshUtil.CreateBoxObject("Handle", new Vector3(oW * 0.60f, 0.025f, 0.03f), MaterialLibrary.GalleyTrolley, oven.transform,
                new Vector3(0f, oH * 0.32f, 0.025f));

            // Red Indicator Accent
            MeshUtil.CreateBoxObject("Indicator", new Vector3(0.04f, 0.04f, 0.02f), MaterialLibrary.GalleyAccent, oven.transform,
                new Vector3(oW * 0.38f, oH * 0.32f, 0.015f));
        }

        private static void CreateUpperCabinet(Transform parent, Vector3 localPos, float width, float height)
        {
            GameObject cab = MeshUtil.CreateBoxObject("UpperDoor", new Vector3(width, height, 0.03f), MaterialLibrary.GalleyCabinet, parent, localPos);
            // Maroon Latch Marker
            MeshUtil.CreateBoxObject("Latch", new Vector3(0.05f, 0.05f, 0.02f), MaterialLibrary.GalleyAccent, cab.transform,
                new Vector3(width * 0.35f, -height * 0.35f, 0.02f));
        }

        private static void CreateCupShelf(Transform parent, Vector3 localPos)
        {
            GameObject shelf = new GameObject("CupShelf");
            shelf.transform.SetParent(parent, false);
            shelf.transform.localPosition = localPos;

            // Nook Background
            MeshUtil.CreateBoxObject("Nook", new Vector3(0.42f, 0.22f, 0.02f), MaterialLibrary.CockpitPanel, shelf.transform, Vector3.zero);

            // 4 Maroon Cups stacked/arranged in shelf
            float r = InteriorConfig.CupRadius;
            float ch = InteriorConfig.CupHeight;
            for (int i = 0; i < 4; i++)
            {
                Mesh cupMesh = MeshUtil.CreatePrism(8, r, ch);
                GameObject cup = MeshUtil.CreateMeshObject($"Cup_{i + 1}", cupMesh, MaterialLibrary.GalleyAccent, shelf.transform);
                cup.transform.localPosition = new Vector3(-0.14f + i * 0.09f, -0.05f, 0.06f);
            }
        }

        private static void CreateAttendantSeat(Transform parent, Vector3 localPos, bool onWall)
        {
            GameObject jump = new GameObject("AttendantJumpSeat");
            jump.transform.SetParent(parent, false);
            jump.transform.localPosition = localPos;

            Material mat = MaterialLibrary.CockpitSeat;

            // Backboard against wall
            MeshUtil.CreateBoxObject("Backboard", new Vector3(0.44f, 1.25f, 0.06f), MaterialLibrary.CockpitPanel, jump.transform,
                new Vector3(0f, 0.65f, 0f), true);

            // Folded Seat Cushion
            MeshUtil.CreateBoxObject("FoldedCushion", new Vector3(0.40f, 0.42f, 0.08f), mat, jump.transform,
                new Vector3(0f, 0.55f, 0.06f));

            // Headrest
            MeshUtil.CreateBoxObject("Headrest", new Vector3(0.30f, 0.18f, 0.06f), mat, jump.transform,
                new Vector3(0f, 1.15f, 0.05f));
        }
    }
}
