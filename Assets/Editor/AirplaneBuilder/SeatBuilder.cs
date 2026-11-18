using System.IO;
using UnityEditor;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class SeatBuilder
    {
        private const string PrefabDir = "Assets/Prefabs";

        public static GameObject CreateSeatModel(string name, Material mainMat, Material darkMat)
        {
            GameObject root = new GameObject(name);

            // 1. Cushion
            MeshUtil.CreateBoxObject("Cushion",
                new Vector3(InteriorConfig.SeatCushionWidth, InteriorConfig.SeatCushionHeight, InteriorConfig.SeatCushionLength),
                mainMat, root.transform,
                new Vector3(0f, InteriorConfig.SeatCushionTopY - InteriorConfig.SeatCushionHeight * 0.5f, 0f));

            // 2. Angular Backrest
            GameObject backrest = MeshUtil.CreateBoxObject("Backrest",
                new Vector3(InteriorConfig.SeatCushionWidth - 0.02f, InteriorConfig.SeatBackrestHeight, InteriorConfig.SeatBackrestThickness),
                mainMat, root.transform,
                new Vector3(0f, InteriorConfig.SeatCushionTopY + InteriorConfig.SeatBackrestHeight * 0.45f, 0.16f));
            backrest.transform.localRotation = Quaternion.Euler(8f, 0f, 0f);

            // 3. Headrest block
            MeshUtil.CreateBoxObject("Headrest",
                new Vector3(InteriorConfig.SeatHeadrestWidth, InteriorConfig.SeatHeadrestHeight, InteriorConfig.SeatHeadrestThickness),
                darkMat, backrest.transform,
                new Vector3(0f, InteriorConfig.SeatBackrestHeight * 0.48f, 0.02f));

            // 4. Center Pedestal
            MeshUtil.CreateBoxObject("Pedestal",
                new Vector3(InteriorConfig.SeatPedestalWidth, InteriorConfig.SeatPedestalHeight, 0.32f),
                darkMat, root.transform,
                new Vector3(0f, InteriorConfig.SeatPedestalHeight * 0.5f, 0.02f));

            // 5. Left Armrest
            float armX = InteriorConfig.SeatCushionWidth * 0.5f + InteriorConfig.SeatArmrestWidth * 0.5f;
            MeshUtil.CreateBoxObject("Armrest_L",
                new Vector3(InteriorConfig.SeatArmrestWidth, InteriorConfig.SeatArmrestHeight, InteriorConfig.SeatArmrestLength),
                darkMat, root.transform,
                new Vector3(-armX, InteriorConfig.SeatArmrestY, 0f));

            // 6. Right Armrest
            MeshUtil.CreateBoxObject("Armrest_R",
                new Vector3(InteriorConfig.SeatArmrestWidth, InteriorConfig.SeatArmrestHeight, InteriorConfig.SeatArmrestLength),
                darkMat, root.transform,
                new Vector3(armX, InteriorConfig.SeatArmrestY, 0f));

            // 7. Box Collider for the seat
            BoxCollider col = root.AddComponent<BoxCollider>();
            col.center = new Vector3(0f, 0.55f, 0.05f);
            col.size = new Vector3(InteriorConfig.SingleSeatWidth, 1.1f, 0.52f);

            return root;
        }

        public static GameObject GetOrCreateSeatPrefab(string prefabName, Material mainMat, Material darkMat)
        {
            if (!Directory.Exists(PrefabDir))
            {
                Directory.CreateDirectory(PrefabDir);
                AssetDatabase.Refresh();
            }

            string path = $"{PrefabDir}/{prefabName}.prefab";
            GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing != null)
                return existing;

            GameObject temp = CreateSeatModel(prefabName, mainMat, darkMat);
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(temp, path);
            Object.DestroyImmediate(temp);
            return prefab;
        }

        public static void BuildAllSeats(Transform seatsParent)
        {
            GameObject redPrefab = GetOrCreateSeatPrefab("Seat_Red", MaterialLibrary.SeatRed, MaterialLibrary.SeatRedDark);
            GameObject bluePrefab = GetOrCreateSeatPrefab("Seat_Blue", MaterialLibrary.SeatBlue, MaterialLibrary.GalleyTrolley);
            GameObject indigoPrefab = GetOrCreateSeatPrefab("Seat_Indigo", MaterialLibrary.SeatIndigo, MaterialLibrary.GalleyTrolley);

            int redIndex = 1;
            int blueIndex = 1;
            int indigoIndex = 1;

            float[] leftXOffsets = new float[] { -0.675f, -1.125f, -1.575f };
            float[] rightXOffsets = new float[] { 0.675f, 1.125f, 1.575f };

            for (int r = 0; r < InteriorConfig.CabinRowCount; r++)
            {
                int rowNumber = r + 1;
                float rowZ = InteriorConfig.ZFrontLavRear - (r + 0.5f) * InteriorConfig.CabinRowPitch;

                GameObject prefabToUse;
                string namePrefix;
                int currentCounter;

                if (rowNumber <= InteriorConfig.RedRowEnd)
                {
                    prefabToUse = redPrefab;
                    namePrefix = "Seat_Red";
                    currentCounter = redIndex;
                }
                else if (rowNumber <= InteriorConfig.BlueRowEnd)
                {
                    prefabToUse = bluePrefab;
                    namePrefix = "Seat_Blue";
                    currentCounter = blueIndex;
                }
                else
                {
                    prefabToUse = indigoPrefab;
                    namePrefix = "Seat_Indigo";
                    currentCounter = indigoIndex;
                }

                // Left 3 seats
                for (int s = 0; s < 3; s++)
                {
                    string seatName = $"{namePrefix}_{currentCounter:D2}";
                    GameObject seat = Object.Instantiate(prefabToUse, seatsParent);
                    seat.name = seatName;
                    seat.transform.localPosition = new Vector3(leftXOffsets[s], InteriorConfig.FloorY, rowZ);
                    seat.transform.localRotation = Quaternion.identity;
                    currentCounter++;
                }

                // Right 3 seats
                for (int s = 0; s < 3; s++)
                {
                    string seatName = $"{namePrefix}_{currentCounter:D2}";
                    GameObject seat = Object.Instantiate(prefabToUse, seatsParent);
                    seat.name = seatName;
                    seat.transform.localPosition = new Vector3(rightXOffsets[s], InteriorConfig.FloorY, rowZ);
                    seat.transform.localRotation = Quaternion.identity;
                    currentCounter++;
                }

                if (rowNumber <= InteriorConfig.RedRowEnd)
                    redIndex = currentCounter;
                else if (rowNumber <= InteriorConfig.BlueRowEnd)
                    blueIndex = currentCounter;
                else
                    indigoIndex = currentCounter;
            }
        }
    }
}
