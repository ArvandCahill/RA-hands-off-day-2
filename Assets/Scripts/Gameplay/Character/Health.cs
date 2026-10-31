
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth { get; private set; }

    [Header("Damage Flash")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.1f;

    private Color originalColor;
    private Coroutine flashCoroutine;
    private PlayerStats playerStats;

    private void Awake()
    {
        currentHealth = maxHealth;
        playerStats = GetComponent<PlayerStats>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    private void Start()
    {
        /*LevelManager.instance.UpdateHealthUI(currentHealth, maxHealth);*/
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || currentHealth <= 0)
            return;

        if (playerStats != null)
        {
            damage = Mathf.CeilToInt(
                damage * (1f - playerStats.DamageReduction)
            );
        }

        damage = Mathf.Max(1, damage);
        currentHealth = Mathf.Max(0, currentHealth - damage);

        PlayDamageFlash();

        if (CompareTag("Player"))
            LevelManager.instance.UpdateHealthUI(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || currentHealth <= 0)
            return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        if (CompareTag("Player"))
            LevelManager.instance.UpdateHealthUI(currentHealth, maxHealth);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void PlayDamageFlash()
    {
        if (spriteRenderer == null)
            return;

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(DamageFlashRoutine());
    }

    private IEnumerator DamageFlashRoutine()
    {
        spriteRenderer.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }
}
