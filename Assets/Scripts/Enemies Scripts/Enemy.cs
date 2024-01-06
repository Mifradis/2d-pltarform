using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ScriptableObject
{
    [Header("Info")]
    [SerializeField] string enemyName;
    [SerializeField] int enemyTier;

    [Header("Features")]
    [SerializeField] float hp;
    [SerializeField] float damage;
    [SerializeField] float fireRate;
    [SerializeField] float viewDistance;
}
