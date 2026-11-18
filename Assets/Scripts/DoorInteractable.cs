using UnityEngine;

namespace AirplaneBuilder
{
    public class DoorInteractable : MonoBehaviour
    {
        [Header("Door Settings")]
        [SerializeField] private float openAngle = -85f;
        [SerializeField] private float smoothSpeed = 6f;
        [SerializeField] private bool isOpen = false;

        private Quaternion closedRotation;
        private Quaternion targetRotation;

        public bool IsOpen => isOpen;

        private void Awake()
        {
            closedRotation = transform.localRotation;
            targetRotation = isOpen ? closedRotation * Quaternion.Euler(0f, openAngle, 0f) : closedRotation;
        }

        private void Update()
        {
            if (Quaternion.Angle(transform.localRotation, targetRotation) > 0.05f)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
            }
            else
            {
                transform.localRotation = targetRotation;
            }
        }

        public void Interact()
        {
            isOpen = !isOpen;
            targetRotation = isOpen ? closedRotation * Quaternion.Euler(0f, openAngle, 0f) : closedRotation;
        }

        public void SetOpenAngle(float angle)
        {
            openAngle = angle;
            if (isOpen)
                targetRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
        }
    }
}
