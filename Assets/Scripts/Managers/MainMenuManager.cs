using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject modePanel;
    public GameObject vsAIPanel;
    public GameObject localPlayPanel;

    public GameObject comingSoonText;

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

        if (comingSoonText != null)
            comingSoonText.SetActive(false);
    }

    public void ShowModePanel()
    {
        titlePanel.SetActive(false);
        modePanel.SetActive(true);
        vsAIPanel.SetActive(false);
        localPlayPanel.SetActive(false);

        if (comingSoonText != null)
            comingSoonText.SetActive(false);
    }

    public void ShowVsAIPanel()
    {
        titlePanel.SetActive(false);
        modePanel.SetActive(false);
        vsAIPanel.SetActive(true);
        localPlayPanel.SetActive(false);

        if (comingSoonText != null)
            comingSoonText.SetActive(false);
    }

    public void ShowLocalPlayPanel()
    {
        titlePanel.SetActive(false);
        modePanel.SetActive(false);
        vsAIPanel.SetActive(false);
        localPlayPanel.SetActive(true);

        if (comingSoonText != null)
            comingSoonText.SetActive(false);
    }

    public void ShowComingSoon()
    {
        if (comingSoonText != null)
            comingSoonText.SetActive(true);
    }
}