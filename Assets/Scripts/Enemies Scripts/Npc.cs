using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Enemy enemyData;

    public static event System.Action OnGuardHasSpottedPlayer;
    public PlayerMovement playersDamge;
    public Light spotlight;
    [SerializeField] float jumpForce;
    public LayerMask viewMask;
    public GameObject hitbox;
    float takingHitTime = 0;
    Color originalSpotlightColor;
    float nextAttackTime;
    public Transform player;
    public Animator animator;
    public GameObject pointA;
    public GameObject pointB;
    [SerializeField]Rigidbody2D rb;
    Transform currentPoint;
    public float stopMovingWhenTakingHit = 0;
    float speedBeforeTakeHit;
    public bool isFacingRight = true;
    float destroyingTime = 0;
    public GameObject hpBar;
    float staticScaleX;
    Collider2D enemyCollider;
    float angleBetweenGuardAndPlayer;
    [SerializeField]GameObject Player;
    [SerializeField] LayerMask groundMask;
    bool isPlayerSpotted;
    bool flip;
    void Start()
    {
        isPlayerSpotted = false;
        currentPoint = pointB.transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<GameObject>();
        enemyData.viewAngle = spotlight.spotAngle;
        originalSpotlightColor = spotlight.color;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalSpotlightColor = spotlight.color;
        enemyData.hp = enemyData.maxHp;
        staticScaleX = hpBar.transform.localScale.x;
        enemyCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if(destroyingTime != 0)
        {
            if(Time.time >= destroyingTime + 3)
            {       
                Destroy(gameObject);
            }
        }
        animator.SetInteger("Speed", (int) enemyData.speed);
        animator.SetInteger("Y-axis", (int) rb.velocity.y);
        if (CanSeePlayer())
        {
            spotlight.color = Color.red;
        }
        else
        {
            spotlight.color = originalSpotlightColor;
        }
        print(isPlayerSpotted);
        if(Time.time - takingHitTime >= 0.4f)
        {
            animator.SetBool("TakingHit", false);
        }
        animator.SetBool("CanMove", CanMove());
    }
    private void FixedUpdate()
    {
        Movement();
        Attack();
        Jump();
    }
    void Movement()
    {
        if (CanMove())
        {
            Vector2 localScale = transform.localScale;
            if (isFacingRight && enemyData.velocity.x < 0f || !isFacingRight && enemyData.velocity.x > 0f)
            {
                isFacingRight = !isFacingRight;
                localScale *= new Vector2(-1, 1);
                transform.localScale = localScale;
            }
            if (!isPlayerSpotted)
            {         
                if (currentPoint == pointB.transform)
                {
                    rb.velocity = new Vector2(Vector2.right.x * enemyData.speed, rb.velocity.y);
                    enemyData.velocity = rb.velocity;
                }
                else
                {
                    rb.velocity = new Vector2(Vector2.left.x * enemyData.speed, rb.velocity.y);
                    enemyData.velocity = rb.velocity;
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
            else if(player.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(Vector2.right.x * enemyData.speed, rb.velocity.y);
                enemyData.velocity = rb.velocity;
            }
            else if(player.transform.position.x < transform.position.x)
            {
                print(-enemyData.speed);
                rb.velocity = new Vector2(Vector2.left.x * enemyData.speed, rb.velocity.y);
                enemyData.velocity = rb.velocity;
            }
        }
    }
    bool CanSeePlayer()
    {

            if(Vector2.Distance(transform.position, player.position) <= enemyData.viewDistance)
            {
                
            Vector2 dirToPlayer = (player.position - transform.position);
            if (isFacingRight)
            {
                angleBetweenGuardAndPlayer = Vector2.Angle(transform.right, dirToPlayer);
            }if(!isFacingRight)
            {
                angleBetweenGuardAndPlayer = Vector2.Angle(-transform.right, dirToPlayer);
            }
                if(angleBetweenGuardAndPlayer < enemyData.viewAngle)
                {
                    if(Physics2D.Raycast(transform.position, player.transform.position, viewMask))
                    {
                        isPlayerSpotted = true;
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
            if (Mathf.Abs(player.position.x - transform.position.x) <= 1 && Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + enemyData.fireRate;
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
        if (!animator.GetBool("TakingHit"))
        {
            if (collision.tag == "Sword")
            {
                animator.SetBool("TakingHit", true);
                takingHitTime = Time.time;
                takingDamage();
            }
        }
    }
    bool CanMove()
    {
        if (animator.GetBool("TakingHit") || animator.GetBool("isAttacking") || Mathf.Abs(player.position.x - transform.position.x) <= 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    void takingDamage()
    {
        enemyData.hp -= playersDamge.damage;
        float scaleX = hpBar.transform.localScale.x;
        hpBar.gameObject.transform.localScale = new Vector2((scaleX - (staticScaleX * (playersDamge.damage / enemyData.maxHp))), hpBar.transform.localScale.y);
        float positionX = ((staticScaleX * (playersDamge.damage / enemyData.maxHp))) / 2;
        hpBar.transform.localPosition = new Vector2(hpBar.transform.localPosition.x - positionX, hpBar.transform.localPosition.y);
        if (enemyData.hp <= 0)
        {   
            destroyingTime = Time.time;
            animator.SetTrigger("Death");
            rb.bodyType = RigidbodyType2D.Static;
            Destroy(enemyCollider);
        }
    }
    void Jump()
    {
        RaycastHit2D jumpController;
        if (isFacingRight)
        {
            jumpController = Physics2D.Raycast(transform.position + Vector3.right, transform.right*0.2f, 5f);
            Debug.DrawRay(transform.position + Vector3.right, transform.right * 0.2f, Color.green);
        }
        else
        {
            jumpController = Physics2D.Raycast(transform.position + Vector3.left, -transform.right*0.2f, 0.2f);
            Debug.DrawRay(transform.position + Vector3.left, -transform.right * 0.2f, Color.green);
        }

        if (rb.velocity.y == 0)
        {
            if (jumpController.collider != null)
            {
                print(jumpController.collider.tag);
                if (jumpController.collider.tag == "Ground")
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}
