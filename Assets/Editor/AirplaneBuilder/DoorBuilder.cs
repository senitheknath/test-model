using UnityEngine;

namespace AirplaneBuilder
{
    public static class DoorBuilder
    {
        public static GameObject CreateHingedDoor(string name, Vector3 size, Material frameMat, Material doorMat, Transform parent, Vector3 position, bool hingeOnLeft = true)
        {
            GameObject hingeRoot = new GameObject(name);
            hingeRoot.transform.SetParent(parent, false);
            hingeRoot.transform.localPosition = position;

            float panelOffsetX = hingeOnLeft ? size.x * 0.5f : -size.x * 0.5f;

            GameObject panel = MeshUtil.CreateBoxObject("DoorPanel", size, doorMat, hingeRoot.transform,
                new Vector3(panelOffsetX, size.y * 0.5f, 0f), true);

            float handleX = hingeOnLeft ? size.x * 0.85f : -size.x * 0.85f;
            MeshUtil.CreateBoxObject("Handle", new Vector3(0.04f, 0.12f, 0.08f), MaterialLibrary.GalleyTrolley, panel.transform,
                new Vector3(handleX, 0f, 0.04f));

            return hingeRoot;
        }

        public static GameObject CreateExitDoor(string name, Transform parent, bool isLeft, Vector3 position)
        {
            GameObject hingeRoot = new GameObject(name);
            hingeRoot.transform.SetParent(parent, false);
            hingeRoot.transform.localPosition = position;

            float halfW = InteriorConfig.ExitDoorWidth * 0.5f;
            Vector3 panelCenter = new Vector3(0f, 0f, -halfW);

            MeshUtil.CreateBoxObject("DoorFrame",
                new Vector3(InteriorConfig.ExitDoorFrameThickness, InteriorConfig.ExitDoorHeight + 0.1f, InteriorConfig.ExitDoorWidth + 0.1f),
                MaterialLibrary.WallShadow, hingeRoot.transform, panelCenter, false);

            GameObject hatch = MeshUtil.CreateBoxObject("HatchPanel",
                new Vector3(0.05f, InteriorConfig.ExitDoorHeight, InteriorConfig.ExitDoorWidth),
                MaterialLibrary.GalleyCabinet, hingeRoot.transform, panelCenter, true);

            MeshUtil.CreateBoxObject("Window", new Vector3(0.06f, 0.32f, 0.22f), MaterialLibrary.WindowGlass, hatch.transform, new Vector3(0f, 0.18f, 0f));

            float leverDir = isLeft ? 0.04f : -0.04f;
            MeshUtil.CreateBoxObject("HandleLever", new Vector3(0.04f, 0.06f, 0.24f), MaterialLibrary.GalleyAccent, hatch.transform, new Vector3(leverDir, -0.10f, 0f));

            MeshUtil.CreateBoxObject("ExitSign", new Vector3(0.04f, 0.12f, 0.28f), MaterialLibrary.IndicatorGreen, hatch.transform, new Vector3(leverDir, InteriorConfig.ExitDoorHeight * 0.44f, 0f));

            return hingeRoot;
        }

        public static GameObject CreateEntryDoor(string name, Transform parent, bool isLeft, Vector3 position)
        {
            GameObject hingeRoot = new GameObject(name);
            hingeRoot.transform.SetParent(parent, false);
            hingeRoot.transform.localPosition = position;

            float halfW = InteriorConfig.EntryDoorWidth * 0.5f;
            // Pivot at forward edge (+Z), panel extends towards -Z
            Vector3 panelCenter = new Vector3(0f, InteriorConfig.EntryDoorHeight * 0.5f, -halfW);

            // Heavy beveled outer frame
            MeshUtil.CreateBoxObject("Frame",
                new Vector3(0.12f, InteriorConfig.EntryDoorHeight + 0.12f, InteriorConfig.EntryDoorWidth + 0.12f),
                MaterialLibrary.WallShadow, hingeRoot.transform, panelCenter, false);

            // Main heavy door panel
            GameObject doorPanel = MeshUtil.CreateBoxObject("DoorPanel",
                new Vector3(InteriorConfig.EntryDoorThickness, InteriorConfig.EntryDoorHeight, InteriorConfig.EntryDoorWidth),
                MaterialLibrary.GalleyCabinet, hingeRoot.transform, panelCenter, true);

            // Small round/octagonal window
            Mesh roundWin = MeshUtil.CreatePrism(8, InteriorConfig.EntryDoorWindowRadius, 0.10f);
            GameObject win = MeshUtil.CreateMeshObject("RoundWindow", roundWin, MaterialLibrary.WindowGlass, doorPanel.transform);
            win.transform.localPosition = new Vector3(0f, 0.38f, 0f);
            win.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

            // Heavy handle lever
            float leverDir = isLeft ? 0.06f : -0.06f;
            MeshUtil.CreateBoxObject("HandleLever",
                new Vector3(0.05f, 0.07f, InteriorConfig.EntryDoorHandleLength),
                MaterialLibrary.GalleyTrolley, doorPanel.transform, new Vector3(leverDir, -0.05f, 0.08f));

            // Accent latch block
            MeshUtil.CreateBoxObject("LatchAccent",
                new Vector3(0.06f, 0.08f, 0.08f),
                MaterialLibrary.GalleyAccent, doorPanel.transform, new Vector3(leverDir, -0.05f, -0.15f));

            return hingeRoot;
        }
    }
}
