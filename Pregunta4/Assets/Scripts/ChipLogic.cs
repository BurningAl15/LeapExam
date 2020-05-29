using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipLogic : MonoBehaviour
{
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

    [SerializeField] private bool hasBeenConnected = false;

    public bool HasBeenConnected
    {
        get => hasBeenConnected;
        set => hasBeenConnected = value;
    }
}
