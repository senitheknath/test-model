using UnityEngine;

namespace AirplaneBuilder
{
    public static class CockpitBuilder
    {
        public static void Build(Transform parentRoot)
        {
            GameObject cockpitRoot = new GameObject("Cockpit");
            cockpitRoot.transform.SetParent(parentRoot, false);

            GameObject seatsGroup = new GameObject("Seats");
            seatsGroup.transform.SetParent(cockpitRoot.transform, false);

            GameObject propsGroup = new GameObject("Props");
            propsGroup.transform.SetParent(cockpitRoot.transform, false);

            GameObject doorsGroup = new GameObject("Doors");
            doorsGroup.transform.SetParent(cockpitRoot.transform, false);

            BuildSeats(seatsGroup.transform);
            BuildWindshield(propsGroup.transform);
            BuildInstrumentPanel(propsGroup.transform);
            BuildCenterPedestal(propsGroup.transform);
            BuildOverheadPanel(propsGroup.transform);
            BuildYokes(propsGroup.transform);

            // Door_Cockpit hinged at left edge of doorway
            Vector3 hingePos = new Vector3(-InteriorConfig.DoorwayWidth * 0.5f, 0f, InteriorConfig.ZCockpitRear);
            DoorBuilder.CreateHingedDoor("Door_Cockpit",
                new Vector3(InteriorConfig.DoorwayWidth, InteriorConfig.DoorwayHeight, 0.05f),
                MaterialLibrary.Wall, MaterialLibrary.GalleyCabinet, doorsGroup.transform, hingePos, true);
        }

        private static void BuildSeats(Transform parent)
        {
            CreateCockpitSeat("Seat_Pilot_L", parent, new Vector3(-InteriorConfig.PilotSeatX, 0f, InteriorConfig.PilotSeatZ));
            CreateCockpitSeat("Seat_Pilot_R", parent, new Vector3(InteriorConfig.PilotSeatX, 0f, InteriorConfig.PilotSeatZ));
            CreateCockpitSeat("Seat_Jump", parent, new Vector3(InteriorConfig.JumpSeatX, 0f, InteriorConfig.JumpSeatZ), 0.85f);
        }

        private static void CreateCockpitSeat(string name, Transform parent, Vector3 localPos, float s = 1.0f)
        {
            GameObject seat = new GameObject(name);
            seat.transform.SetParent(parent, false);
            seat.transform.localPosition = localPos;
            Material mat = MaterialLibrary.CockpitSeat;

            MeshUtil.CreateBoxObject("Base", new Vector3(0.24f * s, 0.32f * s, 0.28f * s), MaterialLibrary.CockpitPanel, seat.transform, new Vector3(0f, 0.16f * s, 0f));
            MeshUtil.CreateBoxObject("Cushion", new Vector3(0.48f * s, 0.12f * s, 0.46f * s), mat, seat.transform, new Vector3(0f, 0.38f * s, 0f));

            GameObject backrest = MeshUtil.CreateBoxObject("Backrest", new Vector3(0.46f * s, 0.65f * s, 0.10f * s), mat, seat.transform, new Vector3(0f, 0.72f * s, 0.18f * s));
            backrest.transform.localRotation = Quaternion.Euler(6f, 0f, 0f);

            MeshUtil.CreateBoxObject("Headrest", new Vector3(0.30f * s, 0.18f * s, 0.10f * s), mat, backrest.transform, new Vector3(0f, 0.40f * s, 0.02f));

            float armX = 0.26f * s;
            MeshUtil.CreateBoxObject("Armrest_L", new Vector3(0.06f * s, 0.06f * s, 0.36f * s), mat, seat.transform, new Vector3(-armX, 0.52f * s, 0f));
            MeshUtil.CreateBoxObject("Armrest_R", new Vector3(0.06f * s, 0.06f * s, 0.36f * s), mat, seat.transform, new Vector3(armX, 0.52f * s, 0f));

            BoxCollider col = seat.AddComponent<BoxCollider>();
            col.center = new Vector3(0f, 0.58f * s, 0.05f * s);
            col.size = new Vector3(0.58f * s, 1.15f * s, 0.55f * s);
        }

