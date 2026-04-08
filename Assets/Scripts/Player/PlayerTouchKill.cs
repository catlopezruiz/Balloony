using UnityEngine;

public class PlayerTouchKill : MonoBehaviour
{
    private PlayerState myState;

    void Awake()
    {
        myState = GetComponent<PlayerState>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (myState != null && myState.IsStunned() && !myState.IsDead())
        {
            myState.Die();
        }
    }
}