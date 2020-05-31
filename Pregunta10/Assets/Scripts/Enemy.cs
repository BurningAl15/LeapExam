using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAttack
{
   public float attackDuration;
   public string[] attackTags;
   public string[] reactionTags;
}

public class Enemy : HealthSystem
{
   [SerializeField] private GameObject lockOnPoint;

   [SerializeField] private Animator anim;

   public int id;

   public string enemyType;

   public EnemyAttack enemyAttack;

   [SerializeField] private bool isAttacking=false;

   public bool CanAttackInOrquestra=false;
   
   protected override void Awake()
   {
      base.Awake();
      lockOnPoint.SetActive(false);
   }

   #region Lock On Methods

   public void ActivateEnemy()
   {
      lockOnPoint.SetActive(true);
   }
   public void DeactivateEnemy()
   {
      lockOnPoint.SetActive(false);
   }

   #endregion

   public override void DoDamage(int _damagePoints)
   {
      base.DoDamage(_damagePoints);
      
      if (isAlive)
      {
         if(currentHealthPoints>0)
            anim.SetTrigger("Hit");
      }
      else
      {
         anim.SetTrigger("Die");
         GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
         GetComponent<Collider2D>().enabled = false;
         this.enabled = false;
      }
   }

   public bool IsEnemyAlive()
   {
      return isAlive;
   }

   #region Interlink

   public void OnMeleeAttackConnected()
   {
      // Interlink._instance.TryInterlink(this);
   }

   #endregion

   #region Orquestra Director
   
   public void GetEnemyAttacks()
   {
      OrquestraDirector._instance.Wait_ForAttack(enemyAttack.attackDuration,this);
   }

   private void Update()
   {
      if (!OrquestraDirector._instance.IsOrquestraAttackOn ^ CanAttackInOrquestra)
      {
         if (!OrquestraDirector._instance.EnemyType.Contains(enemyType) || !OrquestraDirector._instance.EnemyType.Contains("none"))
         {
            if (Input.GetKeyDown(KeyCode.Q))
            {
               if(this.name.Contains("Rana"))
                  Attack();
         
               print("El sistema ha permitido el ataque del enemigo " + this.name);

            }
            if (Input.GetKeyDown(KeyCode.W))
            {
               if(this.name.Contains("Tronco"))
                  Attack();

               print("El sistema ha permitido el ataque del enemigo " + this.name);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
               if(this.name.Contains("Mono"))
                  Attack();

               print("El sistema ha permitido el ataque del enemigo " + this.name);
            }
         }
         else
         {
            OrquestraDirector._instance.OrquestraMessage_SameType(enemyType);
         }
      }
   }

   public void Attack()
   {
      if (!isAttacking)
      {
         isAttacking = true;
         GetEnemyAttacks();
         anim.SetTrigger("Attack");
         print("Enemigo " + this.name + " Intentó atacar");
      }
   }

   public void EndAttackState()
   {
      // print("Attack animation end");
      isAttacking = false;
   }
   #endregion
}
