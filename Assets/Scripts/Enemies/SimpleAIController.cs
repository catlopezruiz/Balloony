using UnityEngine;
using UnityEngine.Tilemaps;

public class SimpleAIController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float decisionTime = 1f;
    public float bombDistance = 1.2f;

    [Header("Flee Settings")]
    public float fleeTime = 3.5f;

    [Header("Obstacle Detection")]
    public LayerMask obstacleLayer;
    public float wallCheckDistance = 1f;

    [Header("Bomb Danger Detection")]
    public LayerMask bombLayer;
    public float dangerCheckRadius = 8f;
    public float explosionDangerRange = 3f;
    public float sameRowColumnTolerance = 0.6f;

    private bool isFleeing = false;
    private Vector3 lastBombPosition;
    private float fleeTimer;

    private Rigidbody2D rb;
    private BombPlacement bombPlacement;
    private PlayerState playerState;

    private Tilemap destructibleTilemap;
    private Vector2 moveDirection;
    private float decisionTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bombPlacement = GetComponent<BombPlacement>();
        playerState = GetComponent<PlayerState>();

        FindDestructibleTilemap();
        PickGoal();
    }

    void Update()
    {
        if (playerState != null && playerState.IsStunned())
        {
            moveDirection = Vector2.zero;
            return;
        }

        if (IsInBombDanger())
        {
            isFleeing = true;
            fleeTimer = fleeTime;
            PickSafeDirection();
            return;
        }

        if (isFleeing)
        {
            fleeTimer -= Time.deltaTime;

            PickSafeDirection();

            if (fleeTimer <= 0)
                isFleeing = false;

            return;
        }

        decisionTimer -= Time.deltaTime;

        if (decisionTimer <= 0)
        {
            PickGoal();
        }

        TryPlaceBombNearTarget();
    }

    void FixedUpdate()
    {
        if (playerState != null && playerState.IsStunned())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (IsBlocked(moveDirection))
        {
            PickOpenDirection();
        }

        rb.linearVelocity = moveDirection * moveSpeed;
    }

    void FindDestructibleTilemap()
    {
        GameObject destructible = GameObject.FindGameObjectWithTag("Destructible");

        if (destructible != null)
            destructibleTilemap = destructible.GetComponent<Tilemap>();
    }

void PickGoal()
{
    decisionTimer = decisionTime;

    Transform nearestPlayer = FindNearestEnemyPlayer();

    if (nearestPlayer != null)
    {
        float distanceToPlayer = Vector2.Distance(transform.position, nearestPlayer.position);

        // ALWAYS prioritize attacking players (including other AIs)
        if (distanceToPlayer < 6f)
        {
            MoveToward(nearestPlayer.position);
            return;
        }
    }

    // If no players nearby → go for blocks
    Vector3? nearestBlock = FindNearestDestructibleBlock();

    if (nearestBlock.HasValue)
    {
        MoveToward(nearestBlock.Value);
        return;
    }

    PickRandomDirection();
}

    Transform FindNearestEnemyPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            if (player == gameObject) continue;

            PlayerState state = player.GetComponent<PlayerState>();
            if (state != null && state.IsDead()) continue;

            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = player.transform;
            }
        }

        return closest;
    }

    Vector3? FindNearestDestructibleBlock()
    {
        if (destructibleTilemap == null)
            return null;

        BoundsInt bounds = destructibleTilemap.cellBounds;

        Vector3? closestBlock = null;
        float closestDistance = Mathf.Infinity;

        foreach (Vector3Int cell in bounds.allPositionsWithin)
        {
            if (!destructibleTilemap.HasTile(cell))
                continue;

            Vector3 worldPos = destructibleTilemap.GetCellCenterWorld(cell);
            float distance = Vector2.Distance(transform.position, worldPos);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBlock = worldPos;
            }
        }

        return closestBlock;
    }

    void MoveToward(Vector3 target)
    {
        Vector2 direction = target - transform.position;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            moveDirection = direction.x > 0 ? Vector2.right : Vector2.left;
        else
            moveDirection = direction.y > 0 ? Vector2.up : Vector2.down;

        if (IsBlocked(moveDirection))
            PickOpenDirection();
    }

