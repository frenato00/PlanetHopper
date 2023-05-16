using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reloading")]
    public float fireRate;
}
