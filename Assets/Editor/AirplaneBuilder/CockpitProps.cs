using System.Collections.Generic;
using UnityEngine;

namespace AirplaneBuilder
{
    public static class CockpitProps
    {
        public static void BuildRearWall(Transform parent)
        {
            GameObject rearGo = new GameObject("RearWall");
            rearGo.transform.SetParent(parent, false);

            float zRear = InteriorConfig.ZCockpitRear;
            float doorW = InteriorConfig.RearDoorW;
            float doorH = InteriorConfig.RearDoorH;
            float halfW = InteriorConfig.CockpitHalfWidth;
            float ceilY = InteriorConfig.CockpitHeight;

            float leftW = halfW - doorW * 0.5f;
            Vector3 leftSize = new Vector3(leftW, doorH, 0.06f);
            MeshUtil.CreateBoxObject("Wall_Left", leftSize, CockpitMaterials.ShellLit, rearGo.transform, new Vector3(-halfW + leftW * 0.5f, doorH * 0.5f, zRear), true);
            MeshUtil.CreateBoxObject("Wall_Right", leftSize, CockpitMaterials.ShellLit, rearGo.transform, new Vector3(halfW - leftW * 0.5f, doorH * 0.5f, zRear), true);

            float headH = ceilY - doorH;
            MeshUtil.CreateBoxObject("Wall_Header", new Vector3(InteriorConfig.CockpitWidth, headH, 0.06f), CockpitMaterials.ShellLit, rearGo.transform, new Vector3(0f, doorH + headH * 0.5f, zRear), true);

            BuildCockpitDoor(rearGo.transform, zRear, doorW, doorH);
            BuildLeftWallProps(rearGo.transform, zRear);
            BuildRightWallProps(rearGo.transform, zRear);

            Vector3 lightPos = new Vector3(0f, ceilY - 0.02f, zRear + 0.30f);
            MeshUtil.CreateBoxObject("RearCeilingLight", new Vector3(0.25f, 0.02f, 0.25f), MaterialLibrary.CeilingLight, rearGo.transform, lightPos);
        }

        private static void BuildCockpitDoor(Transform parent, float z, float w, float h)
        {
            GameObject hinge = new GameObject("Doorway_Cockpit");
            hinge.transform.SetParent(parent, false);
            hinge.transform.localPosition = new Vector3(-w * 0.5f, 0f, z);

            GameObject door = new GameObject("Door_Cockpit");
            door.transform.SetParent(hinge.transform, false);
            door.transform.localPosition = new Vector3(w * 0.5f, h * 0.5f, 0f);

            MeshUtil.CreateBoxObject("DoorPanel", new Vector3(w - 0.02f, h - 0.02f, 0.05f), CockpitMaterials.DialFace, door.transform, Vector3.zero, true);
            MeshUtil.CreateBoxObject("Window", new Vector3(0.14f, 0.14f, 0.06f), CockpitMaterials.Dark, door.transform, new Vector3(0f, InteriorConfig.RearDoorWindowY - h * 0.5f, 0f));
            MeshUtil.CreateBoxObject("Handle", new Vector3(0.12f, 0.03f, 0.04f), CockpitMaterials.ShellLit, door.transform, new Vector3(w * 0.5f - 0.08f, 1.0f - h * 0.5f, 0.04f));

            DoorInteractable di = hinge.AddComponent<DoorInteractable>();
            di.SetOpenAngle(-85f);
        }

        private static void BuildLeftWallProps(Transform parent, float z)
        {
            GameObject leftProps = new GameObject("LeftRearProps");
            leftProps.transform.SetParent(parent, false);

            Vector3 swPos = new Vector3(-0.80f, 1.675f, z + 0.04f);
            MeshUtil.CreateBoxObject("SwitchPanel_Backing", new Vector3(0.35f, 0.35f, 0.03f), CockpitMaterials.Dark, leftProps.transform, swPos);

            Vector3 extPos = new Vector3(-0.75f, 0.70f, z + 0.09f);
            Mesh extMesh = MeshUtil.CreatePrism(10, InteriorConfig.ExtinguisherDia * 0.5f, InteriorConfig.ExtinguisherH);
            GameObject extGo = MeshUtil.CreateMeshObject("FireExtinguisher_Body", extMesh, MaterialLibrary.IndicatorRed, leftProps.transform);
            extGo.transform.localPosition = extPos;

            MeshUtil.CreateBoxObject("Extinguisher_Valve", new Vector3(0.04f, 0.06f, 0.04f), CockpitMaterials.Dark, leftProps.transform, extPos + new Vector3(0f, InteriorConfig.ExtinguisherH * 0.5f + 0.03f, 0f));
            MeshUtil.CreateBoxObject("Jumpseat_BackrestFolded", new Vector3(0.40f, 0.60f, 0.10f), CockpitMaterials.Seat, leftProps.transform, new Vector3(-0.90f, 0.75f, z + 0.06f));
            MeshUtil.CreateBoxObject("Jumpseat_BaseFolded", new Vector3(0.40f, 0.10f, 0.20f), CockpitMaterials.Seat, leftProps.transform, new Vector3(-0.90f, 0.40f, z + 0.12f));
        }

