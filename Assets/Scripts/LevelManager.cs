
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int killCount = 0;
    public TextMeshProUGUI killText;

    public int coinCount = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateKillText();
    }

    public void AddKill()
    {
        killCount++;
        UpdateKillText();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        Debug.Log("Coin count: " + coinCount);
    }

    private void UpdateKillText()
    {
        if (killText != null)
        {
            killText.text = "Kills: " + killCount;
        }
    }
}
