using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int explosionRange = 1;
    public int maxBombs = 1;
    public float maxSpeed = 8f;

    private PlayerMovement playerMovement;
    private BombPlacement bombPlacement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        bombPlacement = GetComponent<BombPlacement>();

        if (bombPlacement != null)
        {
            bombPlacement.maxBombs = maxBombs;
        }
    }

    public void IncreaseSpeed(float amount)
    {
        PlayerMovement p1 = GetComponent<PlayerMovement>();
        Player2Movement p2 = GetComponent<Player2Movement>();

        if (p1 != null)
        {
            p1.moveSpeed = Mathf.Min(p1.moveSpeed + amount, maxSpeed);
            Debug.Log("P1 Speed is now: " + p1.moveSpeed);
        }

        if (p2 != null)
        {
            p2.moveSpeed = Mathf.Min(p2.moveSpeed + amount, maxSpeed);
            Debug.Log("P2 Speed is now: " + p2.moveSpeed);
        }
    }

    public void IncreaseRange(int amount)
    {
        explosionRange += amount;
        Debug.Log("Range is now: " + explosionRange);
    }

    public void IncreaseMaxBombs(int amount)
    {
        maxBombs += amount;

        if (bombPlacement != null)
        {
            bombPlacement.maxBombs = maxBombs;
        }

        Debug.Log("Max bombs is now: " + maxBombs);
    }
}