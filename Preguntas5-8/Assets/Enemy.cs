using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] private GameObject lockOnPoint;

   private void Awake()
   {
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
}
