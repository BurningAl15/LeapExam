using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : HealthSystem
{
   [SerializeField] private GameObject lockOnPoint;

   [SerializeField] private Animator anim;

   public int id;
   
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
         else if (currentHealthPoints < 0)
         {
            anim.SetTrigger("Die");
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
         }
      }
   }

   public bool IsEnemyAlive()
   {
      return isAlive;
   }
   
   public void OnMeleeAttackConnected()
   {
      Interlink._instance.TryInterlink(this);
   }
}
