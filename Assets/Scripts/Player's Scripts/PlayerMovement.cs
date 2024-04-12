using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 8;
    [SerializeField] Vector2 velocity;
    bool canMove = true;

    [Header("Jump")]
    [SerializeField] float jumpForce = 100f;
    [SerializeField] float gravityMultiplier = 40f;
    bool grounded;
    bool jumpCancelled;

    [Header("Dash")]
    [SerializeField] float dashingPower = 300f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    Vector2 dashDirection;
    public bool isDashing;
    public bool canDash = true;
    Vector2 realVelocity;
    float gravity = 0;

    [Header("CheckGround")]
    [SerializeField] float playerHeight;
    [SerializeField] float checkOffset = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform feet;

    [SerializeField] LayerMask enemyMask;
    [SerializeField] LayerMask playerMask;
    PlayerInput playerInput;
    Rigidbody2D rb;
    public float damage;
    [SerializeField] public float healthPoint;
    [SerializeField] public float maxHp;
    float takingHitTime = 0;
    public Animator animations;

    [SerializeField] float enemyDamage;
    [SerializeField] public GameObject hpBar;
    public float staticScaleX;
    public bool isDead;
    string CurrentAnimationName = " ";
    AudioManager audioManager;
    [SerializeField] GameObject mainMenu;


    public bool isFacingRight = true;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }
    void Start()
    {
        isDead = false;
        healthPoint = maxHp;
        rb = GetComponent<Rigidbody2D>();
        animations = GetComponent<Animator>();
        playerInput.onJump += Jump;
        playerInput.onDash += Dash;
        playerInput.onJumpRelease += CutJump;
        playerInput.onMainMenu += MainMenu;
        staticScaleX = hpBar.transform.localScale.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        CurrentAnimationName = animations.GetCurrentAnimatorClipInfo(0)[0].clip.ToString();
        setDashDirection();
        animations.SetInteger("Speed", (int)velocity.x);
        if(animations.GetInteger("Speed") != 0)
        {
            //audioManager.PlaySFX(audioManager.run);
        }
        animations.SetInteger("Y velocity", (int)rb.velocity.y);
        if (Time.time - takingHitTime >= 0.4)
        {
            animations.SetBool("TakingHit", false);
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        Move();
        CheckGrounded();
        IncreaseGravity();
    }
    void IncreaseGravity()
    {
        if (rb.velocity.y < 1)
        {
            rb.AddForce(Vector2.down * gravityMultiplier * Time.deltaTime, ForceMode2D.Force);
        }
    }
    void Jump()
    {
        if (rb.velocity.y == 0&&!isDead)
        {
            audioManager.PlaySFX(audioManager.jump);
            jumpCancelled = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
        }
    }
    void CutJump()
    {
        if (rb.velocity.y != 0 && !jumpCancelled || isDead)
        {
            jumpCancelled = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.3f);
        }
    }
    void CheckGrounded()
    {
        grounded = Physics2D.Raycast(transform.position, Vector2.down, playerHeight * 0.5f + checkOffset, groundLayer);
        animations.SetBool("Grounded", grounded);
    }
    bool GetGrounded()
    {
        return grounded;
    }
    void Move()
    {
        Vector2 localScale = transform.localScale;
        if (CanMove())
        {
            velocity = playerInput.horizontalInput * speed;
            transform.Translate(velocity * Time.deltaTime);
            
            if (isFacingRight && velocity.x < 0f || !isFacingRight && velocity.x > 0f)
            {
                isFacingRight = !isFacingRight;
                localScale *= new Vector2(-1, 1);
                transform.localScale = localScale;
            }
        }
    }
    private IEnumerator DashEnum()
    {
        realVelocity = rb.velocity;
        canDash = false;
        isDashing = true;
        animations.SetBool("isDashing", true);
        gravity = rb.gravityScale;
        rb.gravityScale = 0f;
        //rb.AddForce(dashDirection * dashingPower * Time.deltaTime, ForceMode2D.Impulse);
        rb.velocity = new Vector2(dashDirection.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = gravity;
        gravity = 0f;
        isDashing = false;
        animations.SetBool("isDashing", false);
        rb.velocity = realVelocity;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    void Dash()
    {
        if (canDash && rb.velocity.y == 0 && velocity.x != 0)
        {
            StartCoroutine(DashEnum());
        }
    }
    void setDashDirection()
    {
        dashDirection = velocity == Vector2.zero ? transform.right : velocity;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemySword")
        {
            animations.SetBool("TakingHit", true);
            takingHitTime = Time.time;
            enemyDamage = collision.GetComponentInParent<Npc>().enemyData.damage;
            takingDamage();
        }
    }
    bool CanMove()
    {
        if (animations.GetBool("TakingHit") || CurrentAnimationName.Contains("Attack") || isDashing ||isDead)
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
        if (healthPoint - enemyDamage <= 0)
        {
            healthPoint = 0;
            hpBar.SetActive(false);
        }
        else
        {
            healthPoint -= enemyDamage;
        }
        if (healthPoint <= 0)
        {
            healthPoint = 0;
            //isDead = true;
            //destroyingTime = Time.time;
            isDead = true;
            animations.SetTrigger("Death");
            //Destroy(enemyCollider);
        }
        
        float scaleX = hpBar.transform.localScale.x;
        hpBar.gameObject.transform.localScale = new Vector2((scaleX - (staticScaleX * (enemyDamage / maxHp))), hpBar.transform.localScale.y);
        float positionX = ((staticScaleX * (enemyDamage / maxHp))) / 2;
        hpBar.transform.localPosition = new Vector2(hpBar.transform.localPosition.x - positionX, hpBar.transform.localPosition.y);
        audioManager.PlaySFX(audioManager.takeHit);
    }
    void MainMenu()
    {
        if (!mainMenu.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            mainMenu.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            Time.timeScale = 1;
            mainMenu.SetActive(false);
        }
    }
}

