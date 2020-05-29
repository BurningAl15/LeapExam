using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridChipChecker : MonoBehaviour
{
    public static GridChipChecker _instance;
    
    [SerializeField] private List<ChipLogic> chips=new List<ChipLogic>();

    private int elementsFinished;

    public void AddElementsFinished()
    {
        elementsFinished++;
    }

    public void RemoveElementsFinished()
    {
        elementsFinished--;
    }
    
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if(_instance!=null)
            Destroy(this.gameObject);
    }

    void Update()
    {
        if(chips.Count == elementsFinished)
            print("Finished");
    }
}
