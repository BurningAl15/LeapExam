﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChainAttackManager : MonoBehaviour
{
    public static ChainAttackManager _instance;

    [SerializeField] private int chainIndex = 0;

    [SerializeField] private Text chainText;

    [SerializeField] private Animator anim;

    void Start()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void GetDelay(float _currentDelay,float _maxDelay)
    {
        ChangeAlpha(_currentDelay/_maxDelay);
    }

    public void CallMessage()
    {
        chainIndex++;
        chainText.text = "Chain Attack!\nx" + chainIndex;
        ChangeAlpha(1);
        anim.SetTrigger("Hit");
    }

    public void Reset()
    {
        ChangeAlpha(0);
        chainIndex = 0;
        chainText.text = "Chain Attack!\nx" + chainIndex;
    }
    
    /// <summary>
    /// Alpha should be between 0 and 1
    /// </summary>
    /// <param name="_alpha"></param>
    void ChangeAlpha(float _alpha)
    {
        Color initialColor = chainText.color;
        chainText.color=new Color(initialColor.r,initialColor.g,initialColor.b,_alpha);
    }
}