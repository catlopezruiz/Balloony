using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Player Controls")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    [Header("Stats")]
    public int maxBombs = 1;
    public int bombRange = 1;

    public PlayerStatsUI statsUI;

    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (statsUI != null)
        {
            statsUI.SetStats((int)moveSpeed, maxBombs, bombRange);
        }
    }

    void Update()
    {
        movement = Vector2.zero;

        if (Input.GetKey(upKey)) movement.y = 1f;
        if (Input.GetKey(downKey)) movement.y = -1f;
        if (Input.GetKey(rightKey)) movement.x = 1f;
        if (Input.GetKey(leftKey)) movement.x = -1f;

        movement = movement.normalized;

        // Animation
        bool isMoving = movement.sqrMagnitude > 0.01f;

        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }

        // Flip sprite
        if (spriteRenderer != null)
        {
            if (movement.x < 0)
                spriteRenderer.flipX = true;
            else if (movement.x > 0)
                spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    // SPEED POWERUP
    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;

        if (statsUI != null)
        {
            statsUI.SetStats((int)moveSpeed, maxBombs, bombRange);
        }
    }

    // BOMB COUNT POWERUP
    public void IncreaseBombs(int amount)
    {
        maxBombs += amount;

        if (statsUI != null)
        {
            statsUI.SetStats((int)moveSpeed, maxBombs, bombRange);
        }
    }

    // RANGE POWERUP
    public void IncreaseRange(int amount)
    {
        bombRange += amount;

        if (statsUI != null)
        {
            statsUI.SetStats((int)moveSpeed, maxBombs, bombRange);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpeedPowerUp"))
        {
            IncreaseSpeed(1f);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BombPowerUp"))
        {
            IncreaseBombs(1);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("RangePowerUp"))
        {
            IncreaseRange(1);
            Destroy(other.gameObject);
        }
    }
}