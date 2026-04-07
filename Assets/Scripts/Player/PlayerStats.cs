using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int explosionRange = 1;
    public int maxBombs = 1;

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
        if (playerMovement != null)
        {
            playerMovement.moveSpeed += amount;
            Debug.Log("Speed is now: " + playerMovement.moveSpeed);
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