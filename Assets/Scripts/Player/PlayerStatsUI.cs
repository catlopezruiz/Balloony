using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    public TextMeshProUGUI statsText;

    public int speed = 1;
    public int balloons = 1;
    public int range = 1;

    public int maxDisplay = 10;

    void Start()
    {
        UpdateUI();
    }

    public void SetStats(int newSpeed, int newBalloons, int newRange)
    {
        speed = newSpeed;
        balloons = newBalloons;
        range = newRange;
        UpdateUI();
    }

    public void UpdateUI()
    {
        statsText.text =
            "SPEED     " + MakeBar(speed) + "\n" +
            "BALLOONS  " + MakeBar(balloons) + "\n" +
            "RANGE     " + MakeBar(range);
    }

    private string MakeBar(int value)
    {
        string bar = "";

        for (int i = 0; i < maxDisplay; i++)
        {
            if (i < value)
                bar += "■";
            else
                bar += "□";
        }

        return bar;
    }
}