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
   
   protected override void Awake()
   {
      base.Awake();
      lockOnPoint.SetActive(false);
   }

   public void ActivateEnemy()
   {
      lockOnPoint.SetActive(true);
   }
   public void DeactivateEnemy()
   {
      lockOnPoint.SetActive(false);
   }

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
   
   public void OnMeleeAttackConnected()
   {
      // Interlink._instance.TryInterlink(this);
   }

   public void GetEnemyAttacks()
   {
      OrquestraDirector._instance.Wait_ForAttack(enemyAttack.attackDuration);
   }

   private void Update()
   {
      if (!OrquestraDirector._instance.SomeoneAttacked)
      {
         //Attack
      }
      else
      {
         return;
      }
   }
}
