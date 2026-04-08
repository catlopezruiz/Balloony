using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour
{
    public float stunDuration = 5f;

    private bool isStunned = false;
    private bool isDead = false;
    private Coroutine stunCoroutine;

    private PlayerMovement player1Movement;
    private Player2Movement player2Movement;
    private BombPlacement bombPlacementScript;
    private SpriteRenderer spriteRenderer;

    public Color normalColor = Color.white;
    public Color stunnedColor = Color.cyan;

    private float lastExplosionHitTime = -10f;
    public float explosionHitCooldown = 0.15f;

    void Awake()
    {
        player1Movement = GetComponent<PlayerMovement>();
        player2Movement = GetComponent<Player2Movement>();
        bombPlacementScript = GetComponent<BombPlacement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool IsStunned()
    {
        return isStunned;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void HitByExplosion()
    {
        if (isDead) return;

        if (Time.time - lastExplosionHitTime < explosionHitCooldown)
            return;

        lastExplosionHitTime = Time.time;

        if (isStunned)
        {
            Die();
            return;
        }

        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }

        stunCoroutine = StartCoroutine(StunRoutine());
    }

    private IEnumerator StunRoutine()
    {
        isStunned = true;

        if (player1Movement != null)
            player1Movement.enabled = false;

        if (player2Movement != null)
            player2Movement.enabled = false;

        if (bombPlacementScript != null)
            bombPlacementScript.enabled = false;

        if (spriteRenderer != null)
            spriteRenderer.color = stunnedColor;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        yield return new WaitForSeconds(stunDuration);

        if (!isDead)
        {
            isStunned = false;

            if (player1Movement != null)
                player1Movement.enabled = true;

            if (player2Movement != null)
                player2Movement.enabled = true;

            if (bombPlacementScript != null)
                bombPlacementScript.enabled = true;

            if (spriteRenderer != null)
                spriteRenderer.color = normalColor;
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        isStunned = false;

        if (stunCoroutine != null)
            StopCoroutine(stunCoroutine);

        if (player1Movement != null)
            player1Movement.enabled = false;

        if (player2Movement != null)
            player2Movement.enabled = false;

        if (bombPlacementScript != null)
            bombPlacementScript.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        gameObject.SetActive(false);
    }
}