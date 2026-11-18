using UnityEngine;

namespace AirplaneBuilder
{
    public static class InteriorConfig
    {
        // Fuselage Dimensions
        public const float FuselageWidth = 4.2f;
        public const float FuselageHalfWidth = 2.1f;
        public const float FuselageHeight = 2.6f;
        public const float FloorY = 0.0f;

        // Section Z Boundaries (Nose = +Z, Tail = -Z)
        public const float ZCockpitFront = 1.4f;
        public const float ZCockpitRear = -1.4f;
        public const float ZFrontGalleyRear = -4.9f;
        public const float ZFrontLavRear = -6.5f;
        public const float ZCabinRear = -23.5f;
        public const float ZRearLavRear = -25.1f;
        public const float ZRearGalleyRear = -28.6f; // Tail Wall

        // Doorway standards (Connecting openings)
        public const float DoorwayWidth = 0.92f;
        public const float DoorwayHeight = 2.05f;

        // Aisle & Floor
        public const float AisleWidth = 0.9f;
        public const float AisleHalfWidth = 0.45f;
        public const float AisleStripeWidth = 0.035f;
        public const float AisleStripeLeftX = -0.45f;
        public const float AisleStripeRightX = 0.45f;

        // Cabin Rows & Seating
        public const int CabinRowCount = 20;
        public const float CabinRowPitch = 0.85f;
        public const float SeatBlockWidth = 1.5f;
        public const float SingleSeatWidth = 0.45f;
        public const int RedRowStart = 1;
        public const int RedRowEnd = 4;
        public const int BlueRowStart = 5;
        public const int BlueRowEnd = 10;
        public const int IndigoRowStart = 11;
        public const int IndigoRowEnd = 20;
        public const float ExitDoorZ = -12.0f;

        // Individual Seat Dimensions
        public const float SeatCushionWidth = 0.40f;
        public const float SeatCushionLength = 0.44f;
        public const float SeatCushionHeight = 0.10f;
        public const float SeatCushionTopY = 0.44f;
        public const float SeatBackrestHeight = 0.62f;
        public const float SeatBackrestThickness = 0.08f;
        public const float SeatHeadrestWidth = 0.28f;
        public const float SeatHeadrestHeight = 0.16f;
        public const float SeatHeadrestThickness = 0.09f;
        public const float SeatArmrestWidth = 0.045f;
        public const float SeatArmrestHeight = 0.05f;
        public const float SeatArmrestLength = 0.34f;
        public const float SeatArmrestY = 0.58f;
        public const float SeatPedestalWidth = 0.08f;
        public const float SeatPedestalHeight = 0.34f;

        // Windows
        public const float WindowWidth = 0.32f;
        public const float WindowHeight = 0.48f;
        public const float WindowCenterY = 1.35f;
        public const float WindowDepth = 0.06f;
        public const float WindowCornerCut = 0.07f;

        // Overhead Bins
        public const float BinBottomY = 1.78f;
        public const float BinTopY = 2.26f;
        public const float BinDepth = 0.48f;
        public const float BinWallThickness = 0.03f;

        // Exit Doors (Over-wing)
        public const float ExitDoorWidth = 0.72f;
        public const float ExitDoorHeight = 1.45f;
        public const float ExitDoorCenterY = 1.25f;
        public const float ExitDoorFrameThickness = 0.06f;

        // Galley Dimensions & Props
        public const float EntryDoorWidth = 0.85f;
        public const float EntryDoorHeight = 1.95f;
        public const float EntryDoorThickness = 0.08f;
        public const float EntryDoorWindowRadius = 0.12f;
        public const float EntryDoorHandleLength = 0.26f;

        public const float GalleyCounterHeight = 0.95f;
        public const float GalleyCounterDepth = 0.70f;
        public const float GalleyCabinetWidth = 1.35f;
        public const float GalleyCabinetHeight = 2.15f;

        public const float TrolleyWidth = 0.30f;
        public const float TrolleyHeight = 0.86f;
        public const float TrolleyDepth = 0.62f;

        public const float MicrowaveWidth = 0.42f;
        public const float MicrowaveHeight = 0.26f;
        public const float MicrowaveDepth = 0.38f;

        public const float OvenWidth = 0.42f;
        public const float OvenHeight = 0.32f;
        public const float OvenDepth = 0.38f;

