using UnityEngine;

namespace AirplaneBuilder
{
    public static class CockpitBuilder
    {
        public static void Build(Transform parent)
        {
            CockpitMaterials.Initialize();

            GameObject cockpitRoot = new GameObject("Cockpit");
            cockpitRoot.transform.SetParent(parent, false);
            cockpitRoot.transform.localPosition = Vector3.zero;

            Transform shellRoot = CreateContainer("Shell", cockpitRoot.transform);
            Transform windshieldRoot = CreateContainer("Windshield", cockpitRoot.transform);
            Transform glareshieldRoot = CreateContainer("Glareshield", cockpitRoot.transform);
            Transform instPanelRoot = CreateContainer("InstrumentPanel", cockpitRoot.transform);
            Transform pedestalRoot = CreateContainer("Pedestal", cockpitRoot.transform);
            Transform yokesRoot = CreateContainer("Yokes", cockpitRoot.transform);
            Transform seatsRoot = CreateContainer("Seats", cockpitRoot.transform);
            Transform pilotsRoot = CreateContainer("Pilots", cockpitRoot.transform);
            Transform overheadRoot = CreateContainer("Overhead", cockpitRoot.transform);
            Transform sideLRoot = CreateContainer("SideConsole_L", cockpitRoot.transform);
            Transform sideRRoot = CreateContainer("SideConsole_R", cockpitRoot.transform);
            Transform rearWallRoot = CreateContainer("RearWall", cockpitRoot.transform);
            Transform lightsRoot = CreateContainer("Lights", cockpitRoot.transform);
            Transform viewCamerasRoot = CreateContainer("ViewCameras", cockpitRoot.transform);

            // Shell, Windshield, Side Windows, Night Backdrop
            CockpitShell.Build(shellRoot, windshieldRoot);

            // Glareshield & Autopilot MCP
            CockpitPanels.BuildGlareshield(glareshieldRoot);

            // Main Instrument Panel with PFDs, 10-sided dials, displays, and gear lever
            CockpitPanels.BuildMainPanel(instPanelRoot);

            // Center Pedestal with throttles, CDUs, trim wheels, aft dome
            CockpitProps.BuildPedestal(pedestalRoot);

            // Flight Control Yokes
            CockpitProps.BuildYokes(yokesRoot);

            // Seats & Pilot figures
            CockpitSeats.BuildSeats(seatsRoot, pilotsRoot);

            // Overhead Console
            CockpitPanels.BuildOverhead(overheadRoot);

            // Side Consoles & Clipboard
            CockpitProps.BuildSideConsoles(sideLRoot, sideRRoot);

            // Rear Wall, Door, Fire Extinguisher, Breaker Panel, Folded Jumpseat
            CockpitProps.BuildRearWall(rearWallRoot);

            // Cockpit Moody Night-Flight Lighting
            BuildCockpitLighting(lightsRoot);

            // 10 Reference View Cameras & Capture tool
            CockpitCameraRig.BuildCameras(viewCamerasRoot);
        }

        private static void BuildCockpitLighting(Transform parent)
        {
            GameObject warmGlow = new GameObject("Light_ConsoleWarm");
            warmGlow.transform.SetParent(parent, false);
            warmGlow.transform.localPosition = new Vector3(0f, 1.15f, 0.40f);
            Light lWarm = warmGlow.AddComponent<Light>();
            lWarm.type = LightType.Point;
            lWarm.color = new Color(1.0f, 0.72f, 0.45f);
            lWarm.range = 2.8f;
            lWarm.intensity = 0.85f;

            GameObject coolFill = new GameObject("Light_CeilingCool");
            coolFill.transform.SetParent(parent, false);
            coolFill.transform.localPosition = new Vector3(0f, 2.10f, 0.0f);
            Light lCool = coolFill.AddComponent<Light>();
            lCool.type = LightType.Point;
            lCool.color = new Color(0.40f, 0.55f, 0.75f);
            lCool.range = 3.2f;
            lCool.intensity = 0.45f;
        }

        private static Transform CreateContainer(string name, Transform parent)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            return go.transform;
        }
    }
}
