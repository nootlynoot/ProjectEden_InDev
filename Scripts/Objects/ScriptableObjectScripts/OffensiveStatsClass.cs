using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OffensiveAttributes", menuName = "NewWeaponStats")]
public class OffensiveStatsClass : ScriptableObject
{
    public enum OffenceType {Primary, Secondary, Ultimate}
    public OffenceType AttackType = OffenceType.Primary;
    [Space(10)]
    public string WeaponName;
    [Space(10)]
    public Sprite WeaponArtwork;
    [Space(10)]
    public GameObject Projectile;

    public int Cost;

    public float Damage;

    public float Cooldown; 
}
