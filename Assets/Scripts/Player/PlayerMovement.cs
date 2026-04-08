using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Player Controls")]
    public KeyCode WKey = KeyCode.W;
    public KeyCode SKey = KeyCode.S;
    public KeyCode AKey = KeyCode.A;
    public KeyCode DKey = KeyCode.D;

    [Header("Stats")]
    public int maxBombs = 1;
    public int bombRange = 1;

    public PlayerStatsUI statsUI;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Initialize UI
        if (statsUI != null)
        {
            statsUI.SetStats((int)moveSpeed, maxBombs, bombRange);
        }
    }

    void Update()
    {
        movement.x = 0f;
        movement.y = 0f;

        if (Input.GetKey(WKey)) movement.y = 1f;
        if (Input.GetKey(SKey)) movement.y = -1f;
        if (Input.GetKey(DKey)) movement.x = 1f;
        if (Input.GetKey(AKey)) movement.x = -1f;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * moveSpeed;
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