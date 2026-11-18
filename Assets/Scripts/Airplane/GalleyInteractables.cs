using UnityEngine;

namespace AirplaneBuilder
{
    public class HingeInteractable : MonoBehaviour
    {
        [Header("Hinge Settings")]
        [SerializeField] private Vector3 rotationAxis = Vector3.up;
        [SerializeField] private float openAngle = 90f;
        [SerializeField] private float smoothSpeed = 6f;
        [SerializeField] private bool isOpen = false;

        private Quaternion closedRotation;
        private Quaternion targetRotation;

        public bool IsOpen => isOpen;

        private void Awake()
        {
            closedRotation = transform.localRotation;
            targetRotation = isOpen ? closedRotation * Quaternion.AngleAxis(openAngle, rotationAxis) : closedRotation;
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
            targetRotation = isOpen ? closedRotation * Quaternion.AngleAxis(openAngle, rotationAxis) : closedRotation;
        }

        public void SetOpenAngle(float angle)
        {
            openAngle = angle;
            if (isOpen)
                targetRotation = closedRotation * Quaternion.AngleAxis(openAngle, rotationAxis);
        }

        public void SetAxis(Vector3 axis)
        {
            rotationAxis = axis;
        }

        public void SetOpenImmediate(bool open)
        {
            isOpen = open;
            if (closedRotation == default && transform.localRotation != Quaternion.identity)
                closedRotation = transform.localRotation;
            targetRotation = isOpen ? closedRotation * Quaternion.AngleAxis(openAngle, rotationAxis) : closedRotation;
            transform.localRotation = targetRotation;
        }
    }

    public class SlideInteractable : MonoBehaviour
    {
        [Header("Slide Settings")]
        [SerializeField] private Vector3 slideAxis = Vector3.forward;
        [SerializeField] private float slideDistance = 0.40f;
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private bool isOpen = false;

        private Vector3 closedPosition;
        private Vector3 targetPosition;

        public bool IsOpen => isOpen;

        private void Awake()
        {
            closedPosition = transform.localPosition;
            targetPosition = isOpen ? closedPosition + slideAxis.normalized * slideDistance : closedPosition;
        }

        private void Update()
        {
            if (Vector3.Distance(transform.localPosition, targetPosition) > 0.001f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * smoothSpeed);
            }
            else
            {
                transform.localPosition = targetPosition;
            }
        }

        public void Interact()
        {
            isOpen = !isOpen;
            targetPosition = isOpen ? closedPosition + slideAxis.normalized * slideDistance : closedPosition;
        }

        public void SetSlideDistance(float dist)
        {
            slideDistance = dist;
            if (isOpen)
                targetPosition = closedPosition + slideAxis.normalized * slideDistance;
        }

        public void SetAxis(Vector3 axis)
        {
            slideAxis = axis;
        }

        public void SetOpenImmediate(bool open)
        {
            isOpen = open;
            if (closedPosition == default && transform.localPosition != Vector3.zero)
                closedPosition = transform.localPosition;
            targetPosition = isOpen ? closedPosition + slideAxis.normalized * slideDistance : closedPosition;
            transform.localPosition = targetPosition;
        }
    }
}
