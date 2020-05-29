using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipLogic : MonoBehaviour
{
    [SerializeField] private bool chipIsColliding;
    public enum ChipType
    {
        Single,Double
    }

    public ChipType chipType;

    [SerializeField] private List<ColliderChecker> chipColliders = new List<ColliderChecker>();
    
    [SerializeField] private int index = 0;

    public int Index
    {
        get => index;
        set => index = value;
    }

    private bool hasChanged = false;
    
    void Start()
    {

    }

    void Update()
    {
        // if (hasChanged)
        // {
        //     
        // }
    }
  
}
