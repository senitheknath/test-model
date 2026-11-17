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

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

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

        // Check if the ground check sphere overlaps with any ground layers
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (jumpAction.triggered && isGrounded)
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

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}
