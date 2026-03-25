using UnityEngine;
using UnityEngine.Tilemaps; // Required to talk to the Tilemap

public class BombExplosion : MonoBehaviour
{
    public BombPlacement BombPlacement;
    public float timer = 2f;
    
    // The reference to your orange blocks layer
    public Tilemap destructibleTilemap; 

    void Start()
    {
        StartCoroutine(ExplodeAfterTimer());
    }

    private System.Collections.IEnumerator ExplodeAfterTimer()
    {
        // 1. Pause for the duration of the timer
        yield return new WaitForSeconds(timer);

        Vector2 pos = transform.position;

        // 2. Fire raycasts in all four directions
        RaycastHit2D hitUp = Physics2D.Raycast(pos, Vector2.up, 1f);
        RaycastHit2D hitDown = Physics2D.Raycast(pos, Vector2.down, 1f);
        RaycastHit2D hitLeft = Physics2D.Raycast(pos, Vector2.left, 1f);
        RaycastHit2D hitRight = Physics2D.Raycast(pos, Vector2.right, 1f);
        
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

        // 3. Remove the bomb
        Destroy(gameObject);
    }
}