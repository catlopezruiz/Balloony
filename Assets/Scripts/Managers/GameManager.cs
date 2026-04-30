using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject playerPrefab;
    public GameObject aiPrefab;

    public Transform[] playerSpawns;
    public Transform[] aiSpawns;

    [Header("Win Detection")]
    public PlayerState[] players;
    public TextMeshProUGUI winText;

    private bool gameEnded = false;

    void Start()
    {
        SetupGame();

        Invoke(nameof(FindPlayers), 0.1f);

        if (winText != null)
            winText.gameObject.SetActive(false);
    }

    void FindPlayers()
    {
        players = FindObjectsByType<PlayerState>(FindObjectsSortMode.None);
        Debug.Log("Found " + players.Length + " players for win detection.");
    }

    void SetupGame()
    {
        string gameMode = PlayerPrefs.GetString("GameMode", "LOCAL");

        Debug.Log("Game Mode: " + gameMode);

        if (gameMode == "VS_AI")
        {
            int aiCount = PlayerPrefs.GetInt("AICount", 1);

            Debug.Log("Spawning 1 player and " + aiCount + " AI.");

            SpawnPlayer(playerSpawns[0]);

            for (int i = 0; i < aiCount && i < aiSpawns.Length; i++)
            {
                SpawnAI(aiSpawns[i]);
            }
        }
        else if (gameMode == "LOCAL")
        {
            int playerCount = PlayerPrefs.GetInt("PlayerCount", 2);

            Debug.Log("Spawning " + playerCount + " local players.");

            for (int i = 0; i < playerCount && i < playerSpawns.Length; i++)
            {
                SpawnPlayer(playerSpawns[i]);
            }
        }
    }

    void SpawnPlayer(Transform spawnPoint)
    {
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("Spawned Player at " + spawnPoint.name);
    }

    void SpawnAI(Transform spawnPoint)
    {
        GameObject ai = Instantiate(aiPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("Spawned AI at " + spawnPoint.name);
    }

    void Update()
    {
        if (gameEnded) return;
        if (players == null || players.Length == 0) return;

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

        if (aliveCount <= 1 && players.Length > 1)
        {
            gameEnded = true;

            string message = "";

            if (lastAlive != null)
                message = lastAlive.gameObject.name + " Wins!";
            else
                message = "Draw!";

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