using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy }


public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit _playerUnit;
    [SerializeField] BattleUnit _ennemyUnit;
    [SerializeField] BattleHud _playerHud;
    [SerializeField] BattleHud _ennemyHud;
    [SerializeField] BattleDialogBox _dialogBox;


    public event Action<bool> OnBattleOver;

    BattleState _state;

    int _currentAction;
    int _currentMove;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        _playerUnit.Setup();
        _ennemyUnit.Setup();
        _playerHud.SetData(_playerUnit.Monster);
        _ennemyHud.SetData(_ennemyUnit.Monster);

        _dialogBox.SetMoveNames(_playerUnit.Monster.Moves);


        yield return _dialogBox.TypeDialog($@"A wild {_ennemyUnit.Monster.Base.Name} appeared.");

        PlayerAction();


    }

    private void PlayerAction()
    {
        _state = BattleState.PlayerAction;
        StartCoroutine(_dialogBox.TypeDialog("Choose an action"));
        _dialogBox.EnableActionSelector(true);
    }

    private void PlayerMove()
    {
        _state = BattleState.PlayerMove;
        _dialogBox.EnableActionSelector(false);
        _dialogBox.EnableDialogText(false);
        _dialogBox.EnableMoveSelector(true);
    }


    IEnumerator PerformPlayerMove()
    {
        _state = BattleState.Busy;
        var move = _playerUnit.Monster.Moves[_currentMove];
        yield return _dialogBox.TypeDialog($"{_playerUnit.Monster.Base.Name} used {move.Base.Name}");

        _playerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        _ennemyUnit.PlayHitAnimation();
        //yield return new WaitForSeconds(1f);

        var damageDetails = _ennemyUnit.Monster.TakeDamage(move, _playerUnit.Monster);
        yield return _ennemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return _dialogBox.TypeDialog($"{_ennemyUnit.Monster.Base.Name} Fainted");
            _ennemyUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }
    IEnumerator EnemyMove()
    {
        _state = BattleState.EnemyMove;

        var move = _ennemyUnit.Monster.GetRandomMove();
        yield return _dialogBox.TypeDialog($"{_ennemyUnit.Monster.Base.Name} used {move.Base.Name}");


        _ennemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        _playerUnit.PlayHitAnimation();
        //yield return new WaitForSeconds(1f);

        var damageDetails = _playerUnit.Monster.TakeDamage(move, _playerUnit.Monster);
        yield return _playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return _dialogBox.TypeDialog($"{_playerUnit.Monster.Base.Name} Fainted");
            _playerUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
        }
        else
        {
            PlayerAction();
        }
    }


    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
            yield return _dialogBox.TypeDialog("A critical hit !");


        if (damageDetails.TypeEffectiveness > 1f)
            yield return _dialogBox.TypeDialog("Super effective !");
        else if (damageDetails.TypeEffectiveness < 1f)
            yield return _dialogBox.TypeDialog("Not very effective !");
    }



    public void HandleUpdate()
    {
        if (_state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (_state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }


    private void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_currentAction < 1)
                _currentAction++;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (_currentAction > 0)
                _currentAction--;
        }

        _dialogBox.UpdateActionSelection(_currentAction);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_currentAction == 0)
            {
                //fight
                PlayerMove();
            }
            else if (_currentAction == 1)
            {
                //run
            }
        }
    }

    private void HandleMoveSelection()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (_currentMove < _playerUnit.Monster.Moves.Count - 1)
                _currentMove++;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_currentMove > 0)
                _currentMove--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (_currentMove < _playerUnit.Monster.Moves.Count - 2)
                _currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (_currentMove > 1)
                _currentMove -= 2;
        }

        _dialogBox.UpdateMoveSelection(_currentMove, _playerUnit.Monster.Moves[_currentMove]);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _dialogBox.EnableMoveSelector(false);
            _dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());



        }















    }
}
