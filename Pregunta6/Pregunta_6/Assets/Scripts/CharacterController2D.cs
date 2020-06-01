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

    [Header("Movement")] [SerializeField] private float speed;
    private int direction = 1;

    [Header("Jump")]
    public bool isGrounded;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityScale;

    [Header("Melee Attack")] [SerializeField]
    private bool meleeAttack = false;
    [SerializeField] private int meleeAttackIndex = -1;
    
    [Header("Range Attack")] [SerializeField]
    private bool rangeAttack = false;

    [SerializeField] private int rangeAttackIndex = -1;

    [SerializeField] List<Transform> throwPoints = new List<Transform>();

    [Header("Air Attack")] [SerializeField]
    private bool airAttack = false;

    [SerializeField] private int airAttackIndex = -1;

    private bool finishAttack = true;

    [Header("Attack Chain Timer")] [SerializeField]
    private float attackChainMaxDelay;

    [SerializeField] private float attackChainTimer;

    [Header("Roll")] [SerializeField] private bool rolling = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        gravityScale = rgb.gravityScale;
        attackChainTimer = attackChainMaxDelay;
        ChainAttackManager._instance.Reset();
    }

    #region Input Methods

  public void Melee_Attack()
    {
        if (finishAttack && !rolling)
        {
            attackChainTimer = attackChainMaxDelay;

            //Cancel movement in x,y and turn off the gravity for the character            
            rgb.gravityScale = 0;
            rgb.velocity = Vector2.zero;

            //Start the attack chain            
            meleeAttack = true;
            rangeAttack = false;
            //Let us know in which attack we are
            meleeAttackIndex++;

            if (meleeAttackIndex > 2)
                meleeAttackIndex = 0;
            
            anim.SetBool("MeleeAttack", meleeAttack);
            anim.SetInteger("MeleeAttackIndex", meleeAttackIndex);
            anim.SetBool("RangeAttack", rangeAttack);
        }
    }

    public void Range_Attack()
    {
        if (finishAttack && !rolling)
        {
            attackChainTimer = attackChainMaxDelay;

            //Cancel movement in x,y and turn off the gravity for the character            
            rgb.gravityScale = 0;
            rgb.velocity = Vector2.zero;

            //Start the attack chain            
            rangeAttack = true;
            meleeAttack = false;

            //Let us know in which attack we are
            rangeAttackIndex++;

            if (rangeAttackIndex > 1)
                rangeAttackIndex = 0;
            
            anim.SetBool("RangeAttack", rangeAttack);
            anim.SetInteger("RangeAttackIndex", rangeAttackIndex);
            anim.SetBool("MeleeAttack", meleeAttack);
        }
    }

    public void Air_Attack()
    {
        if (finishAttack)
        {
            finishAttack = false;
            attackChainTimer = attackChainMaxDelay;

            //Cancel movement in x,y and turn off the gravity for the character            
            rgb.gravityScale = 0;
            rgb.velocity = Vector2.zero;

            //Start the attack chain            
            airAttack = true;

            //Let us know in which attack we are
            airAttackIndex++;

            if (airAttackIndex > 2)
                airAttackIndex = 0;

            anim.SetBool("AirAttack", airAttack);
            anim.SetInteger("AirAttackIndex", airAttackIndex);
        }
    }

    public void Jump()
    {
        if (!rolling)
        {
            rgb.velocity = new Vector2(rgb.velocity.x, 1 * jumpForce);

            anim.SetTrigger("HasJumped");
            anim.SetBool("Grounded", isGrounded);
        }
    }

    public void Roll()
    {
        if (!rolling)
        {
            rolling = true;

            Vector2 rollforceAmount = isGrounded ? Vector2.right * direction * 100f : Vector2.right * direction * 50f;

            rgb.AddForce(rollforceAmount, ForceMode2D.Force);
            anim.SetTrigger("Roll");
        }
    }

    public void Horizontal_Movement()
    {
        if (!rolling && finishAttack)
        {
            Vector3 input = Vector3.zero;

            //Inputs for directions to move
            if (Input.GetKey(KeyCode.A))
                input = new Vector3(-1 * speed, rgb.velocity.y, 0);
            else if (Input.GetKey(KeyCode.D))
                input = new Vector3(1 * speed, rgb.velocity.y, 0);
            else
                input = new Vector3(0, rgb.velocity.y, 0);

            //To don't change the direction where seeing in mid-air
            if (isGrounded)
            {
                //Flip
                if (input.x > 0.05f)
                    direction = 1;
                else if (input.x < -0.05f)
                    direction = -1;
            }

            transform.localScale = new Vector3(direction, 1, 1);

            rgb.velocity = isGrounded ? input : new Vector3(rgb.velocity.x, rgb.velocity.y, 0);
            anim.SetFloat("Speed", Mathf.Abs(input.x));
        }
    }

    #endregion

    #region Animation Triggers

    public bool GetAttackType(string _attackType)
    {
        switch (_attackType)
        {
            case "melee":
                return meleeAttack && !rangeAttack && !airAttack;

            case "range":
                return !meleeAttack && rangeAttack && !airAttack;

            case "air":
                return !meleeAttack && !rangeAttack && airAttack;

            default:
                return false;
        }
    }

    public void ResetAttackTrigger(string _attackType, string _attackTypeIndex)
    {
        if (finishAttack)
        {
            if (attackChainTimer >= 0)
            {
                attackChainTimer -= 0.01f;

                // ChainAttackManager._instance.GetDelay(attackChainTimer,attackChainMaxDelay);
                print("Running");
            }
            else if (attackChainTimer < 0)
            {
                switch (_attackType)
                {
                    case "MeleeAttack":
                        meleeAttack = false;
                        anim.SetBool(_attackType, meleeAttack);
                        meleeAttackIndex = -1;
                        anim.SetInteger(_attackTypeIndex, meleeAttackIndex);
                        break;
                    case "RangeAttack":
                        rangeAttack = false;
                        anim.SetBool(_attackType, rangeAttack);
                        rangeAttackIndex = -1;
                        anim.SetInteger(_attackTypeIndex, rangeAttackIndex);
                        break;
                    case "AirAttack":
                        airAttack = false;
                        anim.SetBool(_attackType, airAttack);
                        airAttackIndex = -1;
                        anim.SetInteger(_attackTypeIndex, airAttackIndex);
                        break;
                }

                // ChainAttackManager._instance.Reset();
                print("End");

                rgb.gravityScale = gravityScale;
                attackChainTimer = attackChainMaxDelay;
            }
        }
    }

    public void ResetAttackAnimations()
    {
        if (finishAttack)
        {
            meleeAttack = false;
            rangeAttack = false;
            airAttack = false;

            meleeAttackIndex = -1;
            rangeAttackIndex = -1;
            airAttackIndex = -1;
            
            anim.SetBool("MeleeAttack", meleeAttack);
            anim.SetBool("RangeAttack", rangeAttack);
            anim.SetBool("AirAttack", airAttack);

            anim.SetInteger("MeleeAttackIndex", meleeAttackIndex);
            anim.SetInteger("RangeAttackIndex", rangeAttackIndex);
            anim.SetInteger("AirAttackIndex", airAttackIndex);

            ChainAttackManager._instance.Reset();

            rgb.gravityScale = gravityScale;
            attackChainTimer = attackChainMaxDelay;
        }
    }

    #endregion

    #region Utils

    public void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
    }

    public void CheckVerticalSpeed()
    {
        anim.SetFloat("VerticalSpeed", rgb.velocity.y);
        anim.SetBool("Grounded", isGrounded);
    }

    public void ThrowWeapon()
    {
        GameObject knife = ObjectPooler._instance.GetPooledObject();
        if (knife != null)
        {
            print("Call Knife");
            knife.transform.position = throwPoints[rangeAttackIndex <= 0 ? 0 : 1].position;
            knife.transform.rotation = Quaternion.identity;
            knife.SetActive(true);
            knife.GetComponent<RangeWeapon>().ThrowWeapon(direction);
        }
    }

    public void Camera_Shake_Hit1()
    {
        CameraShake._instance._Shake(.05f,.05f);
    }
    public void Camera_Shake_Hit2()
    {
        CameraShake._instance._Shake(.1f,.075f);
    }
    public void Camera_Shake_Hit3()
    {
        CameraShake._instance._Shake(.05f,.1f);
    }

    #endregion

    #region End Input Actions

    public void FinishRolling()
    {
        rgb.velocity = Vector2.zero;
        rolling = false;
    }

    public void FinishAttack()
    {
        finishAttack = true;
        ChainAttackManager._instance.CallMessage(meleeAttack,rangeAttack,airAttack);

        print("Calling FinishAttack");
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawSphere(feetPos.transform.position, checkRadius);

        Gizmos.color = new Color(0, 1, 0, .5f);
        Gizmos.DrawSphere(throwPoints[0].transform.position, checkRadius);
        Gizmos.DrawSphere(throwPoints[1].transform.position, checkRadius);
    }
}