﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rgb;
    [SerializeField] private float speed;

    public void ThrowWeapon(int _direction)
    {
        transform.localScale = new Vector3(_direction, 1, 1);
        rgb.velocity=Vector2.right*_direction*speed;
    }
}
