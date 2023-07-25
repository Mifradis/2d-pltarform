using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject
{
    [Header("Info")]
    [SerializeField] string weaponName;

    [Header("Shooting")]
    [SerializeField] float damage;
    [SerializeField] float fireRate;
    [SerializeField] float maxDÝstance;
}
