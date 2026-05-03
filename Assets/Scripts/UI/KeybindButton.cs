using UnityEngine;
using TMPro;

public class KeybindButton : MonoBehaviour
{
    public int playerNumber;
    public string action;
    public TextMeshProUGUI buttonText;

    [Header("Optional Fixed Key")]
    public KeyCode fixedKey = KeyCode.None; // ← set this in Inspector

    private bool waitingForKey = false;

    void Start()
    {
        // If a fixed key is set, assign it immediately
        if (fixedKey != KeyCode.None && KeybindManager.Instance != null)
        {
            KeybindManager.Instance.SetKey(playerNumber, action, fixedKey);
        }

        UpdateButtonText();
    }

    // Called when button is clicked
    public void StartRebind()
    {
        if (KeybindManager.Instance == null)
        {
            Debug.LogError("No KeybindManager found.");
            return;
        }

        // If using fixed key → assign instantly (NO rebinding)
        if (fixedKey != KeyCode.None)
        {
            KeybindManager.Instance.SetKey(playerNumber, action, fixedKey);
            UpdateButtonText();
            return;
        }

        // Otherwise allow manual rebind
        waitingForKey = true;

        if (buttonText != null)
            buttonText.text = "Press key...";
    }

    void Update()
    {
        if (!waitingForKey) return;
        if (KeybindManager.Instance == null) return;

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

        if (currentKey == KeyCode.None)
            buttonText.text = "-";
        else
            buttonText.text = currentKey.ToString();
    }
}