        private static void BuildWindshield(Transform parent)
        {
            GameObject wsRoot = new GameObject("Windshield");
            wsRoot.transform.SetParent(parent, false);

            MeshUtil.CreateBoxObject("DarkSky", new Vector3(InteriorConfig.WindshieldWidth + 0.3f, InteriorConfig.WindshieldHeight + 0.3f, 0.02f),
                MaterialLibrary.CockpitSeat, wsRoot.transform, new Vector3(0f, InteriorConfig.WindshieldY, InteriorConfig.WindshieldZ + 0.06f));

            MeshUtil.CreateBoxObject("Glass_L", new Vector3(0.85f, InteriorConfig.WindshieldHeight - 0.06f, 0.02f),
                MaterialLibrary.WindowGlass, wsRoot.transform, new Vector3(-0.55f, InteriorConfig.WindshieldY, InteriorConfig.WindshieldZ));
            MeshUtil.CreateBoxObject("Glass_R", new Vector3(0.85f, InteriorConfig.WindshieldHeight - 0.06f, 0.02f),
                MaterialLibrary.WindowGlass, wsRoot.transform, new Vector3(0.55f, InteriorConfig.WindshieldY, InteriorConfig.WindshieldZ));

            Material pMat = MaterialLibrary.CockpitWall;
            float pt = InteriorConfig.PillarThickness;

            MeshUtil.CreateBoxObject("Pillar_Center", new Vector3(pt, InteriorConfig.WindshieldHeight, pt), pMat, wsRoot.transform, new Vector3(0f, InteriorConfig.WindshieldY, InteriorConfig.WindshieldZ - 0.01f), true);

            GameObject aL = MeshUtil.CreateBoxObject("Pillar_A_L", new Vector3(pt, InteriorConfig.WindshieldHeight + 0.1f, pt), pMat, wsRoot.transform, new Vector3(-1.05f, InteriorConfig.WindshieldY, InteriorConfig.WindshieldZ - 0.01f), true);
            aL.transform.localRotation = Quaternion.Euler(0f, 0f, -12f);

            GameObject aR = MeshUtil.CreateBoxObject("Pillar_A_R", new Vector3(pt, InteriorConfig.WindshieldHeight + 0.1f, pt), pMat, wsRoot.transform, new Vector3(1.05f, InteriorConfig.WindshieldY, InteriorConfig.WindshieldZ - 0.01f), true);
            aR.transform.localRotation = Quaternion.Euler(0f, 0f, 12f);

            MeshUtil.CreateBoxObject("Frame_Brow", new Vector3(InteriorConfig.WindshieldWidth, pt, pt), pMat, wsRoot.transform, new Vector3(0f, InteriorConfig.WindshieldY + InteriorConfig.WindshieldHeight * 0.5f, InteriorConfig.WindshieldZ), true);
            MeshUtil.CreateBoxObject("Frame_Sill", new Vector3(InteriorConfig.WindshieldWidth, pt, pt), pMat, wsRoot.transform, new Vector3(0f, InteriorConfig.WindshieldY - InteriorConfig.WindshieldHeight * 0.5f, InteriorConfig.WindshieldZ), true);
        }

        private static void BuildInstrumentPanel(Transform parent)
        {
            GameObject ip = new GameObject("InstrumentPanel");
            ip.transform.SetParent(parent, false);
            ip.transform.localPosition = new Vector3(0f, InteriorConfig.CockpitPanelY, InteriorConfig.CockpitPanelZ);

            MeshUtil.CreateBoxObject("Glareshield", new Vector3(InteriorConfig.CockpitPanelWidth, 0.06f, InteriorConfig.GlareshieldDepth),
                MaterialLibrary.CockpitWall, ip.transform, new Vector3(0f, InteriorConfig.CockpitPanelHeight * 0.5f + 0.03f, -InteriorConfig.GlareshieldDepth * 0.4f), true);

            GameObject dashboard = MeshUtil.CreateBoxObject("Dashboard", new Vector3(InteriorConfig.CockpitPanelWidth, InteriorConfig.CockpitPanelHeight, 0.12f),
                MaterialLibrary.CockpitPanel, ip.transform, Vector3.zero, true);
            dashboard.transform.localRotation = Quaternion.Euler(-10f, 0f, 0f);

            CreateFlightDisplay(dashboard.transform, -0.65f, "Display_Pilot_L");
            CreateFlightDisplay(dashboard.transform, 0.65f, "Display_Pilot_R");

            GameObject center = new GameObject("Center_MFD");
            center.transform.SetParent(dashboard.transform, false);
            center.transform.localPosition = new Vector3(0f, 0.05f, -0.065f);

            MeshUtil.CreateBoxObject("NavScreen", new Vector3(0.32f, 0.24f, 0.01f), MaterialLibrary.CockpitWall, center.transform, new Vector3(0f, 0.04f, 0f));
            CreateButtonCluster(center.transform, new Vector3(0f, 0.22f, 0f), 6, 1);
            CreateButtonCluster(center.transform, new Vector3(0f, -0.14f, 0f), 4, 2);
        }

