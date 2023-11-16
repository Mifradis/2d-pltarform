using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    bool isHitting = false;
    float attackTime = 0.4f;
    float nextAttackTime;
    public PlayerInput attackInp;
    public Animator animator;
    float firstAttackTime = 0;
    float secondAttackTime = 0;
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
        
        if (!(firstAttackTime == 0) && !(secondAttackTime == 0))
        {
            animator.SetFloat("Combo", secondAttackTime - firstAttackTime);
            firstAttackTime = Time.time;
            secondAttackTime = 0;
        }
        if(!(firstAttackTime == 0)&&(Time.time - firstAttackTime >= 1))
        {
            firstAttackTime = 0;
            animator.SetFloat("Combo", 2);
        }
    }
    void Attack()
    {
        
        if (CanAttack())
        {
            if ((firstAttackTime == 0))
            {
                firstAttackTime = Time.time;
            }
            else if(!(firstAttackTime == 0))
            {
                secondAttackTime = Time.time;
            }
            isHitting = true;
            //animator.SetBool("isSecond", true);
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
