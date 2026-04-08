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

    public Sprite normalBalloonSprite;
    public Sprite poppedBalloonSprite;
    public float poppedDuration = 0.15f;

    private Tilemap destructibleTilemap;
    private SpriteRenderer spriteRenderer;

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

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && normalBalloonSprite != null)
        {
            spriteRenderer.sprite = normalBalloonSprite;
        }

        StartCoroutine(ExplodeAfterTimer());
    }

    private IEnumerator ExplodeAfterTimer()
    {
        yield return new WaitForSeconds(timer);

        if (spriteRenderer != null && poppedBalloonSprite != null)
        {
            spriteRenderer.sprite = poppedBalloonSprite;
        }

        yield return new WaitForSeconds(poppedDuration);

        Vector2 bombPos = transform.position;

        ExplodeDirection(bombPos, Vector2.up);
        ExplodeDirection(bombPos, Vector2.down);
        ExplodeDirection(bombPos, Vector2.left);
        ExplodeDirection(bombPos, Vector2.right);

        if (bombPlacer != null)
        {
            bombPlacer.BombDestroyed();
        }

        Destroy(gameObject);
    }

    private void ExplodeDirection(Vector2 origin, Vector2 direction)
    {
        for (int i = 1; i <= force; i++)
        {
            Vector2 checkPos = origin + direction * i;

            bool stopExplosion = CheckTile(checkPos);

            if (stopExplosion)
            {
                break;
            }
        }
    }

    private bool CheckTile(Vector2 checkPos)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(checkPos);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Barrier"))
            {
                return true;
            }

            if (hit.CompareTag("Destructible") && destructibleTilemap != null)
            {
                Vector3Int cellPosition = destructibleTilemap.WorldToCell(checkPos);
                destructibleTilemap.SetTile(cellPosition, null);

                TrySpawnPowerUp(cellPosition);
                return true;
            }

            if (hit.CompareTag("Player"))
            {
                Debug.Log("Hit the Player!");
            }
        }

        return false;
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