using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AIDifficulty { Easy, Medium, Hard, Impossible }

public class AIbehaviour : MonoBehaviour
{
    [Header("AI Settings")]
    public AIDifficulty difficulty = AIDifficulty.Medium;
    public float baseMoveSpeed = 4f;
    public float stunDuration = 5f;
    public float dangerVisionDistance = 4f; 
    
    private float currentSpeed;
    private float stunSpeedMultiplier = 0f; 
    private float powerupDetectionRange = 0f;
    private bool chasesPlayer = false;

    private Rigidbody2D rb;
    private Vector2 currentDirection;
    private bool isFullyStunned = false; 
    private BombPlacement bombPlacement; 
    private Transform currentTarget;

    // The list of tags we care about for gathering items
    private string[] powerUpTags = { "BombPowerUp", "RangePowerUp", "SpeedPowerUp" };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bombPlacement = GetComponent<BombPlacement>(); 
        currentSpeed = baseMoveSpeed;

        ApplyDifficultySettings();
        ChooseSafeDirection(); 
    }

    void ApplyDifficultySettings()
    {
        switch (difficulty)
        {
            case AIDifficulty.Easy:
                chasesPlayer = false;
                powerupDetectionRange = 0f; 
                stunSpeedMultiplier = 0f;   
                break;
            case AIDifficulty.Medium:
                chasesPlayer = true; 
                powerupDetectionRange = 5f; 
                stunSpeedMultiplier = 0f;   
                break;
            case AIDifficulty.Hard:
                chasesPlayer = true; 
                powerupDetectionRange = 15f; 
                stunSpeedMultiplier = 0.5f;  
                break;
            case AIDifficulty.Impossible:
                chasesPlayer = true; 
                powerupDetectionRange = 100f; 
                stunSpeedMultiplier = 0.8f;   
                break;
        }
    }

    void FixedUpdate()
    {
        if (isFullyStunned) return;

        rb.linearVelocity = currentDirection * currentSpeed;

        bool needsNewDirection = false;
        Vector2 lookAheadPoint = (Vector2)transform.position + (currentDirection * 0.6f);
        Collider2D shortHit = Physics2D.OverlapPoint(lookAheadPoint);

        if (shortHit != null)
        {
            if (shortHit.CompareTag("Barrier") || shortHit.CompareTag("Bomb"))
            {
                needsNewDirection = true;
            } 
            else if (shortHit.CompareTag("Destructible"))
            {
                if (bombPlacement != null) bombPlacement.PlaceBomb(); 
                needsNewDirection = true; 
            }
        } 

        if (!needsNewDirection)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, currentDirection, dangerVisionDistance);
            foreach (RaycastHit2D hit in hits)
            {
                if (IsTagAPowerUp(hit.collider.tag) || hit.collider.CompareTag("Player") || hit.collider.isTrigger) continue; 
                
                if (hit.collider.CompareTag("Bomb"))
                {
                    needsNewDirection = true;
                    break;
                }
                if (hit.collider.CompareTag("Barrier") || hit.collider.CompareTag("Destructible"))
                {
                    break; 
                }
            }
        }

        if (needsNewDirection)
        {
            ChooseSafeDirection();
        }
    }

    void FindBestTarget()
    {
        currentTarget = null;
        float closestDistance = Mathf.Infinity;

        if (powerupDetectionRange > 0)
        {
            // Scan for every type of power-up
            foreach (string tag in powerUpTags)
            {
                GameObject[] powerups = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject pu in powerups)
                {
                    float dist = Vector2.Distance(transform.position, pu.transform.position);
                    if (dist <= powerupDetectionRange && dist < closestDistance)
                    {
                        bool isSafeToGrab = true;
                        // For Hard/Impossible: Only go for it if we are closer than the human player
                        if (difficulty == AIDifficulty.Hard || difficulty == AIDifficulty.Impossible)
                        {
                             GameObject player = GameObject.FindGameObjectWithTag("Player"); 
                             if (player != null)
                             {
                                 float playerDist = Vector2.Distance(player.transform.position, pu.transform.position);
                                 if (playerDist < dist) isSafeToGrab = false; 
                             }
                        }

                        if (isSafeToGrab)
                        {
                            closestDistance = dist;
                            currentTarget = pu.transform;
                        }
                    }
                }
            }
        }

        if (currentTarget == null && chasesPlayer)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                if (p.gameObject != this.gameObject) 
                {
                    currentTarget = p.transform;
                    break; 
                }
            }
        }
    }

    void ChooseSafeDirection()
    {
        Vector2[] allDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        List<Vector2> completelySafePaths = new List<Vector2>();
        List<Vector2> dangerousButOpenPaths = new List<Vector2>();

        foreach (Vector2 dir in allDirections)
        {
            Vector2 nextStepPos = (Vector2)transform.position + (dir * 0.8f);
            Collider2D wallCheck = Physics2D.OverlapPoint(nextStepPos);
            
            bool isPhysicallyBlocked = wallCheck != null && (wallCheck.CompareTag("Barrier") || wallCheck.CompareTag("Destructible") || wallCheck.CompareTag("Bomb"));

            if (!isPhysicallyBlocked)
            {
                if (IsTileSafe(nextStepPos)) completelySafePaths.Add(dir);
                else dangerousButOpenPaths.Add(dir); 
            }
        }

        FindBestTarget(); 

        if (completelySafePaths.Count > 0)
        {
            currentDirection = PickBestDirectionTowardsTarget(completelySafePaths);
        }
        else if (dangerousButOpenPaths.Count > 0)
        {
            currentDirection = dangerousButOpenPaths[Random.Range(0, dangerousButOpenPaths.Count)];
        }
        else
        {
            currentDirection = Vector2.zero; 
        }
    }

    Vector2 PickBestDirectionTowardsTarget(List<Vector2> safePaths)
    {
        if (currentTarget == null) return safePaths[Random.Range(0, safePaths.Count)];

        Vector2 bestDir = safePaths[0];
        float closestDist = Mathf.Infinity;

        foreach (Vector2 dir in safePaths)
        {
            Vector2 imaginedPos = (Vector2)transform.position + dir;
            float distToTarget = Vector2.Distance(imaginedPos, currentTarget.position);

            if (distToTarget < closestDist)
            {
                closestDist = distToTarget;
                bestDir = dir;
            }
        }
        return bestDir;
    }

    bool IsTileSafe(Vector2 checkPos)
    {
        Vector2[] lookDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach (Vector2 lookDir in lookDirections)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(checkPos, lookDir, dangerVisionDistance);
            foreach (RaycastHit2D hit in hits)
            {
                if (IsTagAPowerUp(hit.collider.tag) || hit.collider.CompareTag("Player") || hit.collider.isTrigger) continue; 
                if (hit.collider.CompareTag("Bomb")) return false; 
                if (hit.collider.CompareTag("Barrier") || hit.collider.CompareTag("Destructible")) break; 
            }
        }
        return true; 
    }

    // Helper to check if a tag matches any of our power-up types
    private bool IsTagAPowerUp(string tag)
    {
        foreach (string puTag in powerUpTags)
        {
            if (tag == puTag) return true;
        }
        return false;
    }

    public void HitByExplosion()
    {
        if (!isFullyStunned) 
        {
            StartCoroutine(StunRoutine());
        }
    }

    private IEnumerator StunRoutine()
    {
        if (stunSpeedMultiplier == 0f) 
        {
            isFullyStunned = true;
            rb.linearVelocity = Vector2.zero; 
        }
        
        currentSpeed = baseMoveSpeed * stunSpeedMultiplier;
        yield return new WaitForSeconds(stunDuration); 
        
        currentSpeed = baseMoveSpeed;
        isFullyStunned = false;
        ChooseSafeDirection(); 
    }
}