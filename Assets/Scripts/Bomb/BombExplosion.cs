using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class BombExplosion : MonoBehaviour
{
    public float timer = 2f;

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

        // ONLY check the 4 tiles directly next to the bomb
        CheckTile(bombPos + Vector2.up);
        CheckTile(bombPos + Vector2.down);
        CheckTile(bombPos + Vector2.left);
        CheckTile(bombPos + Vector2.right);

        Destroy(gameObject);
    }

    private void CheckTile(Vector2 checkPos)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(checkPos);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Barrier"))
            {
                // Gray wall: stop here, do nothing
                return;
            }

            if (hit.CompareTag("Destructible") && destructibleTilemap != null)
            {
                // Orange block: break only this one block
                Vector3Int cellPosition = destructibleTilemap.WorldToCell(checkPos);
                destructibleTilemap.SetTile(cellPosition, null);
                return;
            }

            if (hit.CompareTag("Player"))
            {
                Debug.Log("Hit the Player!");
            }
        }
    }
}