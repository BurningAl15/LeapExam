using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridChipChecker : MonoBehaviour
{
    public enum GameState
    {
        Wait,Play,Win
    }

    public GameState gameState;
    
    public static GridChipChecker _instance;
    
    [SerializeField] private List<ChipLogic> chips=new List<ChipLogic>();

    private int elementsFinished;

    private bool finishGame = false;
    
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if(_instance!=null)
            Destroy(this.gameObject);

        gameState = GameState.Wait;
        ColorUtils._instance.RandomizeColor();
    }

    void Update()
    {
        if (chips.Count == elementsFinished && !finishGame)
        {
            ChangeToWin();            
            finishGame = true;
        }
    }

    #region Connected Chips Utils

    public void AddConnectedChips()
    {
        elementsFinished++;
    }

    public void RemoveConnectedChips()
    {
        elementsFinished--;
    }
    #endregion

    #region State Utils
    public void ChangeToPlay()
    {
        gameState = GameState.Play;
    }
    
    public void ChangeToWin()
    {
        gameState = GameState.Win;
    }

    public bool GetState()
    {
        return gameState != GameState.Play ? false : true;
    }    

    #endregion
}
