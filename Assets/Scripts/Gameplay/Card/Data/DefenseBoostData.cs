using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Defense Boost")]
public class DefenseBoostData : PowerUpData
{
    [SerializeField] private float bonus = 0.1f;

    public override void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.AddDefense(bonus);
    }
}