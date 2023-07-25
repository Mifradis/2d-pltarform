using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Weapon/Ranged Weapon")]
public class RangedWeapon : Weapon
{
    [Header("Reloading")]
    [SerializeField] int currentAmmmo;
    [SerializeField] int magSize;
    [SerializeField] float reloadTime;
    bool reloading;
    
}
