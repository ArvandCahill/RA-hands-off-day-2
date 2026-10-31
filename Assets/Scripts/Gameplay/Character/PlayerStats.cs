using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance { get; private set; }

    [Header("Multipliers")]
    [SerializeField] private float damageMultiplier = 1f;
    [SerializeField] private float defenseMultiplier = 1f;
    [SerializeField] private float fireRateMultiplier = 1f;
    [SerializeField] private float movementSpeedMultiplier = 1f;
    [SerializeField] private float reloadTimeMultiplier = 1f;
    [SerializeField, Range(0f, 0.8f)] private float damageReduction;

    public float DamageMultiplier => damageMultiplier;

    public float DefenseMultiplier => defenseMultiplier;
    public float FireRateMultiplier => fireRateMultiplier;
    public float MovementSpeedMultiplier => movementSpeedMultiplier;
    public float ReloadTimeMultiplier => reloadTimeMultiplier;
    public float DamageReduction => damageReduction;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void AddDamage(float amount)
    {
        damageMultiplier += amount;
    }

    public void AddDefense(float amount)
    {
        damageReduction = Mathf.Clamp(
            damageReduction + amount,
            0f,
            0.8f
        );
    }

    public void AddFireRate(float amount)
    {
        fireRateMultiplier += amount;
    }

    public void AddMovementSpeed(float amount)
    {
        movementSpeedMultiplier += amount;
    }

    public void AddReloadSpeed(float amount)
    {
        reloadTimeMultiplier = Mathf.Max(
            0.1f, reloadTimeMultiplier - amount
        );
    }

}