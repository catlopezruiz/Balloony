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

            SpawnPlayer(playerSpawns[0], 1);

            for (int i = 0; i < aiCount && i < aiSpawns.Length; i++)
            {
                SpawnAI(aiSpawns[i]);
            }
        }
        else if (gameMode == "LOCAL")
        {
            int playerCount = PlayerPrefs.GetInt("PlayerCount", 2);

            Debug.Log("Requested local players: " + playerCount);
            Debug.Log("Player spawns available: " + playerSpawns.Length);

            int amountToSpawn = Mathf.Min(playerCount, playerSpawns.Length);

            for (int i = 0; i < amountToSpawn; i++)
            {
                SpawnPlayer(playerSpawns[i], i + 1);
            }

            if (playerCount > playerSpawns.Length)
            {
                Debug.LogWarning("Not enough spawn points! Add more Player Spawns in the GameManager Inspector.");
            }
        }
    }

    void SpawnPlayer(Transform spawnPoint, int playerNumber)
    {
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        player.name = "Player " + playerNumber;

        AssignLocalControls(player, playerNumber);

        Debug.Log("Spawned Player " + playerNumber + " at " + spawnPoint.name);
    }

    void AssignLocalControls(GameObject player, int playerNumber)
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        BombPlacement bombPlacement = player.GetComponent<BombPlacement>();

        if (movement == null)
        {
            Debug.LogWarning(player.name + " is missing PlayerMovement.");
            return;
        }

        if (bombPlacement == null)
        {
            Debug.LogWarning(player.name + " is missing BombPlacement.");
            return;
        }

        if (playerNumber == 1)
        {
            movement.upKey = KeyCode.W;
            movement.downKey = KeyCode.S;
            movement.leftKey = KeyCode.A;
            movement.rightKey = KeyCode.D;
            bombPlacement.placeBombKey = KeyCode.Space;
        }
        else if (playerNumber == 2)
        {
            movement.upKey = KeyCode.UpArrow;
            movement.downKey = KeyCode.DownArrow;
            movement.leftKey = KeyCode.LeftArrow;
            movement.rightKey = KeyCode.RightArrow;
            bombPlacement.placeBombKey = KeyCode.RightShift;
        }
        else if (playerNumber == 3)
        {
            movement.upKey = KeyCode.I;
            movement.downKey = KeyCode.K;
            movement.leftKey = KeyCode.J;
            movement.rightKey = KeyCode.L;
            bombPlacement.placeBombKey = KeyCode.U;
        }
        else if (playerNumber == 4)
        {
            movement.upKey = KeyCode.T;
            movement.downKey = KeyCode.G;
            movement.leftKey = KeyCode.F;
            movement.rightKey = KeyCode.H;
            bombPlacement.placeBombKey = KeyCode.Y;
        }
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