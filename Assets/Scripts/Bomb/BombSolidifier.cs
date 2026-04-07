using UnityEngine;

public class BombSolidifier : MonoBehaviour
{
    private Collider2D bombCollider;

    void Start()
    {
        bombCollider = GetComponent<Collider2D>();
        // Always start as a ghost so nobody gets trapped!
        bombCollider.isTrigger = true; 
    }

    void Update()
    {
        // If the bomb is already solid, shut this script down to save memory
        if (!bombCollider.isTrigger)
            return;

        // Draw a radar box exactly the size of the bomb and see what it hits
        Collider2D[] thingsInside = Physics2D.OverlapBoxAll(transform.position, bombCollider.bounds.size, 0f);
        
        bool isPlayerInside = false;

        // Check if any of the things inside the bomb are players
        foreach (Collider2D hit in thingsInside)
        {
            if (hit.CompareTag("Player"))
            {
                isPlayerInside = true;
                break; // We found a player, no need to keep checking the list!
            }
        }

        // If the radar is completely clear of players, snap into a solid wall!
        if (isPlayerInside == false)
        {
            bombCollider.isTrigger = false;
        }
    }
}