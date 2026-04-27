using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject modePanel;

    public void ShowModePanel()
    {
        titlePanel.SetActive(false);
        modePanel.SetActive(true);
    }

    public void ShowTitlePanel()
    {
        titlePanel.SetActive(true);
        modePanel.SetActive(false);
    }
}