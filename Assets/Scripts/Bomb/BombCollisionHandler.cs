using UnityEngine;

public class BombCollisionHandler : MonoBehaviour
{
    private Collider2D bombCollider;
    private Collider2D playerCollider;
    private bool playerHasLeft = false;

    void Start()
    {
        bombCollider = GetComponent<Collider2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();

            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(bombCollider, playerCollider, true);
            }
        }
    }

    void Update()
    {
        if (playerCollider == null || bombCollider == null || playerHasLeft) return;

        if (!bombCollider.bounds.Intersects(playerCollider.bounds))
        {
            Physics2D.IgnoreCollision(bombCollider, playerCollider, false);
            playerHasLeft = true;
        }
    }
}