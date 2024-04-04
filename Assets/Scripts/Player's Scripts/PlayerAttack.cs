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
    int attackTurn = 0;
    string CurrentClipName = " ";
    void Start()
    {
        attackInp = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        attackInp.onShoot += Attack;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentClipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.ToString();

        if(Time.time > nextAttackTime)
        {
            animator.SetBool("Attacking", false);
        }
        animator.SetInteger("attackTurn", attackTurn);
        if(attackTurn > 4)
        {
            attackTurn = 0;
        }
        
        
        if(!(firstAttackTime == 0)&&(Time.time - firstAttackTime >= 1.5))
        {
            firstAttackTime = 0;
            attackTurn = 0;
            //animator.SetFloat("Combo", 2);
        }
    }
    void Attack()
    {
        
        if (CanAttack())
        {
            animator.SetBool("Attacking", true);
            firstAttackTime = Time.time;
            nextAttackTime = Time.time + animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            if (attackTurn >= 4)
            {
                attackTurn = 0;
            }
            else
            {
                attackTurn++;
            }
        }
    }
    bool CanAttack() {
        if(Time.time > nextAttackTime)
        {
            animator.SetBool("Attacking", false);
            nextAttackTime = Time.time + attackTime;
            return true;
        }
        else
        {
            return false;
        }
    }
}
