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

    #region Collisions
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("ChipColliding") && !chipIsColliding)
        {
            chipIsColliding = true;
            chipLogic.ConnectedElementsCounter++;
            
            chipLogic.ChangeColor(ColorUtils._instance.GetCurrentColor_InGame());
            
            if (!chipLogic.HasBeenConnected)
            {
                if (chipLogic.chipType == ChipLogic.ChipType.Single && chipLogic.ConnectedElementsCounter == 1)
                {
                    chipLogic.HasBeenConnected = true;
                    GridChipChecker._instance.AddConnectedChips();
                }
                else if (chipLogic.chipType == ChipLogic.ChipType.Double && chipLogic.ConnectedElementsCounter == 2)
                {
                    chipLogic.HasBeenConnected = true;
                    GridChipChecker._instance.AddConnectedChips();
                }
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ChipColliding") && chipIsColliding)
        {
            chipIsColliding = false;
            chipLogic.ConnectedElementsCounter--;

            chipLogic.ChangeColor(Color.white);
            
            if (chipLogic.HasBeenConnected)
            {
                if (chipLogic.chipType == ChipLogic.ChipType.Single && chipLogic.ConnectedElementsCounter != 1)
                {
                    chipLogic.HasBeenConnected = false;
                    GridChipChecker._instance.RemoveConnectedChips();
                }
                else if (chipLogic.chipType == ChipLogic.ChipType.Double && chipLogic.ConnectedElementsCounter != 2)
                {
                    chipLogic.HasBeenConnected = false;
                    GridChipChecker._instance.RemoveConnectedChips();
                }
            }
        }
    }

    #endregion
}
