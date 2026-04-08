using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerState[] players;

    public TextMeshProUGUI winText;

    private bool gameEnded = false;

    void Update()
    {
        if (gameEnded) return;

        int aliveCount = 0;
        PlayerState lastAlive = null;

        foreach (PlayerState player in players)
        {
            if (player != null && !player.IsDead())
            {
                aliveCount++;
                lastAlive = player;
            }
        }

        if (aliveCount <= 1)
        {
            gameEnded = true;

            string message = "";

            if (lastAlive != null)
            {
                message = lastAlive.gameObject.name + " Wins!";
            }
            else
            {
                message = "Draw!";
            }

            ShowWinText(message);
        }
    }

    void ShowWinText(string message)
    {
        if (winText != null)
        {
            winText.gameObject.SetActive(true);
            winText.text = message;
        }

        Debug.Log(message);
    }
}