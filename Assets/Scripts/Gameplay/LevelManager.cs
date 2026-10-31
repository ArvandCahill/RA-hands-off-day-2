using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    [Header("Player")]
    public PlayerMovement player;
    private int playerHealth => player.GetComponent<Health>().maxHealth;
    private int playerCurrentHealth => player.GetComponent<Health>().currentHealth;

    [Header("UI Elements")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Level & Experience")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentExp;
    [SerializeField] private int baseExpRequired = 10;
    [SerializeField] private float expGrowthMultiplier = 1.2f;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expText;

    [Header("Level Up")]
    [SerializeField] private UnityEvent onLevelUp;

    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;

    public int ExpRequired =>
        Mathf.CeilToInt(
            baseExpRequired * Mathf.Pow(
                expGrowthMultiplier,
                currentLevel - 1
            )
        );

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        Debug.Log($"Player: {player}");
        Debug.Log($"Health Component: {player.GetComponent<Health>()}");
        Debug.Log($"Max Health: {playerHealth}");
        Debug.Log($"Current Health: {playerCurrentHealth}");

        UpdateExpUI();
    }


    public void AddExp(int amount)
    {
        if (amount <= 0)
            return;

        currentExp += amount;

        while (currentExp >= ExpRequired)
        {
            currentExp -= ExpRequired;
            currentLevel++;

            onLevelUp?.Invoke();
        }

        UpdateExpUI();
    }

    private void UpdateExpUI()
    {
        if (expSlider != null)
        {
            expSlider.maxValue = ExpRequired;
            expSlider.value = currentExp;
        }

        if (levelText != null)
            levelText.text = $"Level {currentLevel}";

        if (expText != null)
            expText.text = $"{currentExp} / {ExpRequired}";
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth  ;
        }
        if (healthText != null)
            healthText.text = $"{currentHealth}/{maxHealth}";

        Debug.Log($"Health UI Updated: {currentHealth}/{maxHealth}");
    }
}