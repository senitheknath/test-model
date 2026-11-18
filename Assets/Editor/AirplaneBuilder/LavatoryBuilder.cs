using UnityEngine;

namespace AirplaneBuilder
{
    public static class LavatoryBuilder
    {
        public static void BuildLav01(Transform parentRoot)
        {
            GameObject lav01 = new GameObject("Lav_01");
            lav01.transform.SetParent(parentRoot, false);

            GameObject doors = new GameObject("Doors");
            doors.transform.SetParent(lav01.transform, false);

            GameObject props = new GameObject("Props");
            props.transform.SetParent(lav01.transform, false);
        }

        public static void BuildLav02(Transform parentRoot)
        {
            GameObject lav02 = new GameObject("Lav_02");
            lav02.transform.SetParent(parentRoot, false);

            GameObject doors = new GameObject("Doors");
            doors.transform.SetParent(lav02.transform, false);

            GameObject props = new GameObject("Props");
            props.transform.SetParent(lav02.transform, false);
        }

        public static void BuildLav03(Transform parentRoot)
        {
            GameObject lav03 = new GameObject("Lav_03");
            lav03.transform.SetParent(parentRoot, false);

            GameObject doors = new GameObject("Doors");
            doors.transform.SetParent(lav03.transform, false);

            GameObject props = new GameObject("Props");
            props.transform.SetParent(lav03.transform, false);
        }
    }
}
