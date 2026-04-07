using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    public enum PowerUpType
    {
        Speed,
        Range,
        Bomb
    }

    public PowerUpType powerUpType;

    public float speedAmount = 1f;
    public int rangeAmount = 1;
    public int bombAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogError("Player touched power-up but has no PlayerStats!");
            return;
        }

        Debug.Log("Picked up: " + powerUpType);

        switch (powerUpType)
        {
            case PowerUpType.Speed:
                stats.IncreaseSpeed(speedAmount);
                break;

            case PowerUpType.Range:
                stats.IncreaseRange(rangeAmount);
                break;

            case PowerUpType.Bomb:
                stats.IncreaseMaxBombs(bombAmount);
                break;
        }

        Destroy(gameObject);
    }
}