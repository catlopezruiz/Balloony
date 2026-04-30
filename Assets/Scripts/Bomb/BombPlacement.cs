using UnityEngine;
using UnityEngine.Tilemaps;

public class BombPlacement : MonoBehaviour
{
    public GameObject bombPrefab;
    public Tilemap levelGrid;

    [Header("Controls")]
    public KeyCode placeBombKey = KeyCode.Space;

    [Header("Bomb Detection")]
    public LayerMask bombLayer;
    public float bombCheckRadius = 0.2f;

    [Header("Bomb Timing")]
    public float bombHoldDelay = 0.2f;

    public int maxBombs = 1;

    private int currentBombsPlaced = 0;
    private float nextBombTime = 0f;
    private PlayerStats playerStats;

void Start()
{
    playerStats = GetComponent<PlayerStats>();

    if (levelGrid == null)
    {
        GameObject gridObject = GameObject.FindGameObjectWithTag("Grid");

        if (gridObject != null)
        {
            levelGrid = gridObject.GetComponent<Tilemap>();

            if (levelGrid == null)
            {
                levelGrid = gridObject.GetComponentInChildren<Tilemap>();
            }
        }
    }

    if (levelGrid == null)
    {
        Tilemap[] tilemaps = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);

        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.gameObject.name == "Walkable")
            {
                levelGrid = tilemap;
                break;
            }
        }
    }

    if (levelGrid == null)
    {
        Debug.LogError("Level Grid was not found. Make sure the Walkable Tilemap exists and is named Walkable.");
    }
}

    void Update()
    {
        if (Input.GetKey(placeBombKey) && Time.time >= nextBombTime)
        {
            TryPlaceBomb();
        }
    }

    public void TryPlaceBomb()
    {
        // 🔥 SAFETY CHECK
        if (levelGrid == null)
        {
            Debug.LogError("Level Grid is missing. Cannot place bomb.");
            return;
        }

        if (currentBombsPlaced >= maxBombs)
            return;

        Vector3 playerPos = transform.position;
        Vector3Int cellPosition = levelGrid.WorldToCell(playerPos);
        Vector3 centerPos = levelGrid.GetCellCenterWorld(cellPosition);

        Collider2D existingBomb = Physics2D.OverlapCircle(centerPos, bombCheckRadius, bombLayer);

        if (existingBomb != null)
            return;

        GameObject bomb = Instantiate(bombPrefab, centerPos, Quaternion.identity);

        BombExplosion bombExplosion = bomb.GetComponent<BombExplosion>();
        if (bombExplosion != null)
        {
            bombExplosion.bombPlacer = this;

            if (playerStats != null)
            {
                bombExplosion.force = playerStats.explosionRange;
                Debug.Log("Bomb spawned with range: " + bombExplosion.force);
            }
        }

        currentBombsPlaced++;
        nextBombTime = Time.time + bombHoldDelay;
    }

    public void PlaceBomb()
    {
        TryPlaceBomb();
    }

    public void BombDestroyed()
    {
        currentBombsPlaced--;

        if (currentBombsPlaced < 0)
        {
            currentBombsPlaced = 0;
        }
    }
}