using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class BombExplosion : MonoBehaviour
{
    public float timer = 2f;
    public int force = 1;

    public BombPlacement bombPlacer;

    public GameObject speedPowerUpPrefab;
    public GameObject rangePowerUpPrefab;
    public GameObject bombPowerUpPrefab;
    public float powerUpSpawnChance = 0.4f;

    private Tilemap destructibleTilemap;

    void Start()
    {
        GameObject mapObject = GameObject.FindGameObjectWithTag("Destructible");

        if (mapObject != null)
        {
            destructibleTilemap = mapObject.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Could not find a Tilemap tagged 'Destructible'!");
        }

        StartCoroutine(ExplodeAfterTimer());
    }

    private IEnumerator ExplodeAfterTimer()
    {
        yield return new WaitForSeconds(timer);

        Vector2 bombPos = transform.position;

        CheckTile(bombPos + Vector2.up);
        CheckTile(bombPos + Vector2.down);
        CheckTile(bombPos + Vector2.left);
        CheckTile(bombPos + Vector2.right);

        if (bombPlacer != null)
        {
            bombPlacer.BombDestroyed();
        }

        Destroy(gameObject);
    }

    private void CheckTile(Vector2 checkPos)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(checkPos);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Barrier"))
            {
                return;
            }

            if (hit.CompareTag("Destructible") && destructibleTilemap != null)
            {
                Vector3Int cellPosition = destructibleTilemap.WorldToCell(checkPos);
                destructibleTilemap.SetTile(cellPosition, null);

                TrySpawnPowerUp(cellPosition);
                return;
            }

            if (hit.CompareTag("Player"))
            {
                Debug.Log("Hit the Player!");
            }
        }
    }

    private void TrySpawnPowerUp(Vector3Int cellPosition)
    {
        if (Random.value > powerUpSpawnChance)
            return;

        int randomChoice = Random.Range(0, 3);
        GameObject prefabToSpawn = null;

        if (randomChoice == 0)
            prefabToSpawn = speedPowerUpPrefab;
        else if (randomChoice == 1)
            prefabToSpawn = rangePowerUpPrefab;
        else
            prefabToSpawn = bombPowerUpPrefab;

        if (prefabToSpawn != null)
        {
            Vector3 spawnPos = destructibleTilemap.GetCellCenterWorld(cellPosition);
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        }
    }
}