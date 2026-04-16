using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    [Header("Player Controls")]
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;

    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = 0f;
        movement.y = 0f;

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
}