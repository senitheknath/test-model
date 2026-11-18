using UnityEngine;

namespace AirplaneBuilder
{
    public static class GalleyMainBlock
    {
        public static void Build(Transform parent)
        {
            GameObject mbGo = new GameObject("MainBlock");
            mbGo.transform.SetParent(parent, false);

            BuildCarcassAndCounter(mbGo.transform);
            BuildLowerBays(mbGo.transform);
            BuildMiddleTier(mbGo.transform);
            BuildUpperTier(mbGo.transform);
        }

        private static void BuildCarcassAndCounter(Transform parent)
        {
            float x0 = 0.80f;
            float x1 = 2.00f;
            float w = x1 - x0;       // 1.20m
            float depth = 0.60f;
            float midX = (x0 + x1) * 0.5f;

            // Back wall of the main block
            Vector3 backPos = new Vector3(midX, 1.00f, 0.01f);
            MeshUtil.CreateBoxObject("BlockBacking", new Vector3(w, 2.00f, 0.02f), GalleyMaterials.WallLit, parent, backPos, true);

            // Countertop slab at Y = 0.85..0.90m
            Vector3 counterPos = new Vector3(midX, 0.875f, -depth * 0.5f);
            MeshUtil.CreateBoxObject("CounterTop", new Vector3(w, 0.05f, depth), GalleyMaterials.CounterTop, parent, counterPos, true);

            // Front edge trim of the counter
            Vector3 edgePos = new Vector3(midX, 0.875f, -depth - 0.005f);
            MeshUtil.CreateBoxObject("CounterEdge", new Vector3(w, 0.05f, 0.01f), GalleyMaterials.CounterEdge, parent, edgePos);

            // Side end-panel on left (X = 0.80m)
            Vector3 endPos = new Vector3(x0 - 0.01f, 0.975f, -depth * 0.5f);
            MeshUtil.CreateBoxObject("BlockEnd_L", new Vector3(0.02f, 1.95f, depth), GalleyMaterials.Cabinet, parent, endPos, true);
        }

        private static void BuildLowerBays(Transform parent)
        {
            GameObject lowerGo = new GameObject("LowerBays");
            lowerGo.transform.SetParent(parent, false);

            // Trolley 01 (Slides out 0.40m with slideable drawer containing 6 meal boxes)
            BuildTrolley(lowerGo.transform, 0.82f, 1.16f, "Trolley_01", true);

            // Trolley 02 (Static parked cart)
            BuildTrolley(lowerGo.transform, 1.18f, 1.52f, "Trolley_02", false);

            // Oven bay: X 1.54..1.98m
            BuildOvenBay(lowerGo.transform, 1.54f, 1.98f);
        }

