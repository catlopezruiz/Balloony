using UnityEngine;
using UnityEngine.Tilemaps;

public class BombPlacement : MonoBehaviour
{
    public GameObject bombPrefab;
    public Tilemap levelGrid;

    public int maxBombs = 1;
    private int currentBombsPlaced = 0;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
                    bombExplosion.force = playerStats.explosionRange;
                    Debug.Log("Bomb spawned with range: " + bombExplosion.force);
                }
            }

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