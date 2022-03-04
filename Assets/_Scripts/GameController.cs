using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle}


public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] BattleSystem _battleSystem;
    [SerializeField] Camera _worldCamera;
    
    
    
    
    GameState _state;


    private void Start()
    {
        _playerController.OnEncountered += StartBattle;
        _battleSystem.OnBattleOver += EndBattle;
    }

    private void StartBattle()
    {
        _state = GameState.Battle;
        _battleSystem.gameObject.SetActive(true);
        _worldCamera.gameObject.SetActive(false);
    }
    private void EndBattle(bool won)
    {
        _state = GameState.FreeRoam;
        _battleSystem.gameObject.SetActive(false);
        _worldCamera.gameObject.SetActive(true);
    }


    private void Update()
    {
        if(_state == GameState.FreeRoam)
        {
            _playerController.HandleUpdate();
        }
        else if (_state == GameState.Battle)
        {
            _battleSystem.HandleUpdate();

        }
    }


}
