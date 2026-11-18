using UnityEngine;

namespace AirplaneBuilder
{
    public static class CabinBuilder
    {
        public static void Build(Transform parentRoot)
        {
            GameObject cabinRoot = new GameObject("Cabin");
            cabinRoot.transform.SetParent(parentRoot, false);

            GameObject seatsGroup = new GameObject("Seats");
            seatsGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject binsGroup = new GameObject("Bins");
            binsGroup.transform.SetParent(cabinRoot.transform, false);

            GameObject doorsGroup = new GameObject("Doors");
            doorsGroup.transform.SetParent(cabinRoot.transform, false);

            // 1. Build all 20 rows of seats
            SeatBuilder.BuildAllSeats(seatsGroup.transform);

            // 2. Build open-front overhead bins above each row
            BuildOverheadBins(binsGroup.transform);

            // 3. Build over-wing exit doors
            BuildExitDoors(doorsGroup.transform);
        }

        private static void BuildOverheadBins(Transform parent)
        {
            for (int r = 0; r < InteriorConfig.CabinRowCount; r++)
            {
                float rowZ = InteriorConfig.ZFrontLavRear - (r + 0.5f) * InteriorConfig.CabinRowPitch;
                int rowNum = r + 1;

                // Left Overhead Bin Unit
                CreateBinCompartment(parent, -1.68f, rowZ, true, rowNum);

                // Right Overhead Bin Unit
                CreateBinCompartment(parent, 1.68f, rowZ, false, rowNum);
            }
        }

        private static void CreateBinCompartment(Transform parent, float posX, float posZ, bool isLeft, int rowNum)
        {
            GameObject bin = new GameObject($"Bin_{(isLeft ? "L" : "R")}_{rowNum:D2}");
            bin.transform.SetParent(parent, false);
            bin.transform.localPosition = new Vector3(posX, InteriorConfig.BinBottomY, posZ);

            float compLen = InteriorConfig.CabinRowPitch - 0.04f;
            float binHeight = InteriorConfig.BinTopY - InteriorConfig.BinBottomY;

            // Bottom shelf
            MeshUtil.CreateBoxObject("Shelf_Bottom",
                new Vector3(InteriorConfig.BinDepth, InteriorConfig.BinWallThickness, compLen),
                MaterialLibrary.WallShadow, bin.transform,
                new Vector3(0f, 0f, 0f));

            // Top shelf / roof
            MeshUtil.CreateBoxObject("Shelf_Top",
                new Vector3(InteriorConfig.BinDepth, InteriorConfig.BinWallThickness, compLen),
                MaterialLibrary.Wall, bin.transform,
                new Vector3(0f, binHeight, 0f));

            // Outer Back Wall (against fuselage)
            float backWallX = isLeft ? -InteriorConfig.BinDepth * 0.5f : InteriorConfig.BinDepth * 0.5f;
            MeshUtil.CreateBoxObject("Wall_Back",
                new Vector3(InteriorConfig.BinWallThickness, binHeight, compLen),
                MaterialLibrary.Wall, bin.transform,
                new Vector3(backWallX, binHeight * 0.5f, 0f));

            // Forward Divider Plate
            MeshUtil.CreateBoxObject("Divider_Fwd",
                new Vector3(InteriorConfig.BinDepth, binHeight, InteriorConfig.BinWallThickness),
                MaterialLibrary.Wall, bin.transform,
                new Vector3(0f, binHeight * 0.5f, compLen * 0.5f));

            // Rear Divider Plate
            MeshUtil.CreateBoxObject("Divider_Rear",
                new Vector3(InteriorConfig.BinDepth, binHeight, InteriorConfig.BinWallThickness),
                MaterialLibrary.Wall, bin.transform,
                new Vector3(0f, binHeight * 0.5f, -compLen * 0.5f));

            // Front Lip (bottom ledge so items don't fall out, front remains OPEN)
            float lipX = isLeft ? InteriorConfig.BinDepth * 0.5f : -InteriorConfig.BinDepth * 0.5f;
            MeshUtil.CreateBoxObject("Lip_Front",
                new Vector3(InteriorConfig.BinWallThickness, 0.06f, compLen),
                MaterialLibrary.Wall, bin.transform,
                new Vector3(lipX, 0.03f, 0f));

            // Box Collider on bin top for player collisions
            BoxCollider col = bin.AddComponent<BoxCollider>();
            col.center = new Vector3(0f, binHeight * 0.5f, 0f);
            col.size = new Vector3(InteriorConfig.BinDepth, binHeight, compLen);
        }

        private static void BuildExitDoors(Transform parent)
        {
            float hingeForwardZ = InteriorConfig.ExitDoorZ + InteriorConfig.ExitDoorWidth * 0.5f;

            // Left Exit Door
            DoorBuilder.CreateExitDoor("Door_Exit_L", parent, true,
                new Vector3(-2.06f, InteriorConfig.ExitDoorCenterY, hingeForwardZ));

            // Right Exit Door
            DoorBuilder.CreateExitDoor("Door_Exit_R", parent, false,
                new Vector3(2.06f, InteriorConfig.ExitDoorCenterY, hingeForwardZ));
        }
    }
}
