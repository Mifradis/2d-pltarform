using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject
{
    [Header("Info")]
    [SerializeField] string weaponName;

    [Header("Shooting")]
    public float damage;
    public float fireRate;
    public float maxDÝstance;
}
