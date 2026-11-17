using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private CapsuleCollider collider;

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction jumpAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        jumpAction = playerInput.actions["Jump"];
        moveAction = playerInput.actions["Move"];

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {   
        Move();

        if (jumpAction.triggered)
        {
            Jump();
        }
    }

    public void Move()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();

        // Convert local/input directions to world directions relative to the player's rotation
        Vector3 moveDirection = transform.right * move.x + transform.forward * move.y;

        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
