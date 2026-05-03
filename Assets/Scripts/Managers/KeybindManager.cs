using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetKey(int playerNumber, string action, KeyCode newKey)
    {
        string keyName = GetKeyName(playerNumber, action);
        PlayerPrefs.SetString(keyName, newKey.ToString());
        PlayerPrefs.Save();
    }

    public KeyCode GetKey(int playerNumber, string action)
    {
        string keyName = GetKeyName(playerNumber, action);

        if (!PlayerPrefs.HasKey(keyName))
            return KeyCode.None;

        string savedKey = PlayerPrefs.GetString(keyName);

        if (System.Enum.TryParse(savedKey, out KeyCode key))
            return key;

        return KeyCode.None;
    }

    private string GetKeyName(int playerNumber, string action)
    {
        return "P" + playerNumber + "_" + action;
    }

    public void ClearKeybinds()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}