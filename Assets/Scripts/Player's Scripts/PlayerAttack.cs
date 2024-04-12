using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    bool isHitting = false;
    float attackTime = 0.6f;
    float nextAttackTime;
    PlayerMovement Movement;
    public PlayerInput attackInp;
    public Animator animator;
    float firstAttackTime = 0;
    float secondAttackTime = 0;
    int attackTurn = 0;
    string CurrentClipName = " ";
    AudioManager audioManager;
    private void Awake()
    {
        Movement = GetComponentInParent<PlayerMovement>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
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
            isHitting = false;
            Movement.canDash = true;
        }
        animator.SetBool("Attacking", isHitting);
        animator.SetInteger("attackTurn", attackTurn);
        if(attackTurn > 3)
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
            isHitting = true;
            Movement.canDash = false;
            if(attackTurn == 2)
            {
                audioManager.PlaySFX(audioManager.explosion);
            }
            else
            {
                audioManager.PlaySFX(audioManager.hit);
            }         
            firstAttackTime = Time.time;
            nextAttackTime = Time.time + attackTime;
            if (attackTurn >= 3)
            {
                attackTurn = 1;
            }
            else
            {
                attackTurn++;
            }
        }
    }
    bool CanAttack() {
        if(Time.time > nextAttackTime&&!Movement.isDead&&!Movement.isDashing)
        {
            isHitting = true;
            nextAttackTime = Time.time + attackTime;
            return true;
        }
        else
        {
            return false;
        }
    }
}
