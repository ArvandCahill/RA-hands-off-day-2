using UnityEngine;

public enum PowerUpType
{
    Stat,
    Weapon
}

public abstract class PowerUpData : ScriptableObject
{
    [Header("Card Information")]
    public Sprite cardImage;
    public string cardTitle;

    [TextArea] public string cardDescriptionId;
    [TextArea] public string cardDescriptionEn;

    public PowerUpType powerUpType;

    public abstract void ApplyEffect(PlayerStats playerStats);

    public virtual bool CanAppear()
    {
        return true;
    }
}