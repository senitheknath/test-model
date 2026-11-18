using UnityEngine;

namespace AirplaneBuilder
{
    public static class CockpitSeats
    {
        public static void BuildSeats(Transform seatsParent, Transform pilotsParent)
        {
            BuildPilotSeat(seatsParent, -InteriorConfig.PilotSeatX, InteriorConfig.PilotSeatZ, "Seat_Captain");
            BuildPilotSeat(seatsParent,  InteriorConfig.PilotSeatX, InteriorConfig.PilotSeatZ, "Seat_FirstOfficer");

            if (InteriorConfig.ShowPilots)
            {
                BuildPilotFigure(pilotsParent, -InteriorConfig.PilotSeatX, InteriorConfig.PilotSeatZ, true, "Pilot_Captain");
                BuildPilotFigure(pilotsParent,  InteriorConfig.PilotSeatX, InteriorConfig.PilotSeatZ, false, "Pilot_FirstOfficer");
            }
        }

        private static void BuildPilotSeat(Transform parent, float x, float z, string name)
        {
            GameObject seatGo = new GameObject(name);
            seatGo.transform.SetParent(parent, false);
            seatGo.transform.localPosition = new Vector3(x, 0f, z);

            float pedH = InteriorConfig.PilotSeatPedestalHeight; // 0.40m
            float pedS = InteriorConfig.PilotSeatPedestalSize;   // 0.30m
            MeshUtil.CreateBoxObject("Pedestal", new Vector3(pedS, pedH, pedS), CockpitMaterials.Dark, seatGo.transform, new Vector3(0f, pedH * 0.5f, 0f), true);

            float cushW = InteriorConfig.PilotSeatCushionW; // 0.50m
            float cushD = InteriorConfig.PilotSeatCushionD; // 0.50m
            float cushH = InteriorConfig.PilotSeatCushionH; // 0.14m
            float cushTop = InteriorConfig.PilotSeatCushionTopY; // 0.48m
            Vector3 cushPos = new Vector3(0f, cushTop - cushH * 0.5f, 0f);
            MeshUtil.CreateBoxObject("Cushion", new Vector3(cushW, cushH, cushD), CockpitMaterials.Seat, seatGo.transform, cushPos, true);

            // Backrest tilted 8 degrees back
            GameObject backGo = new GameObject("BackrestAssembly");
            backGo.transform.SetParent(seatGo.transform, false);
            backGo.transform.localPosition = new Vector3(0f, cushTop, -cushD * 0.45f);
            backGo.transform.localRotation = Quaternion.Euler(8f, 0f, 0f);

            float backW = InteriorConfig.PilotSeatBackrestW; // 0.52m
            float backH = InteriorConfig.PilotSeatBackrestH; // 0.72m
            float backD = InteriorConfig.PilotSeatBackrestD; // 0.12m
            MeshUtil.CreateBoxObject("Backrest", new Vector3(backW, backH, backD), CockpitMaterials.Seat, backGo.transform, new Vector3(0f, backH * 0.5f, 0f));

            // Headrest
            float headW = InteriorConfig.PilotSeatHeadrestW; // 0.40m
            float headH = InteriorConfig.PilotSeatHeadrestH; // 0.28m
            float headD = InteriorConfig.PilotSeatHeadrestD; // 0.14m
            Vector3 headPos = new Vector3(0f, backH + headH * 0.5f, 0.02f);
            MeshUtil.CreateBoxObject("Headrest", new Vector3(headW, headH, headD), CockpitMaterials.Seat, backGo.transform, headPos);

            // Side Armrests
            for (int side = -1; side <= 1; side += 2)
            {
                Vector3 armPos = new Vector3((cushW * 0.5f + 0.03f) * side, cushTop + 0.18f, -0.05f);
                MeshUtil.CreateBoxObject($"Armrest_{side}", new Vector3(0.06f, 0.06f, 0.36f), CockpitMaterials.Dark, seatGo.transform, armPos);
            }
        }

        private static void BuildPilotFigure(Transform parent, float x, float z, bool isCaptain, string name)
        {
            GameObject pilotGo = new GameObject(name);
            pilotGo.transform.SetParent(parent, false);

            float seatY = InteriorConfig.PilotSeatCushionTopY;
            float torsoW = InteriorConfig.PilotTorsoW;
            float torsoH = InteriorConfig.PilotTorsoH;
            float torsoD = InteriorConfig.PilotTorsoD;

            // Seated Torso
            Vector3 torsoPos = new Vector3(x, seatY + torsoH * 0.5f, z - 0.06f);
            MeshUtil.CreateBoxObject("Torso_Uniform", new Vector3(torsoW, torsoH, torsoD), CockpitMaterials.ShellShadow, pilotGo.transform, torsoPos);

            // Head
            float headSize = InteriorConfig.PilotHeadSize; // 0.20m
            Vector3 headPos = new Vector3(x, torsoPos.y + torsoH * 0.5f + headSize * 0.5f, z - 0.04f);
            MeshUtil.CreateBoxObject("Head", Vector3.one * headSize, CockpitMaterials.DialFace, pilotGo.transform, headPos);

            // Hair cap
            Vector3 hairPos = headPos + new Vector3(0f, headSize * 0.4f, 0f);
            MeshUtil.CreateBoxObject("Hair", new Vector3(headSize + 0.02f, headSize * 0.35f, headSize + 0.02f), CockpitMaterials.Dark, pilotGo.transform, hairPos);

            // Arms extending forward toward yoke
            for (int side = -1; side <= 1; side += 2)
            {
                Vector3 shoulder = torsoPos + new Vector3((torsoW * 0.5f + 0.05f) * side, torsoH * 0.25f, 0.05f);
                Vector3 hand = new Vector3(x + (InteriorConfig.YokeBarWidth * 0.45f) * side, InteriorConfig.YokeHubY, InteriorConfig.YokeHubZ);
                Vector3 armDir = hand - shoulder;
                float armLen = armDir.magnitude;

                GameObject arm = new GameObject($"Arm_{side}");
                arm.transform.SetParent(pilotGo.transform, false);
                arm.transform.localPosition = (shoulder + hand) * 0.5f;
                arm.transform.localRotation = Quaternion.LookRotation(armDir, Vector3.up);

                MeshUtil.CreateBoxObject("Sleeve", new Vector3(0.08f, 0.08f, armLen), CockpitMaterials.ShellShadow, arm.transform, Vector3.zero);
                MeshUtil.CreateBoxObject("Hand", new Vector3(0.06f, 0.06f, 0.07f), CockpitMaterials.DialFace, arm.transform, new Vector3(0f, 0f, armLen * 0.45f));
            }
        }
    }
}
