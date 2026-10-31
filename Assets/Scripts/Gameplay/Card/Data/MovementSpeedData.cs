using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Movement Speed")]
public class MovementSpeedData : PowerUpData
{
    [SerializeField] private float bonus = 0.1f;

    public override void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.AddMovementSpeed(bonus);
    }
}