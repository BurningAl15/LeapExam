using System.Collections;
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
            print("Colliding");
            chipIsColliding = true;
            chipLogic.Index++;
        }
        else
        {
            print("---Colliding---");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ChipColliding") && chipIsColliding)
        {
            print("Not Colliding");
            chipIsColliding = false;
            chipLogic.Index--;
        }
        else
        {
            print("---Not Colliding---");
        }
    }
}
