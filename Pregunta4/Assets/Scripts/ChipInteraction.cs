using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipInteraction : MonoBehaviour
{
    private void OnMouseDown()
    {
        if(GridChipChecker._instance.GetState())
            transform.Rotate(Vector3.forward*-90f);
    }
}