        private static void CreateFlightDisplay(Transform parent, float posX, string name)
        {
            GameObject fwd = new GameObject(name);
            fwd.transform.SetParent(parent, false);
            fwd.transform.localPosition = new Vector3(posX, 0.08f, -0.065f);

            float hw = InteriorConfig.HorizonScreenWidth;
            float hh = InteriorConfig.HorizonScreenHeight * 0.5f;

            MeshUtil.CreateBoxObject("Horizon_Sky", new Vector3(hw, hh, 0.01f), MaterialLibrary.HorizonBlue, fwd.transform, new Vector3(0f, hh * 0.5f, 0f));
            MeshUtil.CreateBoxObject("Horizon_Ground", new Vector3(hw, hh, 0.01f), MaterialLibrary.HorizonOrange, fwd.transform, new Vector3(0f, -hh * 0.5f, 0f));

            float r = 0.045f;
            CreateRoundGauge(fwd.transform, new Vector3(-0.16f, 0.05f, 0f), r, "Gauge_Speed");
            CreateRoundGauge(fwd.transform, new Vector3(-0.16f, -0.05f, 0f), r, "Gauge_Attitude");
            CreateRoundGauge(fwd.transform, new Vector3(0.16f, 0.05f, 0f), r, "Gauge_Alt");
            CreateRoundGauge(fwd.transform, new Vector3(0.16f, -0.05f, 0f), r, "Gauge_VSI");

            CreateButtonCluster(fwd.transform, new Vector3(0f, -0.15f, 0f), 3, 2);
        }

        private static void CreateRoundGauge(Transform parent, Vector3 localPos, float radius, string name)
        {
            Mesh prism = MeshUtil.CreatePrism(8, radius, 0.015f);
            GameObject go = MeshUtil.CreateMeshObject(name, prism, MaterialLibrary.CockpitWall, parent);
            go.transform.localPosition = localPos;
            go.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            MeshUtil.CreateBoxObject("Pip", new Vector3(0.01f, 0.035f, 0.016f), MaterialLibrary.IndicatorGreen, go.transform, new Vector3(0f, 0f, 0.01f));
        }

