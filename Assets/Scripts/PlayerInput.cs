using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector]
    public Vector2 horizontalInput;
    [HideInInspector]
    public Vector2 verticalInput;
    [HideInInspector]
    public Vector2 jumpInput;
    [HideInInspector]
    public Vector2 dashInput;

    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode dashKey = KeyCode.LeftShift;

    public event Action onJump = delegate { };
    public event Action onJumpRelease = delegate { };
    public event Action onDash = delegate { };
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        GetMovementInput();
        GetJumpInput();
        GetDashInput();
    }
    void GetMovementInput()
    {
        horizontalInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        //verticalInput = new Vector2(0, Input.GetAxisRaw("Vertical"));
    }
    void GetJumpInput()
    {
        if (Input.GetKey(jumpKey)){
            onJump();
        }
        if (!Input.GetKey(jumpKey))
        {
            onJumpRelease();
        }
    }
    void GetDashInput()
    {
        if (Input.GetKeyDown(dashKey))
        {
            onDash();
        }
    }
}
