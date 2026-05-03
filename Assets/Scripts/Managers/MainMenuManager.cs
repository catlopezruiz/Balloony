using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject modePanel;
    public GameObject vsAIPanel;
    public GameObject localPlayPanel;
    public GameObject keybindPanel;
    public GameObject comingSoonText;

    public string gameSceneName = "MainGameScene";

    private int selectedLocalPlayerCount = 2;

    void Start()
    {
        ShowTitlePanel();

        if (comingSoonText != null)
            comingSoonText.SetActive(false);
    }

    public void ShowTitlePanel()
    {
        titlePanel.SetActive(true);
        modePanel.SetActive(false);
        vsAIPanel.SetActive(false);
        localPlayPanel.SetActive(false);
        keybindPanel.SetActive(false);

        if (comingSoonText != null)
            comingSoonText.SetActive(false);
    }

    public void ShowModePanel()
    {
        titlePanel.SetActive(false);
        modePanel.SetActive(true);
        vsAIPanel.SetActive(false);
        localPlayPanel.SetActive(false);
        keybindPanel.SetActive(false);

        if (comingSoonText != null)
            comingSoonText.SetActive(false);
    }

    public void ShowVsAIPanel()
    {
        titlePanel.SetActive(false);
        modePanel.SetActive(false);
        vsAIPanel.SetActive(true);
        localPlayPanel.SetActive(false);
        keybindPanel.SetActive(false);

        if (comingSoonText != null)
            comingSoonText.SetActive(false);
    }

    public void ShowLocalPlayPanel()
    {
        titlePanel.SetActive(false);
        modePanel.SetActive(false);
        vsAIPanel.SetActive(false);
        localPlayPanel.SetActive(true);
        keybindPanel.SetActive(false);

        if (comingSoonText != null)
            comingSoonText.SetActive(false);
    }

    public void ShowKeybindPanel(int playerCount)
    {
        selectedLocalPlayerCount = playerCount;

        titlePanel.SetActive(false);
        modePanel.SetActive(false);
        vsAIPanel.SetActive(false);
        localPlayPanel.SetActive(false);
        keybindPanel.SetActive(true);

        if (comingSoonText != null)
            comingSoonText.SetActive(false);

        Debug.Log("Selected local players: " + selectedLocalPlayerCount);
    }

    public void BackFromKeybinds()
    {
        keybindPanel.SetActive(false);
        localPlayPanel.SetActive(true);
    }

    public void PlayLocalGame()
    {
        PlayerPrefs.SetString("GameMode", "LOCAL");
        PlayerPrefs.SetInt("PlayerCount", selectedLocalPlayerCount);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }

    public void StartVsAI(int aiCount)
    {
        PlayerPrefs.SetString("GameMode", "VS_AI");
        PlayerPrefs.SetInt("AICount", aiCount);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }

    public void ShowComingSoon()
    {
        if (comingSoonText != null)
            comingSoonText.SetActive(true);
    }
}