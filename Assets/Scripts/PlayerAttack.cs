using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    bool isHitting = false;
    float attackTime = 0.5f;
    float nextAttackTime;
    public PlayerInput attackInp;
    public Animator animator;
    void Start()
    {
        attackInp = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        attackInp.onShoot += Attack;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextAttackTime)
        {
            isHitting = false;
        }
        animator.SetBool("Attacking", isHitting);
    }
    void Attack()
    {
        if (CanAttack())
        {
            isHitting = true;
        }
    }
    bool CanAttack() {
        if(Time.time > nextAttackTime)
        {
            isHitting = false;
            nextAttackTime = Time.time + attackTime;
            return true;
        }
        else
        {
            return false;
        }
    }
}
