using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CharacterController2D : MonoBehaviour
{
    [Header("Components")] [SerializeField]
    private Animator anim;

    [SerializeField] private Rigidbody2D rgb;

    [Header("Movement")] [SerializeField] private float speed;
    private int direction = 1;

    [Header("Jump")] [SerializeField] private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpForce;

    [Header("Melee Attack")] [SerializeField]
    private bool meleeAttack = false;

    [SerializeField] private int meleeAttackIndex = -1;

    [Header("Range Attack")] [SerializeField]
    private bool rangeAttack = false;

    [SerializeField] private int rangeAttackIndex = -1;

    [Header("Air Attack")] [SerializeField]
    private bool airAttack = false;

    [SerializeField] private int airAttackIndex = -1;

    private bool finishAttack = false;

    [Header("Attack Chain Timer")] [SerializeField]
    private float attackChainMaxTime;

    [SerializeField] private float attackChainTimer;
    
    [Header("Roll")]
    [SerializeField] private bool rolling = false;

    void Start()
    {
        attackChainTimer = attackChainMaxTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if(isGrounded)
                Melee_Attack();
            else if (!isGrounded)
                Air_Attack();                
        }

        if (Input.GetKeyDown(KeyCode.L))
            Roll();

        if (meleeAttack || rangeAttack || airAttack)
        {
            attackChainTimer -= Time.deltaTime;
            if (attackChainTimer <= 0)
            {
                if (meleeAttack)
                {
                    meleeAttack = false;
                    anim.SetBool("MeleeAttack", meleeAttack);

                    meleeAttackIndex = -1;
                    anim.SetInteger("MeleeAttackIndex", meleeAttackIndex);
                }
                else if (rangeAttack)
                {
                    rangeAttack = false;
                    anim.SetBool("RangeAttack", rangeAttack);
                    
                    rangeAttackIndex = -1;
                    anim.SetInteger("RangeAttackIndex", rangeAttackIndex);
                }
                else
                {
                    airAttack = false;
                    anim.SetBool("AirAttack", airAttack);

                    airAttackIndex = -1;
                    anim.SetInteger("AirAttackIndex", airAttackIndex);
                }

                rgb.gravityScale = .2f;

                attackChainTimer = attackChainMaxTime;
            }
        }

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);


        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            Jump();
    }
    
    private void FixedUpdate()
    {
        Horizontal_Movement();
        CheckVerticalSpeed();
    }

    void Horizontal_Movement()
    {
        if (!rolling)
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), rgb.velocity.y, 0);

            if (input.x > 0.05f)
                direction = 1;
            else if (input.x < -0.05f)
                direction = -1;

            transform.localScale = new Vector3(direction, 1, 1);

            // rgb.velocity = input * speed;
            // anim.SetFloat("Speed", Mathf.Abs(input.x));
            transform.position += Time.deltaTime * speed * input;
            anim.SetFloat("Speed", Mathf.Abs(input.x));
        }
    }

    void Melee_Attack()
    {
        if (!finishAttack)
        {
            rgb.gravityScale = 0;
            rgb.velocity=Vector2.zero;
            
            meleeAttack = true;
            anim.SetBool("MeleeAttack", meleeAttack);
            meleeAttackIndex++;

            if (meleeAttackIndex > 2)
                meleeAttackIndex = 0;

            anim.SetInteger("MeleeAttackIndex", meleeAttackIndex);

            attackChainTimer = attackChainMaxTime;
        }
    }

    void Range_Attack()
    {
        
    }

    void Air_Attack()
    {
        if (!finishAttack)
        {
            rgb.gravityScale = 0;
            rgb.velocity=Vector2.zero;
            
            airAttack = true;
            anim.SetBool("AirAttack", airAttack);
            airAttackIndex++;

            if (airAttackIndex > 2)
                airAttackIndex = 0;

            anim.SetInteger("AirAttackIndex", airAttackIndex);

            attackChainTimer = attackChainMaxTime;
        }
    }
    
    void Jump()
    {
        rgb.velocity = Vector2.up * jumpForce;

        anim.SetTrigger("HasJumped");
        anim.SetBool("Grounded", isGrounded);
    }

    void Roll()
    {
        rolling = true;
        // rgb.AddForce(Vector2.right * direction * 100f, ForceMode2D.Force);
        rgb.AddForce(Vector2.right * direction * 100f, ForceMode2D.Force);
        anim.SetTrigger("Roll");
    }

    void CheckVerticalSpeed()
    {
        anim.SetFloat("VerticalSpeed", rgb.velocity.y);
        anim.SetBool("Grounded", isGrounded);
    }
    
    public void FinishRolling()
    {
        rgb.velocity = Vector2.zero;
        rolling = false;
    }
    
    public void FinishAttack()
    {
        finishAttack = false;
        // print("Calling FinishAttack");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,0,.5f);
        Gizmos.DrawSphere(feetPos.transform.position,checkRadius);
    }
}
