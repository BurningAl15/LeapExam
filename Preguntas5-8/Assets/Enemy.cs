using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : HealthSystem
{
   [SerializeField] private GameObject lockOnPoint;

   [SerializeField] private Animator anim;
   
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

   public override void Damage(int _damagePoints)
   {
      base.Damage(_damagePoints);
      if (currentHealthPoints > 0)
      {
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
}
