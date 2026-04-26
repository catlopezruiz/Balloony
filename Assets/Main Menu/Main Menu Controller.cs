using UnityEngine;
using TMPro; // Required for TextMeshPro
using UnityEngine.SceneManagement; // Required to change scenes

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels (Group your buttons inside these)")]
    public GameObject playerSelectionPanel;
    public GameObject aiSelectionPanel;
    public GameObject difficultyPanel;

    [Header("AI Button Text")]
    public TextMeshProUGUI aiButton1Text;
    public TextMeshProUGUI aiButton2Text;
    public TextMeshProUGUI aiButton3Text;

    // Hidden variables to remember what the AI buttons currently mean
    private int aiOption1Value;
    private int aiOption2Value;
    private int aiOption3Value;

    void Start()
    {
        // When the menu loads, only show the Player selection screen
        playerSelectionPanel.SetActive(true);
        aiSelectionPanel.SetActive(false);
        difficultyPanel.SetActive(false);
    }

    // Connect this to your "1 Player" and "2 Player" buttons
    public void SelectHumanPlayers(int humans)
    {
        GameSettings.HumanPlayers = humans;

        // Change the text and meaning of the AI buttons based on the choice!
        if (humans == 1)
        {
            aiOption1Value = 1; aiButton1Text.text = "1 AI";
            aiOption2Value = 2; aiButton2Text.text = "2 AI";
            aiOption3Value = 3; aiButton3Text.text = "3 AI";
        }
        else // 2 Players
        {
            aiOption1Value = 0; aiButton1Text.text = "0 AI";
            aiOption2Value = 1; aiButton2Text.text = "1 AI";
            aiOption3Value = 2; aiButton3Text.text = "2 AI";
        }

        // Hide Player screen, Show AI screen
        playerSelectionPanel.SetActive(false);
        aiSelectionPanel.SetActive(true);
    }

    // Connect this to your 3 AI Buttons (Pass 1, 2, or 3 from the Inspector)
    public void SelectAICount(int buttonIndex)
    {
        if (buttonIndex == 1) GameSettings.AIPlayers = aiOption1Value;
        else if (buttonIndex == 2) GameSettings.AIPlayers = aiOption2Value;
        else if (buttonIndex == 3) GameSettings.AIPlayers = aiOption3Value;

        // Smart Logic: If they chose 2 Players and 0 AI, skip the difficulty screen!
        if (GameSettings.AIPlayers == 0)
        {
            StartGame();
        }
        else
        {
            aiSelectionPanel.SetActive(false);
            difficultyPanel.SetActive(true);
        }
    }

    // Connect this to your Difficulty Buttons (Pass 0=Easy, 1=Medium, etc.)
    public void SelectDifficulty(int diffIndex)
    {
        // Convert the simple number back into your custom AIDifficulty setting
        GameSettings.AIDifficultySetting = (AIDifficulty)diffIndex;
        StartGame();
    }

    private void StartGame()
    {
        // IMPORTANT: Change "MainGame" to the exact name of your game scene!
        SceneManager.LoadScene("MainGame"); 
    }
}