void TryPlaceBombNearTarget()
{
    if (bombPlacement == null || isFleeing || IsInBombDanger())
        return;

    Transform nearestEnemy = FindNearestEnemyPlayer();

    if (nearestEnemy != null)
    {
        float enemyDistance = Vector2.Distance(transform.position, nearestEnemy.position);

        // More aggressive bomb placement vs ANY enemy
        if (enemyDistance <= bombDistance)
        {
            PlaceBombAndFlee();
            return;
        }
    }

    // fallback: destroy blocks
    Vector3? nearestBlock = FindNearestDestructibleBlock();

    if (nearestBlock.HasValue)
    {
        float blockDistance = Vector2.Distance(transform.position, nearestBlock.Value);

        if (blockDistance <= bombDistance)
        {
            PlaceBombAndFlee();
        }
    }
}

    void PlaceBombAndFlee()
    {
        lastBombPosition = transform.position;

        bombPlacement.TryPlaceBomb();

        isFleeing = true;
        fleeTimer = fleeTime;

        PickSafeDirection();
    }

    bool IsInBombDanger()
    {
        return PositionIsDangerous(transform.position);
    }

    bool PositionIsDangerous(Vector3 position)
    {
        Collider2D[] bombs = Physics2D.OverlapCircleAll(position, dangerCheckRadius, bombLayer);

        foreach (Collider2D bomb in bombs)
        {
            Vector3 bombPos = bomb.transform.position;

            bool sameColumn = Mathf.Abs(position.x - bombPos.x) <= sameRowColumnTolerance;
            bool sameRow = Mathf.Abs(position.y - bombPos.y) <= sameRowColumnTolerance;

            float distance = Vector2.Distance(position, bombPos);

            if ((sameColumn || sameRow) && distance <= explosionDangerRange)
                return true;
        }

        return false;
    }

    bool DirectionStillDangerous(Vector2 direction)
    {
        Vector3 testPosition = transform.position + (Vector3)(direction.normalized * 1.5f);
        return PositionIsDangerous(testPosition);
    }

    void PickSafeDirection()
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        Vector2 bestDirection = Vector2.zero;
        float bestDangerDistance = -1f;

        foreach (Vector2 dir in directions)
        {
            if (IsBlocked(dir))
                continue;

            Vector3 testPosition = transform.position + (Vector3)(dir * 1.5f);

            if (!PositionIsDangerous(testPosition))
            {
                moveDirection = dir;
                return;
            }

            float dangerDistance = DistanceFromNearestBomb(testPosition);

            if (dangerDistance > bestDangerDistance)
            {
                bestDangerDistance = dangerDistance;
                bestDirection = dir;
            }
        }

        moveDirection = bestDirection;
    }

    float DistanceFromNearestBomb(Vector3 position)
    {
        Collider2D[] bombs = Physics2D.OverlapCircleAll(position, dangerCheckRadius, bombLayer);

        float closestDistance = Mathf.Infinity;

        foreach (Collider2D bomb in bombs)
        {
            float distance = Vector2.Distance(position, bomb.transform.position);

            if (distance < closestDistance)
                closestDistance = distance;
        }

        return closestDistance;
    }

    bool IsBlocked(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, obstacleLayer);
        return hit.collider != null;
    }

    void PickOpenDirection()
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 dir in directions)
        {
            if (!IsBlocked(dir))
            {
                moveDirection = dir;
                return;
            }
        }

        moveDirection = Vector2.zero;
    }

    void PickRandomDirection()
    {
        int choice = Random.Range(0, 5);

        if (choice == 0)
            moveDirection = Vector2.up;
        else if (choice == 1)
            moveDirection = Vector2.down;
        else if (choice == 2)
            moveDirection = Vector2.left;
        else if (choice == 3)
            moveDirection = Vector2.right;
        else
            moveDirection = Vector2.zero;
    }
}