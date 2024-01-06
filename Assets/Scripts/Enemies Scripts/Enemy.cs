using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ScriptableObject
{
    [Header("Info")]
    [SerializeField] string enemyName;
    [SerializeField] int enemyTier;

    [Header("Features")]
    public float maxHp;
    public float hp;
    public float damage;
    public float fireRate;
    public float viewDistance;
    public float speed;
    public Vector2 velocity;
    public float timeToSpotPlayer;
    public float viewAngle;
}
