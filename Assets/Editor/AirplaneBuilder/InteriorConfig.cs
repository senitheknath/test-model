using UnityEngine;

namespace AirplaneBuilder
{
    public static class InteriorConfig
    {
        // Pilots visibility toggle
        public const bool ShowPilots = true;
        public const bool ShowPassengers = true;

        // Fuselage Dimensions
        public const float FuselageWidth = 4.2f;
        public const float FuselageHalfWidth = 2.1f;
        public const float FuselageHeight = 2.6f;
        public const float FloorY = 0.0f;

        public static readonly Vector2[] FuselageArchProfile = new Vector2[]
        {
            new Vector2(-2.05f, 0.00f), // 0: Floor left corner
            new Vector2(-2.10f, 0.55f), // 1: Lower wall
            new Vector2(-2.10f, 1.30f), // 2: Mid wall (window level)
            new Vector2(-1.95f, 1.85f), // 3: Upper wall / bin base
            new Vector2(-1.60f, 2.25f), // 4: Shoulder slope
            new Vector2(-0.90f, 2.52f), // 5: Ceiling slope
            new Vector2( 0.00f, 2.60f), // 6: Ceiling peak
            new Vector2( 0.90f, 2.52f), // 7: Right ceiling slope
            new Vector2( 1.60f, 2.25f), // 8: Right shoulder slope
            new Vector2( 1.95f, 1.85f), // 9: Right upper wall
            new Vector2( 2.10f, 1.30f), // 10: Right mid wall
            new Vector2( 2.10f, 0.55f), // 11: Right lower wall
            new Vector2( 2.05f, 0.00f)  // 12: Floor right corner
        };

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
        public const float AisleStripeWidth = 0.04f;
        public const float AisleStripeLeftX = -0.45f;
        public const float AisleStripeRightX = 0.45f;

        // Cabin Rows & Seating (17 rows total + Mid Bulkhead + Aft Module)
        public const int CabinRowCount = 17;
        public const float CabinRowPitch = 0.85f;
        public const float CabinStartZ = -6.50f;
        public const float CabinEndZ = -23.50f;

        public const int RedRowStart = 1;
        public const int RedRowEnd = 4;
        public const int BlueRowStart = 5;
        public const int BlueRowEnd = 10;
        public const int IndigoRowStart = 11;
        public const int IndigoRowEnd = 17;

        public const float ZMidBulkheadStart = -15.00f;
        public const float ZMidBulkheadEnd = -16.70f;
        public const float ZAftModuleStart = -22.65f;
        public const float ZAftModuleEnd = -23.50f;

        // Seat Triple Coordinates per side
        public const float SeatWidth = 0.45f;
        public const float SeatCenterWindowX = 1.76f; // Index 1
        public const float SeatCenterMiddleX = 1.25f; // Index 2
        public const float SeatCenterAisleX = 0.74f;  // Index 3

        // Passenger Seat Detailed Dimensions
        public const float SeatCushionWidth = 0.45f;
        public const float SeatCushionLength = 0.46f;
        public const float SeatCushionHeight = 0.10f;
        public const float SeatCushionTopY = 0.45f;
        public const float SeatBackrestWidth = 0.45f;
        public const float SeatBackrestHeight = 0.55f;
        public const float SeatBackrestThickness = 0.11f;
        public const float SeatHeadrestWidth = 0.32f;
        public const float SeatHeadrestHeight = 0.18f;
        public const float SeatHeadrestThickness = 0.13f;
        public const float SeatArmrestWidth = 0.06f;
        public const float SeatArmrestHeight = 0.06f;
        public const float SeatArmrestLength = 0.42f;
        public const float SeatTrayWidth = 0.36f;
        public const float SeatTrayHeight = 0.22f;
        public const float SeatTrayThickness = 0.02f;
        public const float SeatPocketWidth = 0.36f;
        public const float SeatPocketHeight = 0.14f;
        public const float SeatPocketThickness = 0.05f;

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

        // Galley Dimensions & Coordinates
        public const float GalleySkinThickness = 0.25f;
        public const float GalleyFrontFaceZ = -1.65f;
        public const float GalleyFrontLength = 3.25f;
        public const float GalleyRearLength = 3.50f;
        public const float GalleyLinerX = 2.00f;
        public const float GalleyLinerThickness = 0.10f;
        public const float GalleyLinerHeight = 1.95f;
        public const float GalleyWalkwayWidth = 2.80f;

