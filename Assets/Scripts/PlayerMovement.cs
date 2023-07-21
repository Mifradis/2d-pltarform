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
    bool grounded;
    bool jumpCancelled;

    [Header("Dash")]
    [SerializeField] float dashingPower = 300f;
    bool isDashing = false;
    bool canDash = true;
    PlayerInput playerInput;
    Rigidbody2D rb;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if(playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing");
        }
    }
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Move();
    }
    void Jump()
    {
        if (grounded)
        {
            jumpCancelled = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    void Move()
    {
        velocity = playerInput.horizontalInput * speed;
        transform.Translate(velocity * Time.deltaTime);
    }
}
