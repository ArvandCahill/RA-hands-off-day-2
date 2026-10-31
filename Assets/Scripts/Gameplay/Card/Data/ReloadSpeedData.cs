using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Reload Speed")]
public class ReloadSpeedData : PowerUpData
{
    [SerializeField] private float reduction = 0.15f;

    public override void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.AddReloadSpeed(reduction);
    }
}