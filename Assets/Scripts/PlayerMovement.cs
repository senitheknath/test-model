using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private CharacterController CC;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
