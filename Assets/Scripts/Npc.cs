using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottedPlayer;
    public float speed = 8;
    public float waitTime = .2f;
    public float timeToSpotPlayer = .5f;
    public Light spotlight;
    public LayerMask viewMask;
    public float viewDistance;
    float viewAngle;
    Color originalSpotlightColor;
    Transform player;
    void Start()
    {
        viewAngle = spotlight.spotAngle;
        originalSpotlightColor = spotlight.color;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        
    }
    bool CanSeePlayer()
    {
        if(Vector2.Distance(transform.position, player.position)< viewDistance)
        {
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector2.Angle(transform.forward, dirToPlayer);
            if(angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if(!Physics2D.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
