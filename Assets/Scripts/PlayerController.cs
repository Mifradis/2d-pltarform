using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    float speed = 8;
    Vector2 input;
    Vector2 velocity;
    float jumpForce = 100f;
    bool grounded;
    bool jumpCancelled;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        velocity = input * speed;
        transform.Translate(velocity * Time.deltaTime);
    }
    void Jump()
    {
        if (grounded)
        {
            jumpCancelled = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
