using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Data/Gun")]
public class GunData : ScriptableObject
{
    [Header("Gun Properties")]
    public int _id;
    public string _gunName;
    public Sprite _gunSprite;
    public Projectile _projectilePrefab;

    [Header("Gun Stats")]
    public int _damage;
    public float _fireRate;
}
