using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChainAttackManager : MonoBehaviour
{
    public static ChainAttackManager _instance;

    [SerializeField] private int meleeIndex = 0;
    [SerializeField] private int rangeIndex = 0;
    [SerializeField] private int airIndex = 0;

    [SerializeField] private Text chainText;
    private bool isActive = false;
    [SerializeField] private Animator anim;

    private float _currentDelay = 0;
    private float _maxDelay = .31f;
    
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(this.gameObject);
        }

        _currentDelay = _maxDelay;
    }


    private void Update()
    {
        if (isActive)
        {
            _currentDelay -= 0.01f;
            GetDelay(_currentDelay, _maxDelay);
            if (_currentDelay<=0)
            {
                ChangeAlpha(0);
                isActive = false;
                _currentDelay = _maxDelay;
            }            
        }
    }

    public void GetDelay(float _currentDelay,float _maxDelay)
    {
        ChangeAlpha(_currentDelay/_maxDelay);
    }

    public void CallMessage(bool melee,bool range,bool air)
    {
        print("Call Message - ChainAttackManager");
        if (melee && !range && !air)
        {
            rangeIndex = 0;
            airIndex = 0;
            meleeIndex++;
            chainText.text = "Melee Combo!\nx" + meleeIndex;
        }else if (!melee && range && !air)
        {
            meleeIndex = 0;
            airIndex = 0;
            rangeIndex++;
            chainText.text = "Range Chain!\nx" + rangeIndex;
        }
        else
        {
            if (!melee && !range && air)
            {
                meleeIndex = 0;
                rangeIndex = 0;
                airIndex++;
                chainText.text = "Air Combo!\nx" + airIndex;
            }
            else
            {
                meleeIndex = 0;
                rangeIndex = 0;
                airIndex = 0;
            }
        }
        ChangeAlpha(1);
        anim.SetTrigger("Hit");
        
        _currentDelay = _maxDelay;
        isActive = true;
    }
    
    public void Reset()
    {
        print("Call Reset - ChainAttackManager");
        
        rangeIndex = 0;
        meleeIndex = 0;
        airIndex = 0;
        ChangeAlpha(0);
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