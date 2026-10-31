using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Rapid Fire")]
public class RapidFireData : PowerUpData
{
    [SerializeField] private float bonus = 0.15f;

    public override void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.AddFireRate(bonus);
    }
}