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
    [HideInInspector]
    public Vector2 shootingInput;

    KeyCode jumpKey = KeyCode.Space;
    KeyCode dashKey = KeyCode.LeftShift;
    KeyCode shootKey = KeyCode.J;
    KeyCode reloadKey = KeyCode.R;
    KeyCode mainMenuKey = KeyCode.H;

    public event Action onJump = delegate { };
    public event Action onMainMenu = delegate { };
    public event Action onShoot = delegate { };
    public event Action onJumpRelease = delegate { };
    public event Action onDash = delegate { };
    public event Action onReload = delegate { };
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        GetJumpInput();
        GetDashInput();
        GetShootInput();
        GetReloadInput();
        GetMainMenuInput();
    }
    private void FixedUpdate()
    {
        
    }
    void GetMovementInput()
    {
        horizontalInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        //verticalInput = new Vector2(0, Input.GetAxisRaw("Vertical"));
    }
    void GetJumpInput()
    {
        if (Input.GetKeyDown(jumpKey)){
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
    void GetShootInput()
    {
        if (Input.GetKeyDown(shootKey))
        {
            onShoot();
        }
    }
    void GetReloadInput()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            onReload();
        }
    }
    void GetMainMenuInput()
    {
        if (Input.GetKeyDown(mainMenuKey))
        {
            onMainMenu();
        }
    }
}
