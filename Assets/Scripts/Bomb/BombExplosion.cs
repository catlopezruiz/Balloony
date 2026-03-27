using UnityEngine;
using UnityEngine.Tilemaps; // Required to erase the tiles

public class BombExplosion : MonoBehaviour
{
    public float timer = 2f;
    
    // Drag your DestructibleMap into this slot on the Bomb Prefab in the Inspector
    private Tilemap destructibleTilemap; 
    public float Force = 1f;

    void Start()
    {
        GameObject mapObject = GameObject.FindGameObjectWithTag("Destructible");

        if (mapObject != null)
        {
            // Grab the actual Tilemap component from the object we found
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
        // 1. Pause for the timer
        yield return new WaitForSeconds(timer);

        Vector2 pos = transform.position;

        // 3. Fire the actual physics raycasts
        RaycastHit2D hitUp = Physics2D.Raycast(pos, Vector2.up, Force);
        RaycastHit2D hitDown = Physics2D.Raycast(pos, Vector2.down, Force);
        RaycastHit2D hitLeft = Physics2D.Raycast(pos, Vector2.left, Force);
        RaycastHit2D hitRight = Physics2D.Raycast(pos, Vector2.right, Force);
        
        // --- CHECK UP ---
        if (hitUp.collider != null)
        {
            if (hitUp.collider.CompareTag("Player"))
            {
                Debug.Log("Hit the Player!");
            }
            else if (hitUp.collider.CompareTag("Destructible"))
            {
                Vector2 hitPosition = hitUp.point + (Vector2.up * 0.1f);
                Vector3Int cellPosition = destructibleTilemap.WorldToCell(hitPosition);
                destructibleTilemap.SetTile(cellPosition, null);
            }
        }

        // --- CHECK DOWN ---
        if (hitDown.collider != null)
        {
            if (hitDown.collider.CompareTag("Player"))
            {
                Debug.Log("Hit the Player!");
            }
            else if (hitDown.collider.CompareTag("Destructible"))
            {
                Vector2 hitPosition = hitDown.point + (Vector2.down * 0.1f);
                Vector3Int cellPosition = destructibleTilemap.WorldToCell(hitPosition);
                destructibleTilemap.SetTile(cellPosition, null);
            }
        }

        // --- CHECK LEFT ---
        if (hitLeft.collider != null)
        {
            if (hitLeft.collider.CompareTag("Player"))
            {
                Debug.Log("Hit the Player!");
            }
            else if (hitLeft.collider.CompareTag("Destructible"))
            {
                Vector2 hitPosition = hitLeft.point + (Vector2.left * 0.1f);
                Vector3Int cellPosition = destructibleTilemap.WorldToCell(hitPosition);
                destructibleTilemap.SetTile(cellPosition, null);
            }
        }

        // --- CHECK RIGHT ---
        if (hitRight.collider != null)
        {
            if (hitRight.collider.CompareTag("Player"))
            {
                Debug.Log("Hit the Player!");
            }
            else if (hitRight.collider.CompareTag("Destructible"))
            {
                Vector2 hitPosition = hitRight.point + (Vector2.right * 0.1f);
                Vector3Int cellPosition = destructibleTilemap.WorldToCell(hitPosition);
                destructibleTilemap.SetTile(cellPosition, null);
            }
        }

        // 4. Destroy the bomb
        Destroy(gameObject);
    }
}