        private static void CreateButtonCluster(Transform parent, Vector3 centerPos, int cols, int rows)
        {
            Material[] btnMats = new Material[]
            {
                MaterialLibrary.IndicatorRed, MaterialLibrary.IndicatorGreen,
                MaterialLibrary.HorizonOrange, MaterialLibrary.HorizonBlue, MaterialLibrary.CeilingLight
            };

            float sp = 0.032f;
            float sX = -(cols - 1) * sp * 0.5f;
            float sY = -(rows - 1) * sp * 0.5f;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Material m = btnMats[(r * cols + c) % btnMats.Length];
                    Vector3 pos = centerPos + new Vector3(sX + c * sp, sY + r * sp, -0.005f);
                    MeshUtil.CreateBoxObject($"Btn_{r}_{c}", new Vector3(0.018f, 0.018f, 0.015f), m, parent, pos);
                }
            }
        }

        private static void BuildCenterPedestal(Transform parent)
        {
            GameObject ped = MeshUtil.CreateBoxObject("CenterPedestal",
                new Vector3(InteriorConfig.CenterPedestalWidth, InteriorConfig.CenterPedestalHeight, InteriorConfig.CenterPedestalLength),
                MaterialLibrary.CockpitPanel, parent,
                new Vector3(0f, InteriorConfig.CenterPedestalHeight * 0.5f, InteriorConfig.CenterPedestalZ), true);

            MeshUtil.CreateBoxObject("QuadrantPlate",
                new Vector3(InteriorConfig.CenterPedestalWidth - 0.04f, 0.02f, InteriorConfig.CenterPedestalLength - 0.06f),
                MaterialLibrary.CockpitWall, ped.transform,
                new Vector3(0f, InteriorConfig.CenterPedestalHeight * 0.5f + 0.01f, 0f));

            CreateThrottleLever(ped.transform, new Vector3(-0.06f, InteriorConfig.CenterPedestalHeight * 0.5f + 0.08f, 0.05f), MaterialLibrary.IndicatorRed, "Throttle_L");
            CreateThrottleLever(ped.transform, new Vector3(0.06f, InteriorConfig.CenterPedestalHeight * 0.5f + 0.08f, 0.05f), MaterialLibrary.IndicatorRed, "Throttle_R");
            CreateThrottleLever(ped.transform, new Vector3(-0.13f, InteriorConfig.CenterPedestalHeight * 0.5f + 0.06f, 0.12f), MaterialLibrary.HorizonBlue, "Speedbrake");
            CreateThrottleLever(ped.transform, new Vector3(0.13f, InteriorConfig.CenterPedestalHeight * 0.5f + 0.06f, 0.12f), MaterialLibrary.HorizonBlue, "Flaps");

            MeshUtil.CreateBoxObject("TrimWheel", new Vector3(0.025f, 0.16f, 0.16f), MaterialLibrary.CockpitWall, ped.transform,
                new Vector3(-InteriorConfig.CenterPedestalWidth * 0.5f - 0.015f, 0.05f, 0.05f));
        }

        private static void CreateThrottleLever(Transform parent, Vector3 pos, Material knobMat, string name)
        {
            GameObject lever = new GameObject(name);
            lever.transform.SetParent(parent, false);
            lever.transform.localPosition = pos;

            MeshUtil.CreateBoxObject("Arm", new Vector3(0.018f, 0.10f, 0.018f), MaterialLibrary.CockpitSeat, lever.transform, Vector3.zero);
            MeshUtil.CreateBoxObject("Knob", new Vector3(0.045f, 0.045f, 0.05f), knobMat, lever.transform, new Vector3(0f, 0.055f, 0f));
        }

        private static void BuildOverheadPanel(Transform parent)
        {
            GameObject op = MeshUtil.CreateBoxObject("OverheadPanel",
                new Vector3(InteriorConfig.OverheadPanelWidth, 0.05f, InteriorConfig.OverheadPanelLength),
                MaterialLibrary.CockpitPanel, parent,
                new Vector3(0f, InteriorConfig.OverheadPanelY, InteriorConfig.OverheadPanelZ), true);
            op.transform.localRotation = Quaternion.Euler(-12f, 0f, 0f);

            CreateButtonCluster(op.transform, new Vector3(0f, -0.03f, 0f), 8, 5);
        }

        private static void BuildYokes(Transform parent)
        {
            CreateYoke("Yoke_Pilot_L", parent, new Vector3(-InteriorConfig.PilotSeatX, 0f, 0.52f));
            CreateYoke("Yoke_Pilot_R", parent, new Vector3(InteriorConfig.PilotSeatX, 0f, 0.52f));
        }

        private static void CreateYoke(string name, Transform parent, Vector3 pos)
        {
            GameObject yoke = new GameObject(name);
            yoke.transform.SetParent(parent, false);
            yoke.transform.localPosition = pos;
            Material mat = MaterialLibrary.CockpitSeat;

            MeshUtil.CreateBoxObject("Column", new Vector3(0.06f, 0.58f, 0.06f), MaterialLibrary.CockpitPanel, yoke.transform, new Vector3(0f, 0.29f, 0f), true);

            Vector3 topPos = new Vector3(0f, 0.58f, 0f);
            MeshUtil.CreateBoxObject("Crossbar", new Vector3(0.28f, 0.04f, 0.04f), mat, yoke.transform, topPos);
            MeshUtil.CreateBoxObject("Grip_L", new Vector3(0.04f, 0.14f, 0.04f), mat, yoke.transform, topPos + new Vector3(-0.12f, 0.05f, -0.02f));
            MeshUtil.CreateBoxObject("Grip_R", new Vector3(0.04f, 0.14f, 0.04f), mat, yoke.transform, topPos + new Vector3(0.12f, 0.05f, -0.02f));
        }
    }
}
