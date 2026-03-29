using UnityEngine;
using UnityEngine.Tilemaps;

public class BombExplosion : MonoBehaviour
{
    public float timer = 2f;
    public float Force = 1f; // Make sure this is set to 3 on your Prefab!

    // The locks that stop the explosion from going through gray walls
    bool barrierUp = false;
    bool barrierDown = false;
    bool barrierLeft = false;
    bool barrierRight = false;

    private Tilemap destructibleTilemap;

    void Start()
    {
        // Automatically find the orange blocks map
        GameObject mapObject = GameObject.FindGameObjectWithTag("Destructible");

        if (mapObject != null)
        {
            destructibleTilemap = mapObject.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("The bomb couldn't find anything tagged 'Destructible'!");
        }

        StartCoroutine(ExplodeAfterTimer());
    }

    private System.Collections.IEnumerator ExplodeAfterTimer()
    {
        yield return new WaitForSeconds(timer);
        Vector2 pos = transform.position;

        // Step outward 1 tile at a time, up to your Force limit
        for (int i = 1; i <= Force; i++)
        {
            // --- CHECK UP ---
            if (!barrierUp)
            {
                // Find the exact center of the tile above us
                Vector2 checkPos = pos + (Vector2.up * i);
                
                // Drop a pin and grab EVERYTHING sitting on that exact spot
                Collider2D[] hitsUp = Physics2D.OverlapPointAll(checkPos);

                foreach (Collider2D hit in hitsUp)
                {
                    if (hit.CompareTag("Barrier"))
                    {
                        barrierUp = true; // Lock the door!
                    }
                    else if (hit.CompareTag("Destructible") && destructibleTilemap != null)
                    {
                        // Delete the orange tile at this exact spot
                        Vector3Int cellPosition = destructibleTilemap.WorldToCell(checkPos);
                        destructibleTilemap.SetTile(cellPosition, null);
                    }
                    else if (hit.CompareTag("Player"))
                    {
                        Debug.Log("Hit the Player!");
                    }
                }
            }

            // --- CHECK DOWN ---
            if (!barrierDown)
            {
                Vector2 checkPos = pos + (Vector2.down * i);
                Collider2D[] hitsDown = Physics2D.OverlapPointAll(checkPos);

                foreach (Collider2D hit in hitsDown)
                {
                    if (hit.CompareTag("Barrier"))
                    {
                        barrierDown = true;
                    }
                    else if (hit.CompareTag("Destructible") && destructibleTilemap != null)
                    {
                        Vector3Int cellPosition = destructibleTilemap.WorldToCell(checkPos);
                        destructibleTilemap.SetTile(cellPosition, null);
                    }
                    else if (hit.CompareTag("Player"))
                    {
                        Debug.Log("Hit the Player!");
                    }
                }
            }

            // --- CHECK LEFT ---
            if (!barrierLeft)
            {
                Vector2 checkPos = pos + (Vector2.left * i);
                Collider2D[] hitsLeft = Physics2D.OverlapPointAll(checkPos);

                foreach (Collider2D hit in hitsLeft)
                {
                    if (hit.CompareTag("Barrier"))
                    {
                        barrierLeft = true;
                    }
                    else if (hit.CompareTag("Destructible") && destructibleTilemap != null)
                    {
                        Vector3Int cellPosition = destructibleTilemap.WorldToCell(checkPos);
                        destructibleTilemap.SetTile(cellPosition, null);
                    }
                    else if (hit.CompareTag("Player"))
                    {
                        Debug.Log("Hit the Player!");
                    }
                }
            }

            // --- CHECK RIGHT ---
            if (!barrierRight)
            {
                Vector2 checkPos = pos + (Vector2.right * i);
                Collider2D[] hitsRight = Physics2D.OverlapPointAll(checkPos);

                foreach (Collider2D hit in hitsRight)
                {
                    if (hit.CompareTag("Barrier"))
                    {
                        barrierRight = true;
                    }
                    else if (hit.CompareTag("Destructible") && destructibleTilemap != null)
                    {
                        Vector3Int cellPosition = destructibleTilemap.WorldToCell(checkPos);
                        destructibleTilemap.SetTile(cellPosition, null);
                    }
                    else if (hit.CompareTag("Player"))
                    {
                        Debug.Log("Hit the Player!");
                    }
                }
            }
        }

        // Destroy the bomb itself
        Destroy(gameObject);
    }
}