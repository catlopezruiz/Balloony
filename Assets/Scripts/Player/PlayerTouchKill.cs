using UnityEngine;

public class PlayerTouchKill : MonoBehaviour
{
    public float checkRadius = 0.35f;

    private PlayerState myState;

    void Awake()
    {
        myState = GetComponent<PlayerState>();
    }

    void Update()
    {
        if (myState == null || myState.IsDead()) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, checkRadius);

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Player")) continue;
            if (hit.gameObject == gameObject) continue;

            PlayerState otherState = hit.GetComponent<PlayerState>();

            if (otherState == null)
                otherState = hit.GetComponentInParent<PlayerState>();

            if (otherState == null || otherState.IsDead()) continue;

            if (myState.IsStunned())
            {
                myState.Die();
                return;
            }

            if (otherState.IsStunned())
            {
                otherState.Die();
                return;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}