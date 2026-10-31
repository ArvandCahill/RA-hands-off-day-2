using UnityEngine;

[CreateAssetMenu(fileName = "WeaponPowerUp", menuName = "Power Up/Weapon")]
public class WeaponPowerUpData : PowerUpData
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject weaponPrefab;

    private bool hasBeenSelected;

    private void OnEnable()
    {
        powerUpType = PowerUpType.Weapon;
    }

    public override void ApplyEffect(PlayerStats playerStats)
    {
        if (hasBeenSelected || weaponPrefab == null)
            return;

        Gun currentGun = playerStats.GetComponentInChildren<Gun>();

        if (currentGun == null)
            return;

        Transform weaponParent = currentGun.transform.parent;

        Destroy(currentGun.gameObject);

        Instantiate(
            weaponPrefab,
            weaponParent
        );

        hasBeenSelected = true;
    }
}