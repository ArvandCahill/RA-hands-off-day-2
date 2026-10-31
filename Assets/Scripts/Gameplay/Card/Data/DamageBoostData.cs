using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Damage Boost")]
public class DamageBoostData : PowerUpData
{
    [SerializeField] private float bonus = 0.15f;

    public override void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.AddDamage(bonus);
    }
}