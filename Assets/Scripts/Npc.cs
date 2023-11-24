using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottedPlayer;
    public float speed = 5;
    Vector2 velocity;
    public float timeToSpotPlayer = .5f;
    public Light spotlight;
    public LayerMask viewMask;
    public float viewDistance;
    public GameObject hitbox;
    float takingHitTime = 0;
    float viewAngle;
    Color originalSpotlightColor;
    public Transform player;
    public Animator animator;
    public GameObject pointA;
    public GameObject pointB;
    [SerializeField]Rigidbody2D rb;
    Transform currentPoint;
    public float stopMovingWhenTakingHit = 0;
    float speedBeforeTakeHit;
    public bool isFacingRight = true;
    void Start()
    {
        currentPoint = pointB.transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<GameObject>();
        viewAngle = spotlight.spotAngle;
        originalSpotlightColor = spotlight.color;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalSpotlightColor = spotlight.color;
    }

    void Update()
    {
        if (animator.GetBool("TakingHit")||animator.GetBool("isAttacking"))
        {
            speedBeforeTakeHit = rb.velocity.x;
            speed = 0;
        }
        animator.SetInteger("Speed", (int)speed);
        if (CanSeePlayer())
        {
            spotlight.color = Color.red;
        }
        else
        {
            spotlight.color = originalSpotlightColor;
        }
        print(CanSeePlayer());
        if(Time.time - takingHitTime >= 0.4)
        {
            animator.SetBool("TakingHit", false);
        }
    }
    private void FixedUpdate()
    {
        Movement();
        Attack();
    }
    void Movement()
    {
        Vector2 localScale = transform.localScale;
        if (isFacingRight && velocity.x < 0f || !isFacingRight && velocity.x > 0f)
        {
            isFacingRight = !isFacingRight;
            localScale *= new Vector2(-1, 1);
            transform.localScale = localScale;
        }
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
            velocity = rb.velocity;
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
            velocity = rb.velocity;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
        }
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
    void Attack()
    {
        if (!animator.GetBool("TakingHit"))
        {
            if (Mathf.Abs(player.position.x - transform.position.x) <= 1)
            {
                animator.SetBool("isAttacking", true);
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Sword")
        {
            animator.SetBool("TakingHit", true);
            takingHitTime = Time.time;
        }
    }
}
