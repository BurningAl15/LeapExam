using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] protected int maxHealthPoints;
    [SerializeField] protected int currentHealthPoints;

    protected virtual void Awake()
    {
        currentHealthPoints = maxHealthPoints;
    }

    public virtual void DoDamage(int _damagePoints)
    {
        currentHealthPoints -= _damagePoints;
        if (currentHealthPoints <= 0)
        {
            currentHealthPoints = 0;
        }
    }
}
