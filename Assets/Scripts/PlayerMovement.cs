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
    bool isDashing;
    bool canDash = true;
    float gravity = 0;

    [Header("CheckGround")]
    [SerializeField] float playerHeight;
    [SerializeField] float checkOffset = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform feet;

    PlayerInput playerInput;
    Rigidbody2D rb;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
  
    }
    void Start()
    {    
        rb = GetComponent<Rigidbody2D>();
        playerInput.onJump += Jump;
        playerInput.onDash += Dash;
        playerInput.onJumpRelease += CutJump;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        setDashDirection();
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
        if (grounded)
        {
            jumpCancelled = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    void CutJump()
    {
        if(!grounded && !jumpCancelled)
        {
            jumpCancelled = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.3f);
        }
    }
    void CheckGrounded()
    {
        grounded = Physics2D.Raycast(transform.position, Vector2.down, playerHeight * 0.5f + checkOffset, groundLayer);
    }
    bool GetGrounded()
    {
        return grounded;
    }
    void Move()
    {
        velocity = playerInput.horizontalInput * speed;
        transform.Translate(velocity * Time.deltaTime);
    }
    private IEnumerator DashEnum()
    {
        canDash = false;
        isDashing = true;
        gravity = rb.gravityScale;
        rb.gravityScale = 0f;
        //rb.AddForce(dashDirection * dashingPower * Time.deltaTime, ForceMode2D.Impulse);
        rb.velocity = new Vector2(dashDirection.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = gravity;
        gravity = 0f;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    void Dash()
    {
        if (canDash)
        {
            StartCoroutine(DashEnum());
        }
    }
    void setDashDirection()
    {
        dashDirection = velocity == Vector2.zero ? transform.right : velocity;
    }
}
