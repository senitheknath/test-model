using UnityEngine;

namespace AirplaneBuilder
{
    public class SeatInteractable : MonoBehaviour
    {
        [Header("Seat Information")]
        public string SeatId = "Seat_00_L1";

        public void Interact()
        {
            Debug.Log($"[SeatInteractable] Interacted with seat: {SeatId}");
        }
    }
}
