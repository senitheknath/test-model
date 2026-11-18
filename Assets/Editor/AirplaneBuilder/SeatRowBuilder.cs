using UnityEngine;

namespace AirplaneBuilder
{
    public static class SeatRowBuilder
    {
        public static float GetRowZ(int rowNumber)
        {
            if (rowNumber <= 10)
            {
                // Rows 1-10: before mid-bulkhead
                return -6.50f - (rowNumber - 0.5f) * InteriorConfig.CabinRowPitch;
            }
            else
            {
                // Rows 11-17: after mid-bulkhead (-16.70)
                int indexAfterBulkhead = rowNumber - 11;
                return -16.70f - (indexAfterBulkhead + 0.5f) * InteriorConfig.CabinRowPitch;
            }
        }

        public static CabinSeatColor GetRowColor(int rowNumber)
        {
            if (rowNumber <= InteriorConfig.RedRowEnd)
                return CabinSeatColor.Red;
            if (rowNumber <= InteriorConfig.BlueRowEnd)
                return CabinSeatColor.Blue;
            return CabinSeatColor.Indigo;
        }

        public static GameObject BuildRow(int rowNumber, Transform parent)
        {
            string rowName = $"Row_{rowNumber:D2}";
            GameObject rowObj = new GameObject(rowName);
            if (parent != null) rowObj.transform.SetParent(parent, false);

            float rowZ = GetRowZ(rowNumber);
            rowObj.transform.localPosition = new Vector3(0f, InteriorConfig.FloorY, rowZ);

            CabinSeatColor color = GetRowColor(rowNumber);
            string colorName = color.ToString();

            // Left Triple (X < 0): Window (L1), Middle (L2), Aisle (L3)
            BuildSeatInRow(rowObj.transform, $"Seat_{colorName}_R{rowNumber:D2}_L1", color, 1, true, -InteriorConfig.SeatCenterWindowX);
            BuildSeatInRow(rowObj.transform, $"Seat_{colorName}_R{rowNumber:D2}_L2", color, 2, true, -InteriorConfig.SeatCenterMiddleX);
            BuildSeatInRow(rowObj.transform, $"Seat_{colorName}_R{rowNumber:D2}_L3", color, 3, true, -InteriorConfig.SeatCenterAisleX);

            // Right Triple (X > 0): Aisle (R3), Middle (R2), Window (R1)
            BuildSeatInRow(rowObj.transform, $"Seat_{colorName}_R{rowNumber:D2}_R3", color, 3, false, InteriorConfig.SeatCenterAisleX);
            BuildSeatInRow(rowObj.transform, $"Seat_{colorName}_R{rowNumber:D2}_R2", color, 2, false, InteriorConfig.SeatCenterMiddleX);
            BuildSeatInRow(rowObj.transform, $"Seat_{colorName}_R{rowNumber:D2}_R1", color, 1, false, InteriorConfig.SeatCenterWindowX);

            return rowObj;
        }

        private static void BuildSeatInRow(Transform rowParent, string seatName, CabinSeatColor color, int index, bool isLeft, float localX)
        {
            GameObject seat = SeatMeshBuilder.CreateSeat(seatName, color, index, isLeft, rowParent);
            seat.transform.localPosition = new Vector3(localX, 0f, 0f);
            seat.transform.localRotation = Quaternion.identity;
        }

        public static void BuildAllRows(Transform parent)
        {
            for (int r = 1; r <= InteriorConfig.CabinRowCount; r++)
            {
                BuildRow(r, parent);
            }
        }
    }
}
