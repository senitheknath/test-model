using UnityEngine;

namespace AirplaneBuilder
{
    public static class LavatoryBuilder
    {
        public static void BuildLav01(Transform parentRoot)
        {
            GameObject lav01 = new GameObject("Lav_01");
            lav01.transform.SetParent(parentRoot, false);
            BuildLavatoryRoom(lav01.transform, "Door_Lav_01", -1f, InteriorConfig.ZFrontGalleyRear, InteriorConfig.ZFrontLavRear);
        }

        public static void BuildLav02(Transform parentRoot)
        {
            GameObject lav02 = new GameObject("Lav_02");
            lav02.transform.SetParent(parentRoot, false);
            BuildLavatoryRoom(lav02.transform, "Door_Lav_02", -1f, InteriorConfig.ZCabinRear, InteriorConfig.ZRearLavRear);
        }

        public static void BuildLav03(Transform parentRoot)
        {
            GameObject lav03 = new GameObject("Lav_03");
            lav03.transform.SetParent(parentRoot, false);
            BuildLavatoryRoom(lav03.transform, "Door_Lav_03", 1f, InteriorConfig.ZCabinRear, InteriorConfig.ZRearLavRear);
        }

        private static void BuildLavatoryRoom(Transform root, string doorName, float xSign, float zFwd, float zAft)
        {
            GameObject doorsGroup = new GameObject("Doors");
            doorsGroup.transform.SetParent(root, false);

            GameObject propsGroup = new GameObject("Props");
            propsGroup.transform.SetParent(root, false);

            float aisleWallX = xSign * (InteriorConfig.AisleHalfWidth + 0.10f);
            float outerWallX = xSign * 2.05f;
            float midZ = (zFwd + zAft) * 0.5f;
            float roomLen = Mathf.Abs(zFwd - zAft);
            float roomWidth = Mathf.Abs(outerWallX - aisleWallX);

            // 1. Enclosure Walls
            BuildEnclosure(propsGroup.transform, xSign, aisleWallX, outerWallX, zFwd, zAft, roomLen);

            // 2. Door_Lav hinged on aisle wall opening
            float doorHingeZ = midZ + InteriorConfig.LavDoorWidth * 0.5f;
            DoorBuilder.CreateLavatoryDoor(doorName, doorsGroup.transform, new Vector3(aisleWallX, 0f, doorHingeZ), xSign < 0);

            // 3. Octagonal Toilet with Angular Lid & Flush Plate
            float toiletZ = zAft + 0.38f;
            float toiletX = xSign * 1.35f;
            CreateToilet(propsGroup.transform, new Vector3(toiletX, 0f, toiletZ), zAft);

            // 4. Corner Sink with Faucet and Knobs
            float sinkZ = zFwd - 0.35f;
            float sinkX = xSign * 0.95f;
            CreateCornerSink(propsGroup.transform, new Vector3(sinkX, 0f, sinkZ), xSign);

            // 5. Mirror above sink
            CreateMirror(propsGroup.transform, new Vector3(sinkX, 1.35f, zFwd - 0.05f));

            // 6. Paper Towel Dispenser & Waste Flap
            float sideWallX = xSign * 1.95f;
            CreateTowelAndWaste(propsGroup.transform, new Vector3(sideWallX, 0f, midZ), xSign);

            // 7. Handrails
            CreateHandrails(propsGroup.transform, xSign, sideWallX, toiletZ, midZ);
        }

        private static void BuildEnclosure(Transform parent, float xSign, float aisleX, float outerX, float zFwd, float zAft, float len)
        {
            float h = InteriorConfig.LavatoryHeight;
            float dw = InteriorConfig.LavDoorWidth;
            float midZ = (zFwd + zAft) * 0.5f;
            float wallPartLen = (len - dw) * 0.5f;

            // Aisle wall (forward part of doorway)
            MeshUtil.CreateBoxObject("Wall_Aisle_Fwd", new Vector3(0.06f, h, wallPartLen), MaterialLibrary.Wall, parent,
                new Vector3(aisleX, h * 0.5f, zFwd - wallPartLen * 0.5f), true);

            // Aisle wall (rear part of doorway)
            MeshUtil.CreateBoxObject("Wall_Aisle_Rear", new Vector3(0.06f, h, wallPartLen), MaterialLibrary.Wall, parent,
                new Vector3(aisleX, h * 0.5f, zAft + wallPartLen * 0.5f), true);

            // Aisle wall header above doorway
            float headerH = h - InteriorConfig.LavDoorHeight;
            MeshUtil.CreateBoxObject("Wall_Aisle_Header", new Vector3(0.06f, headerH, dw), MaterialLibrary.Wall, parent,
                new Vector3(aisleX, InteriorConfig.LavDoorHeight + headerH * 0.5f, midZ), true);

            // Partition forward wall
            float pWidth = Mathf.Abs(outerX - aisleX);
            float pCenterX = (outerX + aisleX) * 0.5f;
            MeshUtil.CreateBoxObject("Wall_Fwd", new Vector3(pWidth, h, 0.06f), MaterialLibrary.Wall, parent,
                new Vector3(pCenterX, h * 0.5f, zFwd), true);

            // Partition rear wall
            MeshUtil.CreateBoxObject("Wall_Rear", new Vector3(pWidth, h, 0.06f), MaterialLibrary.Wall, parent,
                new Vector3(pCenterX, h * 0.5f, zAft), true);

            // Ceiling
            MeshUtil.CreateBoxObject("Ceiling", new Vector3(pWidth, 0.06f, len), MaterialLibrary.Wall, parent,
                new Vector3(pCenterX, h + 0.03f, midZ), true);
        }