        // Legacy door aliases for DoorBuilder
        public const float EntryDoorWidth = 0.85f;
        public const float EntryDoorHeight = 1.95f;
        public const float EntryDoorThickness = 0.08f;
        public const float EntryDoorWindowRadius = 0.12f;
        public const float EntryDoorHandleLength = 0.26f;

        public const float GalleyDoorWidth = 0.75f;
        public const float GalleyDoorHeight = 2.00f;
        public const float GalleyClosetX = -0.95f;
        public const float GalleyClosetWidth = 0.70f;
        public const float GalleyClosetHeight = 2.00f;
        public const float GalleyClosetRecess = 0.20f;

        public const float GalleyEntryDoorLeafW = 1.00f;
        public const float GalleyEntryDoorLeafH = 1.90f;
        public const float GalleyEntryDoorWinDia = 0.22f;
        public const float GalleyEntryDoorHandleY = 0.95f;
        public const float GalleyEntryDoorWinY = 1.45f;
        public const float GalleyHandrailDia = 0.03f;
        public const float GalleyHandrailH = 0.90f;

        public const float GalleyJumpseatW = 0.38f;
        public const float GalleyJumpseatD = 0.38f;
        public const float GalleyJumpseatCushH = 0.07f;
        public const float GalleyJumpseatBackH = 0.55f;
        public const float GalleyJumpseatAlcoveW = 0.50f;
        public const float GalleyJumpseatAlcoveH = 0.70f;
        public const float GalleyJumpseatAlcoveD = 0.12f;

        // Lavatory Dimensions & Props
        public const float LavatoryWidth = 1.4f;
        public const float LavatoryLength = 1.6f;
        public const float LavatoryHeight = 2.3f;
        public const float LavDoorWidth = 0.68f;
        public const float LavDoorHeight = 1.95f;

        public const float ToiletRadius = 0.20f;
        public const float ToiletHeight = 0.42f;
        public const float ToiletSeatThickness = 0.04f;
        public const float ToiletLidHeight = 0.40f;

        public const float SinkBasinWidth = 0.44f;
        public const float SinkBasinLength = 0.36f;
        public const float SinkHeight = 0.80f;

        public const float MirrorWidth = 0.38f;
        public const float MirrorHeight = 0.56f;
        public const float TowelBoxWidth = 0.28f;
        public const float TowelBoxHeight = 0.30f;
        public const float WasteFlapWidth = 0.25f;
        public const float WasteFlapHeight = 0.32f;
        public const float HandrailLength = 0.44f;

        // Cockpit Footprint & Layout
        public const float CockpitWidth = 2.6f;
        public const float CockpitHalfWidth = 1.3f;
        public const float CockpitLength = 2.8f;
        public const float CockpitHeight = 2.3f;
        public const float CockpitCeilingWidth = 1.0f;

        // Cockpit Windshield & Windows
        public const float WindshieldSillY = 1.30f;
        public const float WindshieldTopY = 2.05f;
        public const float WindshieldPillarWidth = 0.10f;
        public const float WindshieldCenterHalfWidth = 0.40f;
        public const float StandbyCompassSize = 0.09f;

        public const float SideWindowX = 1.30f;
        public const float SideWindowZStart = -0.10f;
        public const float SideWindowZEnd = 0.85f;
        public const float SideWindowYBottom = 1.20f;
        public const float SideWindowYTop = 1.95f;

        // Cockpit Glareshield & Autopilot
        public const float GlareshieldHalfWidth = 1.15f;
        public const float GlareshieldY = 1.30f;
        public const float GlareshieldZStart = 0.75f;
        public const float GlareshieldZEnd = 1.20f;
        public const float GlareshieldThickness = 0.07f;
        public const float AutopilotHalfWidth = 0.45f;
        public const float AutopilotYBottom = 1.37f;
        public const float AutopilotYTop = 1.45f;
        public const float AutopilotZStart = 0.95f;
        public const float AutopilotZEnd = 1.05f;

