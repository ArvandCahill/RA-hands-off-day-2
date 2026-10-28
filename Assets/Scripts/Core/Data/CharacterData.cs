using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "Data/Character")]
public class CharacterData : ScriptableObject
{
    [Header("Character Properties")]
    public int _id;
    public string _characterName;
    public Sprite _characterSprite;

    [Header("Character Stats")]
    public int _health;

    [Header("Shop Properties")]
    public int _price;
    public bool _isUnlocked;

    public void CheckMilestone(int currency)
    {
        if (currency >= _price)
        {
            _isUnlocked = true;
        }
    }
}