        private static void CreateToilet(Transform parent, Vector3 pos, float rearWallZ)
        {
            GameObject toilet = new GameObject("Toilet");
            toilet.transform.SetParent(parent, false);
            toilet.transform.localPosition = pos;

            float r = InteriorConfig.ToiletRadius;
            float th = InteriorConfig.ToiletHeight;

            // Octagonal base
            Mesh basePrism = MeshUtil.CreatePrism(8, r, th);
            GameObject baseObj = MeshUtil.CreateMeshObject("Base", basePrism, MaterialLibrary.Toilet, toilet.transform, true);
            baseObj.transform.localPosition = new Vector3(0f, th * 0.5f, 0f);

            // Toilet bowl rim
            Mesh rimPrism = MeshUtil.CreatePrism(8, r * 1.08f, 0.06f);
            GameObject rim = MeshUtil.CreateMeshObject("Rim", rimPrism, MaterialLibrary.Toilet, toilet.transform);
            rim.transform.localPosition = new Vector3(0f, th + 0.03f, 0f);

            // Dark inner hole
            Mesh innerPrism = MeshUtil.CreatePrism(8, r * 0.65f, 0.07f);
            GameObject inner = MeshUtil.CreateMeshObject("BowlInner", innerPrism, MaterialLibrary.GalleyTrolley, toilet.transform);
            inner.transform.localPosition = new Vector3(0f, th + 0.035f, 0f);

            // Angular toilet seat lid (upright against wall)
            GameObject lid = MeshUtil.CreateBoxObject("Lid", new Vector3(r * 1.8f, InteriorConfig.ToiletLidHeight, 0.04f), MaterialLibrary.Toilet, toilet.transform,
                new Vector3(0f, th + InteriorConfig.ToiletLidHeight * 0.45f, -r * 0.75f));
            lid.transform.localRotation = Quaternion.Euler(-8f, 0f, 0f);

            // Flush Plate on rear wall
            MeshUtil.CreateBoxObject("FlushPlate", new Vector3(0.18f, 0.18f, 0.02f), MaterialLibrary.WallShadow, parent,
                new Vector3(pos.x, th + 0.45f, rearWallZ + 0.04f));
            MeshUtil.CreateBoxObject("FlushButton", new Vector3(0.08f, 0.08f, 0.03f), MaterialLibrary.Sink, parent,
                new Vector3(pos.x, th + 0.45f, rearWallZ + 0.05f));
        }

        private static void CreateCornerSink(Transform parent, Vector3 pos, float xSign)
        {
            GameObject sink = new GameObject("CornerSink");
            sink.transform.SetParent(parent, false);
            sink.transform.localPosition = pos;

            float w = InteriorConfig.SinkBasinWidth;
            float l = InteriorConfig.SinkBasinLength;
            float sh = InteriorConfig.SinkHeight;

            // Pedestal column
            MeshUtil.CreateBoxObject("Pedestal", new Vector3(0.20f, sh - 0.15f, 0.20f), MaterialLibrary.Sink, sink.transform,
                new Vector3(0f, (sh - 0.15f) * 0.5f, 0f), true);

            // Basin block
            MeshUtil.CreateBoxObject("Basin", new Vector3(w, 0.15f, l), MaterialLibrary.Sink, sink.transform,
                new Vector3(0f, sh - 0.075f, 0f), true);

            // Basin bowl recess
            MeshUtil.CreateBoxObject("Recess", new Vector3(w - 0.10f, 0.08f, l - 0.10f), MaterialLibrary.Toilet, sink.transform,
                new Vector3(0f, sh - 0.03f, 0f));

            // Faucet Spout
            MeshUtil.CreateBoxObject("FaucetSpout", new Vector3(0.035f, 0.10f, 0.08f), MaterialLibrary.WallShadow, sink.transform,
                new Vector3(0f, sh + 0.05f, 0.04f));

            // Hot & Cold Knobs
            MeshUtil.CreateBoxObject("Knob_H", new Vector3(0.035f, 0.035f, 0.035f), MaterialLibrary.IndicatorRed, sink.transform,
                new Vector3(-0.08f, sh + 0.02f, 0.08f));
            MeshUtil.CreateBoxObject("Knob_C", new Vector3(0.035f, 0.035f, 0.035f), MaterialLibrary.HorizonBlue, sink.transform,
                new Vector3(0.08f, sh + 0.02f, 0.08f));
        }

