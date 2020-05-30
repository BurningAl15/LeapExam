using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class CharacterController2D : MonoBehaviour
{
    [Header("Components")] [SerializeField]
    private Animator anim;

    [SerializeField] private Rigidbody2D rgb;

    [Header("Movement")] 
    [SerializeField] private float speed;
    private int direction = 1;

    [Header("Jump")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityScale;
    
    [Header("Melee Attack")] 
    [SerializeField] private bool meleeAttack = false;
    [SerializeField] private int meleeAttackIndex = -1;

    [Header("Range Attack")] 
    [SerializeField] private bool rangeAttack = false;
    [SerializeField] private int rangeAttackIndex = -1;

    [SerializeField] List<Transform> throwPoints=new List<Transform>();
    [SerializeField] private GameObject knifePrefab;
    
    [Header("Air Attack")] 
    [SerializeField] private bool airAttack = false;
    [SerializeField] private int airAttackIndex = -1;

    private bool finishAttack = false;

    [Header("Attack Chain Timer")]
    [SerializeField] private float attackChainMaxDelay;

    [SerializeField] private float attackChainTimer;
    
    [Header("Roll")]
    [SerializeField] private bool rolling = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        gravityScale = rgb.gravityScale;
        attackChainTimer = attackChainMaxDelay;
    }

    void AttackTrigger( string _attackType,string _attackTypeIndex, bool _attack,int _attackIndex)
    {
        // if (_attack)
        // {
            attackChainTimer -= 0.01f;

            if (attackChainTimer < 0)
            {
                _attack = false;
                anim.SetBool(_attackType, _attack);
                _attackIndex = -1;
                anim.SetInteger(_attackTypeIndex, _attackIndex);
                
                rgb.gravityScale = gravityScale;
                attackChainTimer = attackChainMaxDelay;
            }
        // }
    }
    
    void Update()
    {
        if (meleeAttack && !rangeAttack && !airAttack)
        {
            AttackTrigger("MeleeAttack","MeleeAttackIndex",meleeAttack,meleeAttackIndex);
        }
        else if (!meleeAttack && rangeAttack && !airAttack)
        {
            AttackTrigger("RangeAttack","RangeAttackIndex",rangeAttack,rangeAttackIndex);
        }else if (!meleeAttack && !rangeAttack && airAttack)
        {
            AttackTrigger("AirAttack","AirAttackIndex",airAttack,airAttackIndex);
        }
        else
        {
            meleeAttack = false;
            rangeAttack = false;
            airAttack = false;
                
            anim.SetBool("MeleeAttack", meleeAttack);
            anim.SetBool("RangeAttack", rangeAttack);
            anim.SetBool("AirAttack", airAttack);
        
            meleeAttackIndex = -1;
            rangeAttackIndex = -1;
            airAttackIndex = -1;
        
            anim.SetInteger("MeleeAttackIndex", meleeAttackIndex);
            anim.SetInteger("RangeAttackIndex", rangeAttackIndex);
            anim.SetInteger("AirAttackIndex", airAttackIndex);
        
            rgb.gravityScale = gravityScale;
            attackChainTimer = attackChainMaxDelay;
        }

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        
        if (isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.Space))
                Jump();
            if (Input.GetKeyDown(KeyCode.J))
                Melee_Attack();
            if (Input.GetKeyDown(KeyCode.I))
                Range_Attack();
            if (Input.GetKeyDown(KeyCode.L))
                Roll();
        }
        else if (!isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.J))
                Air_Attack();                
        }
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
            Vector3 input = new Vector3( isGrounded ? Input.GetAxisRaw("Horizontal") * speed : rgb.velocity.x, rgb.velocity.y, 0);
            
            if (input.x > 0.05f)
                direction = 1;
            else if (input.x < -0.05f)
                direction = -1;

            transform.localScale = new Vector3(direction, 1, 1);

            rgb.velocity = input;
            anim.SetFloat("Speed", Mathf.Abs(input.x));
        }
    }

    void Melee_Attack()
    {
        if (!finishAttack && !rolling)
        {
            rgb.gravityScale = 0;
            rgb.velocity=Vector2.zero;
            
            meleeAttack = true;
            rangeAttack = false;
            airAttack = false;
            
            anim.SetBool("MeleeAttack", meleeAttack);
            anim.SetBool("AirAttack", airAttack);
            anim.SetBool("RangeAttack", rangeAttack);

            meleeAttackIndex++;

            if (meleeAttackIndex > 2)
                meleeAttackIndex = 0;

            anim.SetInteger("MeleeAttackIndex", meleeAttackIndex);
           
            attackChainTimer = attackChainMaxDelay;
        }
    }

    void Range_Attack()
    {
        if (!finishAttack && !rolling)
        {
            rgb.gravityScale = 0;
            rgb.velocity=Vector2.zero;
            
            meleeAttack = false;
            rangeAttack = true;
            airAttack = false;
            
            anim.SetBool("MeleeAttack", meleeAttack);
            anim.SetBool("AirAttack", airAttack);
            anim.SetBool("RangeAttack", rangeAttack);
            
            rangeAttackIndex++;

            if (rangeAttackIndex > 1)
                rangeAttackIndex = 0;
            
            anim.SetInteger("RangeAttackIndex", rangeAttackIndex);

            attackChainTimer = attackChainMaxDelay;
        }
    }

    void Air_Attack()
    {
        if (!finishAttack)
        {
            rgb.gravityScale = 0;
            rgb.velocity=Vector2.zero;
            
            meleeAttack = false;
            rangeAttack = false;
            airAttack = true;

            anim.SetBool("MeleeAttack", meleeAttack);
            anim.SetBool("AirAttack", airAttack);
            anim.SetBool("RangeAttack", rangeAttack);

            airAttackIndex++;

            if (airAttackIndex > 2)
                airAttackIndex = 0;

            anim.SetInteger("AirAttackIndex", airAttackIndex);

            attackChainTimer = attackChainMaxDelay;
        }
    }
    
    void Jump()
    {
        if (!rolling)
        {
            rgb.velocity = new Vector2(rgb.velocity.x,1 * jumpForce);

            anim.SetTrigger("HasJumped");
            anim.SetBool("Grounded", isGrounded);
        }
    }

    void Roll()
    {
        if (!rolling)
        {
            rolling = true;

            Vector2 rollforceAmount=Vector2.zero;
            if (isGrounded)
                rollforceAmount = Vector2.right * direction * 100f;
            else if(!isGrounded)
                rollforceAmount = Vector2.right * direction * 50f;
        
            rgb.AddForce(rollforceAmount, ForceMode2D.Force);
            anim.SetTrigger("Roll");
        }
    }

    void CheckVerticalSpeed()
    {
        anim.SetFloat("VerticalSpeed", rgb.velocity.y);
        anim.SetBool("Grounded", isGrounded);
    }

    public void ThrowWeapon()
    {
        GameObject knife = Instantiate(knifePrefab, throwPoints[rangeAttackIndex<=0?0:1].position, Quaternion.identity);
        knife.GetComponent<RangeWeapon>().ThrowWeapon(direction);
    }
    
    public void FinishRolling()
    {
        rgb.velocity = Vector2.zero;
        rolling = false;
    }
    
    public void FinishAttack()
    {
        finishAttack = false;
        print("Calling FinishAttack");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,0,.5f);
        Gizmos.DrawSphere(feetPos.transform.position,checkRadius);
        Gizmos.color = new Color(0,1,0,.5f);
        Gizmos.DrawSphere(throwPoints[0] .transform.position,checkRadius);
        Gizmos.DrawSphere(throwPoints[1] .transform.position,checkRadius);
    }
}
