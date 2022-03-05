using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy, PartyScreen }


public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit _playerUnit;
    [SerializeField] BattleUnit _ennemyUnit;
    [SerializeField] BattleHud _playerHud;
    [SerializeField] BattleHud _ennemyHud;
    [SerializeField] BattleDialogBox _dialogBox;
    [SerializeField] PartyScreen _partyScreen;


    public event Action<bool> OnBattleOver;

    private BattleState _state;

    private int _currentAction;
    private int _currentMove;
    private int _currentMember;

    MonsterParty _playerParty;
    Monster _wildMonster;

    public void StartBattle(MonsterParty playerparty, Monster wildMonster)
    {
        this._playerParty = playerparty;
        this._wildMonster = wildMonster;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        _playerUnit.Setup(_playerParty.GetHealthyMonster());
        _ennemyUnit.Setup(_wildMonster);
        _playerHud.SetData(_playerUnit.Monster);
        _ennemyHud.SetData(_ennemyUnit.Monster);

        _partyScreen.Init();


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
    private void OpenPartyScreen()
    {
        _state = BattleState.PartyScreen;
        _partyScreen.SetPartyData(_playerParty.Monsters);
        _partyScreen.gameObject.SetActive(true);
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
        move.PP--;
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
        move.PP--;
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

            var nextMonster = _playerParty.GetHealthyMonster();
            if (nextMonster != null)
            {
                OpenPartyScreen();
            }
            else
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
        else if (_state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }


    private void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _currentAction++;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _currentAction--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _currentAction += 2;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            _currentAction -= 2;
        }

        _currentAction = Mathf.Clamp(_currentAction, 0, 3);

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
                //Bag
            }
            else if (_currentAction == 2)
            {
                //Monster
                OpenPartyScreen();
            }
            else if (_currentAction == 3)
            {
                //run
            }
        }
    }

    private void HandleMoveSelection()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            _currentMove++;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _currentMove--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            _currentMove -= 2;
        }

        _currentMove = Mathf.Clamp(_currentMove, 0, _playerUnit.Monster.Moves.Count - 1);

        _dialogBox.UpdateMoveSelection(_currentMove, _playerUnit.Monster.Moves[_currentMove]);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _dialogBox.EnableMoveSelector(false);
            _dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _dialogBox.EnableMoveSelector(false);
            _dialogBox.EnableDialogText(true);
            PlayerAction();
        }

    }
    private void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _currentMember++;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _currentMember--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _currentMember += 2;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            _currentMember -= 2;
        }

        _currentMember = Mathf.Clamp(_currentMember, 0, _playerParty.Monsters.Count - 1);

        _partyScreen.UpdateSelectedMember(_currentMember);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            var selectedMember = _playerParty.Monsters[_currentMember];
            if (selectedMember.HP <= 0)
            {
                _partyScreen.SetMessageText("You cant choose a fainted monster");
                return;
            }
            if (selectedMember == _playerUnit.Monster)
            {
                _partyScreen.SetMessageText("cant switch with same monster");
                return;
            }
            _partyScreen.gameObject.SetActive(false);
            _state = BattleState.Busy;
            StartCoroutine(SwitchMonster(selectedMember));
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _partyScreen.gameObject.SetActive(false);
            PlayerAction();
        }


    }

    IEnumerator SwitchMonster(Monster newMonster)
    {
        if (_playerUnit.Monster.HP > 0)
        {
            yield return _dialogBox.TypeDialog($"Come back {_playerUnit.Monster.Base.Name}");
            _playerUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        _playerUnit.Setup(newMonster);
        _playerHud.SetData(newMonster);

        _dialogBox.SetMoveNames(newMonster.Moves);


        yield return _dialogBox.TypeDialog($@"Go {newMonster.Base.Name}");

        StartCoroutine(EnemyMove());
    }


}
