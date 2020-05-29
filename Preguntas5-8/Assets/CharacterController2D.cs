using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private Rigidbody2D rgb;
    [SerializeField] private float speed;

    [SerializeField] private bool meleeAttack = false;
    [SerializeField] private bool airAttack = false;
    [SerializeField] private bool rangeAttack = false;

    [SerializeField] private int meleeAttackIndex = -1;
    [SerializeField] private int rangeAttackIndex = -1;
    [SerializeField] private int airAttackIndex = -1;

    private bool finishAttack = false;

    [SerializeField] private float attackChainMaxTime;
    [SerializeField] private float attackChainTimer;
    void Start()
    {
        attackChainTimer = attackChainMaxTime;
    }

    void Update()
    {
        Horizontal_Movement();

        if (Input.GetKeyDown(KeyCode.J))
        {
            Melee_Attack();
        }
        
        if (meleeAttack)
        {
            attackChainTimer -= Time.deltaTime;
            if (attackChainTimer<=0)
            {
                meleeAttack = false;
                anim.SetBool("MeleeAttack", meleeAttack);
                
                rangeAttack = false;
                airAttack = false;

                meleeAttackIndex = -1;
                anim.SetInteger("MeleeAttackIndex",meleeAttackIndex);

                rangeAttackIndex = -1;
                airAttackIndex = -1;
        
                attackChainTimer = attackChainMaxTime;
            }
        }
    }

    void Horizontal_Movement()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),rgb.velocity.y);
        rgb.velocity = input * speed;

        if (input.x > 0.05f)
        {
            transform.localScale=Vector3.one;
        }else if (input.x < -0.05f)
        {
            transform.localScale=new Vector3(-1,1,1);
        }
        
        anim.SetFloat("Speed", Mathf.Abs(rgb.velocity.x));
    }

    void Melee_Attack()
    {
        if (!finishAttack)
        {
            meleeAttack = true;
            anim.SetBool("MeleeAttack", meleeAttack);
            meleeAttackIndex++;

            if (meleeAttackIndex > 2)
                meleeAttackIndex = 0;
            
            anim.SetInteger("MeleeAttackIndex",meleeAttackIndex);
            
            attackChainTimer = attackChainMaxTime;
            // finishAttack = true;
        }
    }

    void Jump()
    {
        anim.SetFloat("VerticalSpeed", rgb.velocity.y);
    }

    public void FinishAttack()
    {
        // finishAttack = true;
        finishAttack = false;
        print("Calling FinishAttack");
    }
}
