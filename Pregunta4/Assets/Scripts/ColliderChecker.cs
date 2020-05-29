﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderChecker : MonoBehaviour
{
    [SerializeField] private bool chipIsColliding;

    public bool ChipIsColliding
    {
        get => chipIsColliding;
        set => chipIsColliding = value;
    }

    [SerializeField] private ChipLogic chipLogic;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("ChipColliding") && !chipIsColliding)
        {
            chipIsColliding = true;
            chipLogic.Index++;

            if (!chipLogic.HasBeenConnected)
            {
                if (chipLogic.chipType == ChipLogic.ChipType.Single && chipLogic.Index == 1)
                {
                    chipLogic.HasBeenConnected = true;
                    GridChipChecker._instance.AddElementsFinished();
                }
                else if (chipLogic.chipType == ChipLogic.ChipType.Double && chipLogic.Index == 2)
                {
                    chipLogic.HasBeenConnected = true;
                    GridChipChecker._instance.AddElementsFinished();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ChipColliding") && chipIsColliding)
        {
            chipIsColliding = false;
            chipLogic.Index--;
            
            if (chipLogic.HasBeenConnected)
            {
                if (chipLogic.chipType == ChipLogic.ChipType.Single && chipLogic.Index != 1)
                {
                    chipLogic.HasBeenConnected = false;
                    GridChipChecker._instance.RemoveElementsFinished();
                }
                else if (chipLogic.chipType == ChipLogic.ChipType.Double && chipLogic.Index != 2)
                {
                    chipLogic.HasBeenConnected = false;
                    GridChipChecker._instance.RemoveElementsFinished();
                }
            }
        }
    }
}
