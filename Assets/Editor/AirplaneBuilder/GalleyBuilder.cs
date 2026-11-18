using UnityEngine;

namespace AirplaneBuilder
{
    public enum GalleyType
    {
        Front,
        Rear
    }

    public static class GalleyBuilder
    {
        public static void BuildFront(Transform parent) => Build(parent, GalleyType.Front);
        public static void BuildRear(Transform parent) => Build(parent, GalleyType.Rear);

        public static void Build(Transform parent, GalleyType type)
        {
            // 1. Initialize Galley shared materials
            GalleyMaterials.Initialize();

            // 2. Determine root transform, position and rotation
            string rootName = (type == GalleyType.Front) ? "Galley_Front" : "Galley_Rear";
            GameObject galleyRoot = new GameObject(rootName);
            galleyRoot.transform.SetParent(parent, false);

            float length;
            if (type == GalleyType.Front)
            {
                galleyRoot.transform.localPosition = new Vector3(0f, 0f, InteriorConfig.GalleyFrontFaceZ); // -1.65m
                galleyRoot.transform.localRotation = Quaternion.identity;
                length = InteriorConfig.GalleyFrontLength; // 3.25m
            }
            else
            {
                galleyRoot.transform.localPosition = new Vector3(0f, 0f, InteriorConfig.ZRearGalleyRear); // -28.6m
                galleyRoot.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                length = InteriorConfig.GalleyRearLength; // 3.50m
            }

            // 3. Create required Hierarchy containers
            Transform shellRoot = CreateContainer("Shell", galleyRoot.transform);
            Transform bulkheadRoot = CreateContainer("Bulkhead", galleyRoot.transform);
            Transform mainBlockRoot = CreateContainer("MainBlock", galleyRoot.transform);
            Transform ovenWallRoot = CreateContainer("OvenWall", galleyRoot.transform);
            Transform prepCounterRoot = CreateContainer("PrepCounter", galleyRoot.transform);
            Transform coffeeShelfRoot = CreateContainer("CoffeeShelf", galleyRoot.transform);
            Transform lockersRoot = CreateContainer("Lockers", galleyRoot.transform);
            Transform wasteBinRoot = CreateContainer("WasteBin", galleyRoot.transform);
            Transform intercomRoot = CreateContainer("Intercom", galleyRoot.transform);
            Transform jumpseatRoot = CreateContainer("Jumpseat", galleyRoot.transform);
            Transform doorsRoot = CreateContainer("Doors", galleyRoot.transform);
            Transform propsRoot = CreateContainer("Props", galleyRoot.transform);
            Transform lightsRoot = CreateContainer("Lights", galleyRoot.transform);
            Transform viewCamerasRoot = CreateContainer("ViewCameras", galleyRoot.transform);

            // Phase 1: Shell (liners, skin, floor), Bulkhead with doors, Dark Stack, Ceiling Lights, Entry Doors, Jumpseat & Alcove
            GalleyShell.Build(shellRoot, type, length);
            GalleyWallUnits.BuildBulkhead(bulkheadRoot, type);
            GalleyWallUnits.BuildJumpseat(jumpseatRoot);
            GalleyWallUnits.BuildEntryDoors(doorsRoot, type);

            // Phase 2 Stubs: MainBlock, OvenWall, WasteBin
            GalleyMainBlock.Build(mainBlockRoot);
            GalleyProps.BuildOvenWall(ovenWallRoot, length);
            GalleyProps.BuildWasteBin(wasteBinRoot);

            // Phase 3 Stubs: PrepCounter, CoffeeShelf, Lockers, Intercom
            GalleyProps.BuildPrepCounter(prepCounterRoot, length);
            GalleyProps.BuildCoffeeShelf(coffeeShelfRoot, length);
            GalleyProps.BuildUpperLockers(lockersRoot, length);
            GalleyProps.BuildIntercom(intercomRoot);

            // Phase 4: Reference Cameras (Front Galley)
            if (type == GalleyType.Front)
            {
                GalleyCameraRig.BuildCameras(viewCamerasRoot);
            }
        }

        private static Transform CreateContainer(string name, Transform parent)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            return go.transform;
        }
    }
}
