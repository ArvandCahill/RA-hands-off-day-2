
using UnityEngine;

[CreateAssetMenu(fileName = "Healing", menuName = "Power Up/Healing")]
public class HealingData : PowerUpData
{
    [SerializeField] private int healAmount = 1;

    public override void ApplyEffect(PlayerStats playerStats)
    {
        Health health = playerStats.GetComponent<Health>();

        if (health != null)
            health.Heal(healAmount);
    }
}
