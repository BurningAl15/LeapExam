using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] protected int maxHealthPoints;
    [SerializeField] protected int currentHealthPoints;
    [SerializeField] protected bool isAlive;
    
    protected virtual void Awake()
    {
        currentHealthPoints = maxHealthPoints;
        isAlive = true;
    }

    public virtual void DoDamage(int _damagePoints)
    {
        if (isAlive)
        {
            currentHealthPoints -= _damagePoints;
            if (currentHealthPoints <= 0)
            {
                currentHealthPoints = 0;
                isAlive = false;
            }
        }
    }
}