        private static void BuildRightWallProps(Transform parent, float z)
        {
            GameObject rightProps = new GameObject("RightRearProps");
            rightProps.transform.SetParent(parent, false);

            float cbW = InteriorConfig.BreakerPanelW;
            float cbH = InteriorConfig.BreakerPanelH;
            Vector3 cbPos = new Vector3(0.85f, 1.475f, z + 0.04f);
            MeshUtil.CreateBoxObject("BreakerPanel_Insert", new Vector3(cbW, cbH, 0.03f), CockpitMaterials.Dark, rightProps.transform, cbPos);

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Vector3 togPos = cbPos + new Vector3(-cbW * 0.5f + 0.05f + c * (cbW - 0.1f) / 7f, -cbH * 0.5f + 0.06f + r * (cbH - 0.12f) / 3f, 0.02f);
                    MeshUtil.CreateBoxObject($"Breaker_{r}_{c}", Vector3.one * 0.02f, ((r + c) % 4 == 0) ? CockpitMaterials.DialFace : CockpitMaterials.ShellLit, rightProps.transform, togPos);
                }
            }

            MeshUtil.CreateBoxObject("CrewFlightBag", new Vector3(0.50f, 0.40f, 0.35f), CockpitMaterials.Seat, rightProps.transform, new Vector3(0.85f, 0.20f, z + 0.30f), true);
        }

        public static void BuildPedestal(Transform parent)
        {
            GameObject pedGo = new GameObject("PedestalStand");
            pedGo.transform.SetParent(parent, false);

            float w = InteriorConfig.PedestalHalfWidth * 2f; // 0.34m
            float z0 = InteriorConfig.PedestalZRear;          // -0.25m
            float z1 = InteriorConfig.PedestalZFront;         // 0.82m
            float len = z1 - z0;                             // 1.07m
            float topY = InteriorConfig.PedestalTopY;        // 0.78m

            Vector3 pedPos = new Vector3(0f, topY * 0.5f, (z0 + z1) * 0.5f);
            MeshUtil.CreateBoxObject("PedestalBase", new Vector3(w, topY, len), CockpitMaterials.Panel, pedGo.transform, pedPos, true);
            MeshUtil.CreateBoxObject("PedestalTopPlate", new Vector3(w - 0.02f, 0.02f, len - 0.04f), CockpitMaterials.Dark, pedGo.transform, new Vector3(0f, topY + 0.01f, (z0 + z1) * 0.5f));

            // Throttles & Control Levers at Z=0.45
            Vector3 thPos = new Vector3(0f, topY + 0.06f, 0.45f);
            MeshUtil.CreateBoxObject("Throttle_L", new Vector3(0.035f, 0.10f, 0.04f), MaterialLibrary.IndicatorRed, pedGo.transform, thPos + new Vector3(-0.04f, 0f, 0f));
            MeshUtil.CreateBoxObject("Throttle_R", new Vector3(0.035f, 0.10f, 0.04f), CockpitMaterials.HorizonBlue, pedGo.transform, thPos + new Vector3(0.04f, 0f, 0f));
            MeshUtil.CreateBoxObject("SpeedbrakeLever", new Vector3(0.025f, 0.08f, 0.03f), CockpitMaterials.Dark, pedGo.transform, thPos + new Vector3(-0.11f, -0.01f, 0f));
            MeshUtil.CreateBoxObject("FlapLever", new Vector3(0.025f, 0.08f, 0.03f), CockpitMaterials.DialFace, pedGo.transform, thPos + new Vector3(0.11f, -0.01f, 0f));

            // Dual CDU Screens at Z=0.15
            MeshUtil.CreateBoxObject("CDU_Left", new Vector3(0.13f, 0.01f, 0.18f), CockpitMaterials.HorizonHaze, pedGo.transform, new Vector3(-0.075f, topY + 0.022f, 0.15f));
            MeshUtil.CreateBoxObject("CDU_Right", new Vector3(0.13f, 0.01f, 0.18f), CockpitMaterials.HorizonHaze, pedGo.transform, new Vector3(0.075f, topY + 0.022f, 0.15f));

            // Trim Wheels on flanks
            Mesh wheelMesh = MeshUtil.CreatePrism(10, 0.11f, 0.03f);
            GameObject twL = MeshUtil.CreateMeshObject("TrimWheel_L", wheelMesh, CockpitMaterials.Dark, pedGo.transform);
            twL.transform.localPosition = new Vector3(-w * 0.5f - 0.015f, topY - 0.08f, 0.45f);
            twL.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

            GameObject twR = MeshUtil.CreateMeshObject("TrimWheel_R", wheelMesh, CockpitMaterials.Dark, pedGo.transform);
            twR.transform.localPosition = new Vector3(w * 0.5f + 0.015f, topY - 0.08f, 0.45f);
            twR.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

            // Octagonal Dome Housing at rear end (Z=-0.12)
            Mesh domeMesh = MeshUtil.CreatePrism(8, InteriorConfig.OctagonalDomeDia * 0.5f, InteriorConfig.OctagonalDomeHeight);
            GameObject domeGo = MeshUtil.CreateMeshObject("AftOctagonalDome", domeMesh, CockpitMaterials.Panel, pedGo.transform);
            domeGo.transform.localPosition = new Vector3(0f, topY + InteriorConfig.OctagonalDomeHeight * 0.5f, -0.12f);
        }

        public static void BuildYokes(Transform parent)
        {
            GameObject yokesGo = new GameObject("FlightControls");
            yokesGo.transform.SetParent(parent, false);

            for (int side = -1; side <= 1; side += 2)
            {
                float x = InteriorConfig.YokeX * side;
                GameObject yk = new GameObject(side < 0 ? "Yoke_Capt" : "Yoke_FO");
                yk.transform.SetParent(yokesGo.transform, false);

                // Column rising from floor
                Vector3 colBase = new Vector3(x, 0.35f, 0.48f);
                MeshUtil.CreateBoxObject("Column", new Vector3(0.07f, 0.70f, 0.07f), CockpitMaterials.Dark, yk.transform, colBase);

                // Hub & Handle
                Vector3 hubPos = new Vector3(x, InteriorConfig.YokeHubY, InteriorConfig.YokeHubZ);
                MeshUtil.CreateBoxObject("Hub", new Vector3(0.09f, 0.07f, 0.09f), CockpitMaterials.Floor, yk.transform, hubPos);
                MeshUtil.CreateBoxObject("Crossbar", new Vector3(InteriorConfig.YokeBarWidth, 0.035f, 0.035f), CockpitMaterials.Floor, yk.transform, hubPos);

                float hw = InteriorConfig.YokeBarWidth * 0.5f;
                MeshUtil.CreateBoxObject("Grip_L", new Vector3(0.035f, InteriorConfig.YokeGripHeight, 0.035f), CockpitMaterials.Floor, yk.transform, hubPos + new Vector3(-hw, 0.03f, 0f));
                MeshUtil.CreateBoxObject("Grip_R", new Vector3(0.035f, InteriorConfig.YokeGripHeight, 0.035f), CockpitMaterials.Floor, yk.transform, hubPos + new Vector3( hw, 0.03f, 0f));
            }
        }

        public static void BuildSideConsoles(Transform leftParent, Transform rightParent)
        {
            float y = (InteriorConfig.ConsoleYMin + InteriorConfig.ConsoleYMax) * 0.5f;
            float h = InteriorConfig.ConsoleYMax - InteriorConfig.ConsoleYMin;
            float w = InteriorConfig.ConsoleXMax - InteriorConfig.ConsoleXMin;
            float z0 = InteriorConfig.ConsoleZStart;
            float z1 = InteriorConfig.ConsoleZEnd;
            float len = z1 - z0;
            float zMid = (z0 + z1) * 0.5f;

            // Left Console (Captain)
            Vector3 posL = new Vector3(-(InteriorConfig.ConsoleXMin + w * 0.5f), y, zMid);
            MeshUtil.CreateBoxObject("Console_L_Body", new Vector3(w, h, len), CockpitMaterials.Panel, leftParent, posL, true);

            // Captain's Clipboard with paperwork
            Vector3 clipPos = new Vector3(-InteriorConfig.ConsoleXMin - 0.08f, InteriorConfig.ConsoleYMax + 0.04f, 0.20f);
            GameObject clipGo = MeshUtil.CreateBoxObject("Clipboard", new Vector3(0.18f, 0.02f, 0.26f), CockpitMaterials.Dark, leftParent, clipPos);
            clipGo.transform.localRotation = Quaternion.Euler(20f, -10f, 0f);
            MeshUtil.CreateBoxObject("Paper", new Vector3(0.15f, 0.005f, 0.22f), CockpitMaterials.DialFace, clipGo.transform, new Vector3(0f, 0.012f, 0f));

            // Right Console (First Officer)
            Vector3 posR = new Vector3(InteriorConfig.ConsoleXMin + w * 0.5f, y, zMid);
            MeshUtil.CreateBoxObject("Console_R_Body", new Vector3(w, h, len), CockpitMaterials.Panel, rightParent, posR, true);
        }
    }
}