        public const float CupRadius = 0.038f;
        public const float CupHeight = 0.09f;

        // Cockpit Layout & Components
        public const float CockpitWidth = 2.6f;
        public const float CockpitLength = 2.8f;
        public const float PilotSeatX = 0.65f;
        public const float PilotSeatZ = -0.15f;
        public const float JumpSeatX = -0.48f;
        public const float JumpSeatZ = -1.05f;

        public const float CockpitPanelWidth = 2.2f;
        public const float CockpitPanelHeight = 0.75f;
        public const float CockpitPanelY = 0.68f;
        public const float CockpitPanelZ = 0.82f;
        public const float GlareshieldDepth = 0.28f;

        public const float HorizonScreenWidth = 0.16f;
        public const float HorizonScreenHeight = 0.14f;
        public const float CenterPedestalWidth = 0.36f;
        public const float CenterPedestalLength = 0.70f;
        public const float CenterPedestalHeight = 0.56f;
        public const float CenterPedestalZ = 0.20f;

        public const float OverheadPanelWidth = 0.65f;
        public const float OverheadPanelLength = 0.85f;
        public const float OverheadPanelY = 2.25f;
        public const float OverheadPanelZ = 0.25f;

        public const float WindshieldZ = 1.32f;
        public const float WindshieldY = 1.58f;
        public const float WindshieldWidth = 2.2f;
        public const float WindshieldHeight = 0.76f;
        public const float PillarThickness = 0.08f;

        // Ceiling Lights
        public const float CeilingLightWidth = 0.38f;
        public const float CeilingLightThickness = 0.025f;
        public const float CeilingLightY = 2.58f;
        public const float GalleyLightSize = 0.25f;

        // Lavatory
        public const float LavatoryWidth = 1.4f;
        public const float LavatoryLength = 1.6f;
        public const float LavatoryHeight = 2.3f;

        // Material Smoothness & Specular
        public const float DefaultSmoothness = 0.1f;
        public const float DefaultMetallic = 0.0f;

        // Colors (Hex format)
        public static readonly Color ColorWall = Hex("#D9D3B8");
        public static readonly Color ColorWallShadow = Hex("#B5AF95");
        public static readonly Color ColorCeilingLight = Hex("#FFF3C4");
        public static readonly Color ColorFloor = Hex("#5A5A5E");
        public static readonly Color ColorAisleStripe = Hex("#FFFFFF");

        public static readonly Color ColorSeatRed = Hex("#A3123A");
        public static readonly Color ColorSeatRedDark = Hex("#7A0E2C");
        public static readonly Color ColorSeatBlue = Hex("#3F7FB8");
        public static readonly Color ColorSeatIndigo = Hex("#3A2F8F");

        public static readonly Color ColorGalleyCabinet = Hex("#BDBDB5");
        public static readonly Color ColorGalleyTrolley = Hex("#3A3A3E");
        public static readonly Color ColorGalleyAccent = Hex("#8A1530");

        public static readonly Color ColorCockpitWall = Hex("#2B3340");
        public static readonly Color ColorCockpitPanel = Hex("#4A505C");
        public static readonly Color ColorCockpitSeat = Hex("#151518");

        public static readonly Color ColorButtonRed = Hex("#FF3333");
        public static readonly Color ColorButtonGreen = Hex("#33CC44");
        public static readonly Color ColorButtonOrange = Hex("#FF8800");
        public static readonly Color ColorButtonBlue = Hex("#2288EE");
        public static readonly Color ColorButtonYellow = Hex("#FFDD22");

        public static readonly Color ColorHorizonOrange = Hex("#FF7700");
        public static readonly Color ColorHorizonBlue = Hex("#1E88E5");

        public static readonly Color ColorToilet = Hex("#8E9396");
        public static readonly Color ColorSink = Hex("#CFCFC8");
        public static readonly Color ColorMirror = Hex("#A9C3DB");
        public static readonly Color ColorIndicatorGreen = Hex("#00E676");
        public static readonly Color ColorIndicatorRed = Hex("#FF1744");
        public static readonly Color ColorWindowGlass = Hex("#1A222E");

        public static Color Hex(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
                return color;
            return Color.white;
        }
    }
}
