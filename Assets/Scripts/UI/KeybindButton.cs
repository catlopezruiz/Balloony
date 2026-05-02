using UnityEngine;
using TMPro;

public class KeybindButton : MonoBehaviour
{
    public int playerNumber;
    public string action;

    public TextMeshProUGUI buttonText;

    private bool waitingForKey = false;

    void Start()
    {
        UpdateButtonText();
    }

    public void StartRebind()
    {
        waitingForKey = true;

        if (buttonText != null)
        {
            buttonText.text = "Press key...";
        }
    }

    void Update()
    {
        if (!waitingForKey) return;

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                KeybindManager.Instance.SetKey(playerNumber, action, key);

                waitingForKey = false;
                UpdateButtonText();

                break;
            }
        }
    }

    public void UpdateButtonText()
    {
        if (buttonText == null) return;
        if (KeybindManager.Instance == null) return;

        KeyCode currentKey = KeybindManager.Instance.GetKey(playerNumber, action);
        buttonText.text = currentKey.ToString();
    }
}