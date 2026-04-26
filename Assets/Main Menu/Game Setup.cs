using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [Header("Human Players")]
    public GameObject player1;
    public GameObject player2;

    [Header("AI Players")]
    public GameObject ai1;
    public GameObject ai2;
    public GameObject ai3;

    void Start()
    {
        // 1. Setup Humans
        player1.SetActive(true); // Player 1 always exists
        
        // Turn on Player 2 only if the memory bridge says they chose 2 players
        if (GameSettings.HumanPlayers == 2) player2.SetActive(true);
        else player2.SetActive(false);

        // 2. Setup AI
        GameObject[] allAIs = { ai1, ai2, ai3 };

        for (int i = 0; i < allAIs.Length; i++)
        {
            // If this AI is within the number they requested, turn it on
            if (i < GameSettings.AIPlayers)
            {
                allAIs[i].SetActive(true);
                
                // Inject the chosen difficulty straight into the AI's brain!
                AIbehaviour aiScript = allAIs[i].GetComponent<AIbehaviour>();
                if (aiScript != null)
                {
                    aiScript.difficulty = GameSettings.AIDifficultySetting;
                }
            }
            else
            {
                // Turn off extra AI
                allAIs[i].SetActive(false); 
            }
        }
    }
}