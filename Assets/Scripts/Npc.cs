using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottedPlayer;
    public float speed = 8;
    public float timeToSpotPlayer = .5f;
    public Light spotlight;
    public LayerMask viewMask;
    public float viewDistance;
    float viewAngle;
    Color originalSpotlightColor;
    public Transform player;
    void Start()
    {
        viewAngle = spotlight.spotAngle;
        originalSpotlightColor = spotlight.color;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalSpotlightColor = spotlight.color;
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            spotlight.color = Color.red;
        }
        else
        {
            spotlight.color = originalSpotlightColor;
        }
        print(CanSeePlayer());
    }
    bool CanSeePlayer()
    {
        if(Vector2.Distance(transform.position, player.position) < viewDistance)
        {
            Vector2 dirToPlayer = (player.position - transform.position);
            float angleBetweenGuardAndPlayer = Vector2.Angle(transform.right, dirToPlayer);
            if(angleBetweenGuardAndPlayer < viewAngle)
            {
                if(Physics2D.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