        private static void BuildTrolley(Transform parent, float x0, float x1, string name, bool hasDrawer)
        {
            float w = x1 - x0; // 0.34m
            float h = 0.72f;
            float d = 0.54f;
            float midX = (x0 + x1) * 0.5f;

            GameObject trolleyRoot = new GameObject(name);
            trolleyRoot.transform.SetParent(parent, false);
            trolleyRoot.transform.localPosition = new Vector3(midX, h * 0.5f, -d * 0.5f - 0.03f);

            // Trolley Main Body
            MeshUtil.CreateBoxObject("Body", new Vector3(w, h, d), GalleyMaterials.Trolley, trolleyRoot.transform, Vector3.zero, true);

            // Top notch handle
            Vector3 handlePos = new Vector3(0f, h * 0.5f - 0.04f, -d * 0.5f - 0.015f);
            MeshUtil.CreateBoxObject("NotchHandle", new Vector3(0.18f, 0.04f, 0.03f), GalleyMaterials.CounterEdge, trolleyRoot.transform, handlePos);

            // Brake pedal at bottom
            Vector3 pedalPos = new Vector3(0f, -h * 0.5f + 0.03f, -d * 0.5f - 0.02f);
            MeshUtil.CreateBoxObject("BrakePedal", new Vector3(0.08f, 0.03f, 0.05f), GalleyMaterials.CounterEdge, trolleyRoot.transform, pedalPos);

            // 3 Maroon cups on top in gap under counter
            for (int i = 0; i < 3; i++)
            {
                float cx = -w * 0.3f + i * (w * 0.3f);
                Mesh cupMesh = MeshUtil.CreatePrism(8, 0.035f, 0.10f);
                GameObject cup = MeshUtil.CreateMeshObject($"Cup_{i + 1}", cupMesh, GalleyMaterials.Maroon, trolleyRoot.transform);
                cup.transform.localPosition = new Vector3(cx, h * 0.5f + 0.05f, 0f);
            }

            if (hasDrawer)
            {
                // SlideInteractable for Trolley cart (slides out 0.40m along -Z)
                SlideInteractable cartSlide = trolleyRoot.AddComponent<SlideInteractable>();
                cartSlide.SetAxis(Vector3.back);
                cartSlide.SetSlideDistance(0.40f);

                // Child Drawer
                GameObject drawerGo = new GameObject("Trolley_01_Drawer");
                drawerGo.transform.SetParent(trolleyRoot.transform, false);
                drawerGo.transform.localPosition = new Vector3(0f, h * 0.5f - 0.10f, 0f);

                float dw = 0.30f;
                float dh = 0.06f;
                float dd = 0.40f;
                MeshUtil.CreateBoxObject("Tray", new Vector3(dw, dh, dd), GalleyMaterials.CounterTop, drawerGo.transform, Vector3.zero, true);

                // Drawer slides out an additional 0.35m along -Z
                SlideInteractable drawerSlide = drawerGo.AddComponent<SlideInteractable>();
                drawerSlide.SetAxis(Vector3.back);
                drawerSlide.SetSlideDistance(0.35f);

                // 6 Meal Boxes in 2x3 grid inside drawer (alternating yellow/amber)
                float[] mealX = new float[] { -0.07f, 0.07f };
                float[] mealZ = new float[] { -0.12f, 0f, 0.12f };
                int idx = 0;
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 2; c++)
                    {
                        Material mMat = (idx % 2 == 0) ? GalleyMaterials.CeilingLight : GalleyMaterials.CounterEdge;
                        Vector3 mbPos = new Vector3(mealX[c], 0.035f, mealZ[r]);
                        MeshUtil.CreateBoxObject($"MealBox_{idx + 1}", new Vector3(0.10f, 0.05f, 0.07f), mMat, drawerGo.transform, mbPos);
                        idx++;
                    }
                }
            }
        }

        private static void BuildOvenBay(Transform parent, float x0, float x1)
        {
            float w = x1 - x0; // 0.44m
            float midX = (x0 + x1) * 0.5f;

            // Lower base cabinet at Y = 0..0.45m
            Vector3 cabPos = new Vector3(midX, 0.225f, -0.28f);
            MeshUtil.CreateBoxObject("BaseCabinet", new Vector3(w, 0.45f, 0.56f), GalleyMaterials.Cabinet, parent, cabPos, true);

            // 2 Maroon handle tabs on base cabinet doors
            MeshUtil.CreateBoxObject("CabTab_L", new Vector3(0.05f, 0.03f, 0.015f), GalleyMaterials.Maroon, parent, new Vector3(midX - 0.10f, 0.35f, -0.565f));
            MeshUtil.CreateBoxObject("CabTab_R", new Vector3(0.05f, 0.03f, 0.015f), GalleyMaterials.Maroon, parent, new Vector3(midX + 0.10f, 0.35f, -0.565f));

            // Light oven at Y = 0.45..0.75m
            Vector3 ovenPos = new Vector3(midX, 0.60f, -0.28f);
            MeshUtil.CreateBoxObject("BayOven_Body", new Vector3(w - 0.02f, 0.30f, 0.54f), GalleyMaterials.OvenBody, parent, ovenPos, true);

            // Dark window & handle bar
            MeshUtil.CreateBoxObject("BayOven_Window", new Vector3(w - 0.12f, 0.18f, 0.01f), GalleyMaterials.GlassDark, parent, new Vector3(midX, 0.60f, -0.555f));
            MeshUtil.CreateBoxObject("BayOven_Handle", new Vector3(0.02f, 0.16f, 0.03f), GalleyMaterials.CounterEdge, parent, new Vector3(midX + 0.14f, 0.60f, -0.57f));
        }

        private static void BuildMiddleTier(Transform parent)
        {
            GameObject midGo = new GameObject("MiddleTier");
            midGo.transform.SetParent(parent, false);

            // Two upper cabinets: X 0.82..1.15 and X 1.17..1.50
            BuildMiddleCabinet(midGo.transform, 0.82f, 1.15f, "MidCab_1");
            BuildMiddleCabinet(midGo.transform, 1.17f, 1.50f, "MidCab_2");

            // Microwave bay: X 1.54..1.98m
            BuildMicrowaveBay(midGo.transform, 1.54f, 1.98f);
        }

        private static void BuildMiddleCabinet(Transform parent, float x0, float x1, string name)
        {
            float w = x1 - x0; // 0.33m
            float h = 0.48f;
            float d = 0.40f;
            float midX = (x0 + x1) * 0.5f;

            Vector3 pos = new Vector3(midX, 1.15f, -d * 0.5f - 0.05f);
            MeshUtil.CreateBoxObject(name, new Vector3(w, h, d), GalleyMaterials.Cabinet, parent, pos, true);

            // Inset door line
            MeshUtil.CreateBoxObject($"{name}_Inset", new Vector3(w - 0.06f, h - 0.06f, 0.005f), GalleyMaterials.DoorInset, parent, pos + new Vector3(0f, 0f, -d * 0.5f - 0.003f));

            // Maroon latch tab at bottom center
            Vector3 tabPos = new Vector3(midX, 0.93f, -d - 0.055f);
            MeshUtil.CreateBoxObject($"{name}_LatchTab", new Vector3(0.06f, 0.03f, 0.02f), GalleyMaterials.Maroon, parent, tabPos);
        }

        private static void BuildMicrowaveBay(Transform parent, float x0, float x1)
        {
            float midX = (x0 + x1) * 0.5f;
            float y = 1.12f;

            // Microwave Body (0.42 x 0.30 x 0.35m)
            Vector3 mwPos = new Vector3(midX, y, -0.25f);
            MeshUtil.CreateBoxObject("Microwave_Body", new Vector3(0.42f, 0.30f, 0.35f), GalleyMaterials.OvenBody, parent, mwPos, true);

            // Dark window (0.25 x 0.17m)
            Vector3 winPos = new Vector3(midX - 0.06f, y, -0.43f);
            MeshUtil.CreateBoxObject("Microwave_Window", new Vector3(0.25f, 0.17f, 0.01f), GalleyMaterials.GlassDark, parent, winPos);

            // Control panel with 3 tiny dials & amber display
            Vector3 cpPos = new Vector3(midX + 0.13f, y + 0.08f, -0.43f);
            MeshUtil.CreateBoxObject("Microwave_Display", new Vector3(0.08f, 0.03f, 0.01f), GalleyMaterials.ExitGlow, parent, cpPos);

            for (int i = 0; i < 3; i++)
            {
                Vector3 dPos = new Vector3(midX + 0.13f, y + 0.02f - i * 0.05f, -0.435f);
                Mesh knobMesh = MeshUtil.CreatePrism(8, 0.015f, 0.015f);
                GameObject k = MeshUtil.CreateMeshObject($"Dial_{i + 1}", knobMesh, GalleyMaterials.CounterEdge, parent);
                k.transform.localPosition = dPos;
                k.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            }

            // 3 Cups beside microwave
            for (int i = 0; i < 3; i++)
            {
                Mesh cupMesh = MeshUtil.CreatePrism(8, 0.035f, 0.10f);
                GameObject cup = MeshUtil.CreateMeshObject($"ShelfCup_{i + 1}", cupMesh, GalleyMaterials.DoorLeaf, parent);
                cup.transform.localPosition = new Vector3(0.85f + i * 0.09f, 0.95f, -0.20f);
            }
        }

        private static void BuildUpperTier(Transform parent)
        {
            GameObject upGo = new GameObject("UpperTier");
            upGo.transform.SetParent(parent, false);

            float[] xStarts = new float[] { 0.82f, 1.24f, 1.66f };
            float[] xEnds   = new float[] { 1.22f, 1.64f, 2.00f };

            for (int i = 0; i < 3; i++)
            {
                float w = xEnds[i] - xStarts[i];
                float midX = (xStarts[i] + xEnds[i]) * 0.5f;
                float h = 0.48f;
                float d = 0.40f;
                Vector3 pos = new Vector3(midX, 1.70f, -d * 0.5f - 0.05f);

                MeshUtil.CreateBoxObject($"UpperCab_{i + 1}", new Vector3(w, h, d), GalleyMaterials.Cabinet, upGo.transform, pos, true);
                MeshUtil.CreateBoxObject($"UpperCab_{i + 1}_Inset", new Vector3(w - 0.06f, h - 0.06f, 0.005f), GalleyMaterials.DoorInset, upGo.transform, pos + new Vector3(0f, 0f, -d * 0.5f - 0.003f));

                // Maroon latch tab at bottom center
                Vector3 tabPos = new Vector3(midX, 1.48f, -d - 0.055f);
                MeshUtil.CreateBoxObject($"UpperCab_{i + 1}_Tab", new Vector3(0.06f, 0.03f, 0.02f), GalleyMaterials.Maroon, upGo.transform, tabPos);
            }
        }
    }
}