        // Main Instrument Panel
        public const float MainPanelHalfWidth = 1.10f;
        public const float MainPanelYBottom = 0.72f;
        public const float MainPanelYTop = 1.30f;
        public const float MainPanelZ = 0.78f;
        public const float MainPanelTiltDeg = 12f;
        public const float MainPanelThickness = 0.12f;

        // Center Pedestal
        public const float PedestalHalfWidth = 0.17f;
        public const float PedestalZFront = 0.82f;
        public const float PedestalZRear = -0.25f;
        public const float PedestalTopY = 0.78f;
        public const float OctagonalDomeDia = 0.34f;
        public const float OctagonalDomeHeight = 0.17f;

        // Yokes
        public const float YokeX = 0.55f;
        public const float YokeHubY = 1.00f;
        public const float YokeHubZ = 0.30f;
        public const float YokeBarWidth = 0.38f;
        public const float YokeGripHeight = 0.12f;

        // Cockpit Seats & Pilots
        public const float PilotSeatX = 0.55f;
        public const float PilotSeatZ = -0.15f;
        public const float PilotSeatCushionTopY = 0.48f;
        public const float PilotSeatPedestalSize = 0.30f;
        public const float PilotSeatPedestalHeight = 0.40f;
        public const float PilotSeatCushionW = 0.50f;
        public const float PilotSeatCushionD = 0.50f;
        public const float PilotSeatCushionH = 0.14f;
        public const float PilotSeatBackrestW = 0.52f;
        public const float PilotSeatBackrestH = 0.72f;
        public const float PilotSeatBackrestD = 0.12f;
        public const float PilotSeatHeadrestW = 0.40f;
        public const float PilotSeatHeadrestH = 0.28f;
        public const float PilotSeatHeadrestD = 0.14f;

        // Pilot Figures
        public const float PilotHeadSize = 0.20f;
        public const float PilotTorsoW = 0.42f;
        public const float PilotTorsoH = 0.50f;
        public const float PilotTorsoD = 0.24f;

        // Overhead Panel
        public const float OverheadHalfWidth = 0.55f;
        public const float OverheadZStart = 0.25f;
        public const float OverheadZEnd = 1.05f;
        public const float OverheadDrop = 0.12f;

        // Side Consoles
        public const float ConsoleXMin = 1.05f;
        public const float ConsoleXMax = 1.30f;
        public const float ConsoleYMin = 0.55f;
        public const float ConsoleYMax = 0.85f;
        public const float ConsoleZStart = -0.45f;
        public const float ConsoleZEnd = 0.55f;

        // Rear Wall & Door
        public const float RearDoorW = 0.75f;
        public const float RearDoorH = 2.00f;
        public const float RearDoorWindowSize = 0.14f;
        public const float RearDoorWindowY = 1.55f;
        public const float ExtinguisherDia = 0.11f;
        public const float ExtinguisherH = 0.33f;
        public const float BreakerPanelW = 0.60f;
        public const float BreakerPanelH = 0.55f;

        // Ceiling Lights
        public const float CeilingLightWidth = 0.38f;
        public const float CeilingLightThickness = 0.025f;
        public const float CeilingLightY = 2.58f;
        public const float GalleyLightSize = 0.25f;

        // Material Smoothness & Specular
        public const float DefaultSmoothness = 0.1f;
        public const float DefaultMetallic = 0.0f;

        // Base Colors
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

        public static readonly Color ColorToilet = Hex("#8E9396");
        public static readonly Color ColorSink = Hex("#CFCFC8");
        public static readonly Color ColorMirror = Hex("#A9C3DB");
        public static readonly Color ColorIndicatorGreen = Hex("#00E676");
        public static readonly Color ColorIndicatorRed = Hex("#FF1744");
        public static readonly Color ColorWindowGlass = Hex("#1A222E");

