using UnityEngine;
using UnityEngine.InputSystem;

public class playerLook : MonoBehaviour
{
    [Header("References")]
    public Transform playerBody;

    [Header("Settings")]
    public float mouseSensitivity = 0.1f;
    public float maxLookAngle = 80f;

    private float xRotation = 0f;
    private Vector2 lookInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Lock cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current != null)
        {
            lookInput = Mouse.current.delta.ReadValue() * mouseSensitivity;

            float mouseX = lookInput.x;
            float mouseY = lookInput.y;

            // Vertical look (Camera pitch)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Horizontal look (Player body yaw)
            if (playerBody != null)
            {
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }
    }
}
