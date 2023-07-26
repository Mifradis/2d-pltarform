using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Weapon/Ranged Weapon")]
public class RangedWeapon : Weapon
{
    [Header("Reloading")]
    public int currentAmmmo;
    public int magSize;
    public float reloadTime;
    [HideInInspector]
    public bool reloading = false;
}