        // Target Cockpit Specific Color Palette
        public static readonly Color CockpitWallLit = Hex("#3B4454");
        public static readonly Color CockpitWallShadow = Hex("#2D3542");
        public static readonly Color CockpitFrame = Hex("#4A5361");
        public static readonly Color CockpitPanelBody = Hex("#4A515E");
        public static readonly Color CockpitGlareshieldTop = Hex("#2A2F39");
        public static readonly Color CockpitPedestalBody = Hex("#565E6C");
        public static readonly Color CockpitPedestalTop = Hex("#3A404B");
        public static readonly Color CockpitOctagonalDome = Hex("#647084");
        public static readonly Color CockpitOverhead = Hex("#3E4553");
        public static readonly Color CockpitDarkInsert = Hex("#20242C");
        public static readonly Color CockpitFloor = Hex("#1F232A");
        public static readonly Color CockpitCeilingFacet = Hex("#323A47");
        public static readonly Color CockpitNightSky = Hex("#0B111B");
        public static readonly Color CockpitHorizonHaze = Hex("#172234");
        public static readonly Color CockpitDialFace = Hex("#B09283");
        public static readonly Color CockpitDialBezel = Hex("#2C323B");
        public static readonly Color CockpitScreenOff = Hex("#14171C");
        public static readonly Color CockpitPedestalScreen = Hex("#17201C");
        public static readonly Color CockpitHorizonBlue = Hex("#5E86AE");
        public static readonly Color CockpitHorizonOrange = Hex("#C97B3C");

        // Button Colors (Emissive)
        public static readonly Color CockpitBtnRed = Hex("#D2352F");
        public static readonly Color CockpitBtnGreen = Hex("#3FD17A");
        public static readonly Color CockpitBtnAmber = Hex("#E7A13A");
        public static readonly Color CockpitBtnYellow = Hex("#E6C84A");
        public static readonly Color CockpitBtnBlue = Hex("#4A7FD0");
        public static readonly Color CockpitBtnWhite = Hex("#E6E6E0");

        // Cockpit Seats & Props Colors
        public static readonly Color CockpitSeatCharcoal = Hex("#17191E");
        public static readonly Color CockpitSeatEdge = Hex("#22252B");
        public static readonly Color CockpitYoke = Hex("#1B1D22");
        public static readonly Color CockpitDoor = Hex("#9AA1AA");
        public static readonly Color CockpitDoorFrame = Hex("#5A616C");
        public static readonly Color CockpitDoorWindow = Hex("#12161C");
        public static readonly Color CockpitExtinguisher = Hex("#C42B2B");
        public static readonly Color CockpitBracket = Hex("#444A52");
        public static readonly Color CockpitClipboard = Hex("#CFCFCB");

        // Pilot Figure Colors
        public static readonly Color PilotCaptSkin = Hex("#C99B77");
        public static readonly Color PilotCaptHair = Hex("#6A4630");
        public static readonly Color PilotFoSkin = Hex("#A57A5A");
        public static readonly Color PilotFoHair = Hex("#1E1A18");
        public static readonly Color PilotUniform = Hex("#2A3342");

        // Target Galley Specific Color Palette
        public static readonly Color GalleyWallLit = Hex("#CFCBBB");
        public static readonly Color GalleyWallShadow = Hex("#AEAA9A");
        public static readonly Color GalleyCeilingFacet = Hex("#BEBAA9");
        public static readonly Color GalleyWallTrim = Hex("#B5B1A1");
        public static readonly Color GalleyFloor = Hex("#3A3A3E");
        public static readonly Color GalleyCabinet = Hex("#BDBDB5");
        public static readonly Color GalleyDoorInset = Hex("#9C9C95");
        public static readonly Color GalleyCounterTop = Hex("#55555A");
        public static readonly Color GalleyCounterEdge = Hex("#8C8C88");
        public static readonly Color GalleyTrolley = Hex("#34343A");
        public static readonly Color GalleyOvenBody = Hex("#D8D6CE");
        public static readonly Color GalleyOvenBezel = Hex("#A9A9A3");
        public static readonly Color GalleyGlass = Hex("#20242C");
        public static readonly Color GalleyMaroon = Hex("#8A1530");
        public static readonly Color GalleyExitGlow = Hex("#B3213F");
        public static readonly Color GalleyDoorLeaf = Hex("#D8D4C4");
        public static readonly Color GalleyRedDecal = Hex("#A8283F");
        public static readonly Color GalleyJumpseatAlcove = Hex("#BDB9A8");
        public static readonly Color GalleyHandrail = Hex("#C9CBC4");

        public static Color Hex(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
                return color;
            return Color.white;
        }
    }
}
