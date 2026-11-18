using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace AirplaneBuilder
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Look")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float mouseSensitivity = 0.15f;
        [SerializeField] private float minPitch = -80f;
        [SerializeField] private float maxPitch = 80f;

        [Header("Interaction")]
        [SerializeField] private float interactRange = 2.5f;

        private CharacterController characterController;
        private float cameraPitch = 0f;
        private float verticalVelocity = 0f;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();

            if (cameraTransform == null)
            {
                Camera cam = GetComponentInChildren<Camera>();
                if (cam != null) cameraTransform = cam.transform;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleLook();
            HandleMovement();
            HandleInteraction();

            // Toggle cursor lock with Escape
            if (WasKeyJustPressed(KeyCode.Escape))
            {
                Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = (Cursor.lockState != CursorLockMode.Locked);
            }
        }

        private void HandleLook()
        {
            if (Cursor.lockState != CursorLockMode.Locked) return;

            Vector2 look = GetLookInput();
            float mouseX = look.x * mouseSensitivity;
            float mouseY = look.y * mouseSensitivity;

            cameraPitch -= mouseY;
            cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);

            if (cameraTransform != null)
                cameraTransform.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);

            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleMovement()
        {
            Vector2 moveInput = GetMoveInput();
            Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

            if (characterController.isGrounded)
            {
                verticalVelocity = -0.5f;
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            Vector3 velocity = moveDirection * moveSpeed;
            velocity.y = verticalVelocity;

            characterController.Move(velocity * Time.deltaTime);
        }

        private void HandleInteraction()
        {
            if (!WasInteractPressed()) return;

            Transform rayOrigin = cameraTransform != null ? cameraTransform : transform;
            Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                DoorInteractable door = hit.collider.GetComponentInParent<DoorInteractable>();
                if (door != null)
                {
                    door.Interact();
                }
            }
        }

        private Vector2 GetMoveInput()
        {
#if ENABLE_INPUT_SYSTEM
            Vector2 input = Vector2.zero;
            if (Keyboard.current != null)
            {
                if (Keyboard.current.wKey.isPressed) input.y += 1f;
                if (Keyboard.current.sKey.isPressed) input.y -= 1f;
                if (Keyboard.current.aKey.isPressed) input.x -= 1f;
                if (Keyboard.current.dKey.isPressed) input.x += 1f;
            }
            return input.normalized;
#else
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            return new Vector2(x, y).normalized;
#endif
        }

        private Vector2 GetLookInput()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                return Mouse.current.delta.ReadValue();
            }
            return Vector2.zero;
#else
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 10f;
#endif
        }

        private bool WasInteractPressed()
        {
#if ENABLE_INPUT_SYSTEM
            bool eKey = Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
            bool click = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
            return eKey || click;
#else
            return Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0);
#endif
        }

        private bool WasKeyJustPressed(KeyCode key)
        {
#if ENABLE_INPUT_SYSTEM
            if (key == KeyCode.Escape)
                return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
            return false;
#else
            return Input.GetKeyDown(key);
#endif
        }
    }
}
