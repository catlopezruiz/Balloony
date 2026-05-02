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
        SetDefaultKeysIfNeeded();
    }

    private void SetDefaultKeysIfNeeded()
    {
        SetDefaultKey(1, "Up", KeyCode.W);
        SetDefaultKey(1, "Down", KeyCode.S);
        SetDefaultKey(1, "Left", KeyCode.A);
        SetDefaultKey(1, "Right", KeyCode.D);
        SetDefaultKey(1, "Bomb", KeyCode.Space);

        SetDefaultKey(2, "Up", KeyCode.UpArrow);
        SetDefaultKey(2, "Down", KeyCode.DownArrow);
        SetDefaultKey(2, "Left", KeyCode.LeftArrow);
        SetDefaultKey(2, "Right", KeyCode.RightArrow);
        SetDefaultKey(2, "Bomb", KeyCode.RightShift);

        SetDefaultKey(3, "Up", KeyCode.I);
        SetDefaultKey(3, "Down", KeyCode.K);
        SetDefaultKey(3, "Left", KeyCode.J);
        SetDefaultKey(3, "Right", KeyCode.L);
        SetDefaultKey(3, "Bomb", KeyCode.U);

        SetDefaultKey(4, "Up", KeyCode.T);
        SetDefaultKey(4, "Down", KeyCode.G);
        SetDefaultKey(4, "Left", KeyCode.F);
        SetDefaultKey(4, "Right", KeyCode.H);
        SetDefaultKey(4, "Bomb", KeyCode.Y);
    }

    private void SetDefaultKey(int playerNumber, string action, KeyCode defaultKey)
    {
        string keyName = GetKeyName(playerNumber, action);

        if (!PlayerPrefs.HasKey(keyName))
        {
            PlayerPrefs.SetString(keyName, defaultKey.ToString());
        }
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
        string savedKey = PlayerPrefs.GetString(keyName);

        return (KeyCode)System.Enum.Parse(typeof(KeyCode), savedKey);
    }

    private string GetKeyName(int playerNumber, string action)
    {
        return "P" + playerNumber + "_" + action;
    }

    public void ResetKeybinds()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SetDefaultKeysIfNeeded();
    }
}