        private static void CreateMirror(Transform parent, Vector3 pos)
        {
            MeshUtil.CreateBoxObject("Mirror", new Vector3(InteriorConfig.MirrorWidth, InteriorConfig.MirrorHeight, 0.02f),
                MaterialLibrary.Mirror, parent, pos);
        }

        private static void CreateTowelAndWaste(Transform parent, Vector3 pos, float xSign)
        {
            GameObject unit = new GameObject("TowelWasteUnit");
            unit.transform.SetParent(parent, false);
            unit.transform.localPosition = pos;

            float dir = -xSign; // inward into room

            // Paper towel dispenser box
            MeshUtil.CreateBoxObject("PaperTowelBox",
                new Vector3(0.12f, InteriorConfig.TowelBoxHeight, InteriorConfig.TowelBoxWidth),
                MaterialLibrary.Sink, unit.transform,
                new Vector3(dir * 0.06f, 1.35f, 0f));

            // Waste receptacle box with angled flap
            GameObject waste = MeshUtil.CreateBoxObject("WasteFlapBox",
                new Vector3(0.12f, InteriorConfig.WasteFlapHeight, InteriorConfig.WasteFlapWidth),
                MaterialLibrary.Toilet, unit.transform,
                new Vector3(dir * 0.06f, 0.70f, 0f));

            MeshUtil.CreateBoxObject("FlapPlate",
                new Vector3(0.02f, InteriorConfig.WasteFlapHeight * 0.7f, InteriorConfig.WasteFlapWidth * 0.8f),
                MaterialLibrary.WallShadow, waste.transform,
                new Vector3(dir * 0.06f, 0f, 0f));
        }

        private static void CreateHandrails(Transform parent, float xSign, float wallX, float toiletZ, float midZ)
        {
            float dir = -xSign;
            float hl = InteriorConfig.HandrailLength;

            // Handrail 1: Horizontal grab bar next to toilet
            GameObject rail1 = new GameObject("Handrail_Toilet");
            rail1.transform.SetParent(parent, false);
            rail1.transform.localPosition = new Vector3(wallX + dir * 0.05f, 0.75f, toiletZ + 0.15f);

            MeshUtil.CreateBoxObject("Bar", new Vector3(0.03f, 0.03f, hl), MaterialLibrary.WallShadow, rail1.transform, Vector3.zero);
            MeshUtil.CreateBoxObject("Stand1", new Vector3(0.05f, 0.03f, 0.03f), MaterialLibrary.WallShadow, rail1.transform, new Vector3(dir * 0.02f, 0f, hl * 0.45f));
            MeshUtil.CreateBoxObject("Stand2", new Vector3(0.05f, 0.03f, 0.03f), MaterialLibrary.WallShadow, rail1.transform, new Vector3(dir * 0.02f, 0f, -hl * 0.45f));

            // Handrail 2: Near sink / door
            GameObject rail2 = new GameObject("Handrail_Door");
            rail2.transform.SetParent(parent, false);
            rail2.transform.localPosition = new Vector3(wallX + dir * 0.05f, 1.05f, midZ + 0.25f);

            MeshUtil.CreateBoxObject("Bar", new Vector3(0.03f, 0.03f, hl * 0.8f), MaterialLibrary.WallShadow, rail2.transform, Vector3.zero);
            MeshUtil.CreateBoxObject("Stand1", new Vector3(0.05f, 0.03f, 0.03f), MaterialLibrary.WallShadow, rail2.transform, new Vector3(dir * 0.02f, 0f, hl * 0.35f));
            MeshUtil.CreateBoxObject("Stand2", new Vector3(0.05f, 0.03f, 0.03f), MaterialLibrary.WallShadow, rail2.transform, new Vector3(dir * 0.02f, 0f, -hl * 0.35f));
        }
    }
}
