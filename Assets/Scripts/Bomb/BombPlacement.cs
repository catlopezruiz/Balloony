using UnityEngine;
using UnityEngine.Tilemaps; // We need this to talk to the grid!

public class BombPlacement : MonoBehaviour
{
    public GameObject bombPrefab;
    
    // Add a slot to drag your Tilemap into, just like the explosion script
    public Tilemap levelGrid; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 1. Get where the player is standing right now
            Vector3 playerPos = transform.position;

            // 2. Ask the Tilemap: "What cell coordinate is the player inside?"
            Vector3Int cellPosition = levelGrid.WorldToCell(playerPos);

            // 3. Ask the Tilemap: "What is the exact world center of that specific cell?"
            Vector3 centerPos = levelGrid.GetCellCenterWorld(cellPosition);

            // 4. Spawn the bomb at that perfect center point!
            Instantiate(bombPrefab, centerPos, Quaternion.identity);
        }
    }
}