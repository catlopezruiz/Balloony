using UnityEngine;
using UnityEngine.Tilemaps;

public class BombPlacement : MonoBehaviour
{
    public GameObject bombPrefab;
    public Tilemap levelGrid;

    // The Magic Fix! This lets us set the button in the Inspector.
    [Header("Controls")]
    public KeyCode placeBombKey = KeyCode.Space;

    public int maxBombs = 1;
    
    // Just one variable! Unity keeps a separate copy of this for each player.
    private int currentBombsPlaced = 0; 

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        // Now it only listens to the specific key assigned in the Inspector!
        if (Input.GetKeyDown(placeBombKey))
        {
            if (currentBombsPlaced >= maxBombs)
                return;

            Vector3 playerPos = transform.position;
            Vector3Int cellPosition = levelGrid.WorldToCell(playerPos);
            Vector3 centerPos = levelGrid.GetCellCenterWorld(cellPosition);

            GameObject bomb = Instantiate(bombPrefab, centerPos, Quaternion.identity);

            BombExplosion bombExplosion = bomb.GetComponent<BombExplosion>();
            if (bombExplosion != null)
            {
                bombExplosion.bombPlacer = this;

                if (playerStats != null)
                {
                    // Note: Ensure this matches the capitalization in your BombExplosion script (Force vs force)
                    bombExplosion.force = playerStats.explosionRange;
                    Debug.Log("Bomb spawned with range: " + bombExplosion.force);
                }
            }

            // Adds 1 to THIS specific player's bomb count
            currentBombsPlaced++;
        }
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