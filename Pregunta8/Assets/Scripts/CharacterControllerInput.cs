using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerInput : MonoBehaviour
{
    [SerializeField] private CharacterController2D player;
    
    void Update()
    {
        //Reset if melee
        if (player.GetAttackType("melee"))
            player.ResetAttackTrigger("MeleeAttack", "MeleeAttackIndex");
        //Reset if range
        else if (player.GetAttackType("range"))
            player.ResetAttackTrigger("RangeAttack", "RangeAttackIndex");
        else
        {
            //Reset if air
            if (player.GetAttackType("air"))
                player.ResetAttackTrigger("AirAttack", "AirAttackIndex");
            //Reset all
            else
                player.ResetAttackAnimations();
        }

        player.CheckGround();

        if (player.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                player.Jump();
            if (player.GetFinishAttack())
            {
                if (Input.GetKeyDown(KeyCode.J))
                    player.Melee_Attack();
                if (Input.GetKeyDown(KeyCode.I))
                    player.Range_Attack();
            }
            if (Input.GetKeyDown(KeyCode.L))
                player.Roll();
        }
        else if (!player.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.J))
                player.Air_Attack();
        }
    }
    private void FixedUpdate()
    {
        player.Horizontal_Movement();
        player.CheckVerticalSpeed();
    }

}
