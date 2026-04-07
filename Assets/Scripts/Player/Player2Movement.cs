using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float moveSpeed = 5f;

    // The [Header] tag just makes a nice bold title in your Unity Inspector!
    [Header("Player Controls")]
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Reset movement to zero every frame
        movement.x = 0f;
        movement.y = 0f;

        // 2. Check exactly which keys are being held down
        if (Input.GetKey(upKey)) 
        {
            movement.y = 1f;
        }
        if (Input.GetKey(downKey)) 
        {
            movement.y = -1f;
        }
        if (Input.GetKey(rightKey)) 
        {
            movement.x = 1f;
        }
        if (Input.GetKey(leftKey)) 
        {
            movement.x = -1f;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * moveSpeed;
    }

    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
    }
}