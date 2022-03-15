using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, Busy, PartyScreen, BattleOver }


public class BattleSystem : MonoBehaviour
{
    [SerializeField] List<BattleUnit> _playerUnits;
    [SerializeField] List<BattleUnit> _ennemyUnits;

    [SerializeField] BattleDialogBox _dialogBox;
    [SerializeField] PartyEnnemiScreen _partyEnnemiScreen;
    [SerializeField] PartyPlayerScreen _partyPlayerScreen;
    [SerializeField] Image _playerImage;
    [SerializeField] Image _ennemiImage;

    public event Action<bool> onBattleOver;

    private BattleState _state;

    private int _currentPlayer = 0;
    private int _currentEnnemy = 0;

    private int _currentAction;
    private int _currentMove;
    private int _currentMember = 0;
    private int _currentEnnemySelection = 0;

    private MonsterParty _playerParty;
    private MonsterParty _ennemiParty;
    private Monster _wildMonster;

    public bool isEnnemiBattle = false;

    private PlayerController _player;
    private EnnemiController _ennemi;
    public void StartBattle(MonsterParty playerParty, Monster wildMonster)
    {
        this._playerParty = playerParty;
        this._wildMonster = wildMonster;
        StartCoroutine(SetupBattle());
    }
    public void StartEnnemiBattle(MonsterParty playerParty, MonsterParty ennemiParty)
    {
        this._playerParty = playerParty;
        this._ennemiParty = ennemiParty;

        isEnnemiBattle = true;
        _player = playerParty.GetComponent<PlayerController>();
        _ennemi = ennemiParty.GetComponent<EnnemiController>();
        StartCoroutine(SetupBattle());
    }
    public IEnumerator SetupBattle()
    {
        foreach (var _playerUnit in _playerUnits)
        {
            _playerUnit.Clear();
        }
        foreach (var _ennemyUnit in _ennemyUnits)
        {
            _ennemyUnit.Clear();
        }



        if (!isEnnemiBattle)
        {

            //Wild ennemi
            /*_playerUnit.Setup(_playerParty.Monsters[_currentMember]);
            _playerUnit2.Setup2(_playerParty.Monsters[_currentMember + 1]);
            _playerUnit3.Setup2(_playerParty.Monsters[_currentMember + 2]);

            _ennemyUnit.Setup(_wildMonster);
            _dialogBox.SetMoveNames(_playerUnit.Monster.Moves);
            _playerUnit.Show();
            _playerUnit2.Show();
            _playerUnit3.Show();
            _ennemyUnit.Show();
            */
            yield return _dialogBox.TypeDialog($@"A wild {_ennemyUnits[0].Monster.Base.Name} appeared.");

        }
        else
        {
            for (int i = 0; i < _playerUnits.Count; i++)
            {
                _playerUnits[i].Setup(_playerParty.Monsters[_currentMember + i]);
            }
            for (int i = 0; i < _ennemyUnits.Count; i++)
            {
                if (i < _ennemiParty.Monsters.Count)
                {
                    _ennemyUnits[i].Setup(_ennemiParty.Monsters[_currentMember + i]);
                    _ennemyUnits[i].gameObject.SetActive(true);
                }
                else
                    _ennemyUnits[i].gameObject.SetActive(false);


            }

            foreach (var _playerUnit in _playerUnits)
            {
                _playerUnit.Show();
            }
            foreach (var _ennemyUnit in _ennemyUnits)
            {
                _ennemyUnit.Show();
            }
            //_dialogBox.SetMoveNames(_playerUnits[0].Monster.Moves);

        }
        ActionSelection();
    }

    void BattleOver(bool won)
    {
        _state = BattleState.BattleOver;
        onBattleOver(won);
    }

    private void ActionSelection()
    {
        _state = BattleState.ActionSelection;


        StartCoroutine(_dialogBox.TypeDialog("Choose an action"));
        _dialogBox.EnableActionSelector(true);
    }
    private void OpenPartyScreen()
    {
        _state = BattleState.PartyScreen;
        _partyEnnemiScreen.SetPartyData(_playerParty.Monsters);
        _partyEnnemiScreen.gameObject.SetActive(true);
    }
    private void MoveSelection()
    {
        _state = BattleState.MoveSelection;
        if (_playerUnits[_currentPlayer].Monster.HP > 0)
        {
            //StartCoroutine(_dialogBox.TypeDialog($@"It's {_playerUnits[_currentPlayer].Monster.Base.Name} turn"));
            _dialogBox.SetMoveNames(_playerUnits[_currentPlayer].Monster.Moves);
            _dialogBox.EnableMoveSelector(true);
            _dialogBox.EnableActionSelector(false);
            _dialogBox.EnableDialogText(false);
        }
        else
        {
            for (int i = 0; i < _playerParty.Monsters.Count - 1; i++)
            {
                if (_playerUnits[i].Monster.HP <= 0)
                    _currentPlayer++;
                else break;
            }
            MoveSelection();
        }
    }


    IEnumerator PlayerMove()
    {
        _state = BattleState.PerformMove;


        var move = _playerUnits[_currentPlayer].Monster.Moves[_currentMove];

        target = _ennemyUnits[Random.Range(0, _ennemiParty.Monsters.Count)];
        if (target.Monster.HP <= 0 && _ennemiParty.GetHealthyMonster() != null)
        {

            for (int i = _ennemiParty.Monsters.Count - 1; i >= 0; i--)
            {
                if (_ennemyUnits[i].Monster.HP > 0)
                {
                    target = _ennemyUnits[i];
                    break;
                }
            }
        }
        //HandleEnnemySelection();
        yield return RunMove(_playerUnits[_currentPlayer], target, move);


        if (_currentPlayer + 1 < _playerParty.Monsters.Count && _playerUnits[_currentPlayer + 1].Monster.HP > 0)
        {
            _currentPlayer++;
            MoveSelection();
        }
        else if (_currentPlayer + _playerParty.Monsters.Count - 1 < _playerParty.Monsters.Count && _playerUnits[_currentPlayer + _playerParty.Monsters.Count - 1].Monster.HP > 0)
        {
            _currentPlayer += _playerParty.Monsters.Count - 1;
            MoveSelection();
        }
        else
        {
            _currentEnnemy = 0;
            StartCoroutine(EnemyMove());
        }

    }
    BattleUnit target;
    IEnumerator EnemyMove()
    {
        _state = BattleState.PerformMove;
        for (int i = 0; i < _ennemiParty.Monsters.Count; i++)
        {
            if (_ennemyUnits[i].Monster.HP > 0)
            {
                var move = _ennemyUnits[i].Monster.GetRandomMove();
                target = _playerUnits[Random.Range(0, _playerParty.Monsters.Count)];
                if (target.Monster.HP <= 0 && _playerParty.GetHealthyMonster() != null)
                {
                    for (int y = _playerParty.Monsters.Count - 1; y >= 0; y--)
                    {
                        if (_playerUnits[y].Monster.HP > 0)
                        {
                            target = _playerUnits[y];
                            break;
                        }
                    }
                }
                yield return RunMove(_ennemyUnits[i], target, move);
            }
        }

        var playerParty = _playerParty.GetHealthyMonster();
        if (playerParty != null)
        {
            if (!isEnnemiBattle)
                ActionSelection();
            else
            {
                _currentPlayer = 0;
                MoveSelection();
            }
        }
    }

    IEnumerator RunMove(BattleUnit souceUnit, BattleUnit targetUnit, Move move)
    {
        move.PP--;
        yield return _dialogBox.TypeDialog($"{souceUnit.Monster.Base.Name} used {move.Base.Name}");

        souceUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        targetUnit.PlayHitAnimation();

        var damageDetails = targetUnit.Monster.TakeDamage(move, souceUnit.Monster);
        yield return targetUnit.Hud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return _dialogBox.TypeDialog($"{targetUnit.Monster.Base.Name} Fainted");
            targetUnit.PlayFaintAnimation();

            var _hudTarget = targetUnit.GetComponentInChildren<BattleHud>(true);
            _hudTarget.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);

            CheckForBattleOver(targetUnit);
        }
    }
    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextMonster = _playerParty.GetHealthyMonster();

            if (nextMonster == null)
                BattleOver(false);


        }
        else
        {
            if (!isEnnemiBattle)
            {
                BattleOver(true);
            }
            else
            {
                var nextEnnemi = _ennemiParty.GetHealthyMonster();
                if (nextEnnemi == null)
                    BattleOver(true);
                /*{
                    StartCoroutine(SandNextEnnemiMonster(nextEnnemi));
                }
                else
                {

                    BattleOver(true);
                }*/
            }
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
        if (_state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if (_state == BattleState.MoveSelection)
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
                MoveSelection();
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
                StartCoroutine(TryToEscape());
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

        _currentMove = Mathf.Clamp(_currentMove, 0, _playerUnits[_currentPlayer].Monster.Moves.Count - 1);

        _dialogBox.UpdateMoveSelection(_currentMove, _playerUnits[_currentPlayer].Monster.Moves[_currentMove]);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _dialogBox.EnableMoveSelector(false);
            _dialogBox.EnableDialogText(true);
            StartCoroutine(PlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _dialogBox.EnableMoveSelector(false);
            _dialogBox.EnableDialogText(true);
            ActionSelection();
        }

    }

    private void HandleEnnemySelection()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            _currentEnnemySelection++;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _currentEnnemySelection--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _currentEnnemySelection += 2;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            _currentEnnemySelection -= 2;
        }

        if (_currentEnnemySelection > 4)
            _currentEnnemySelection = 0;
        //_currentMove = Mathf.Clamp(_currentEnnemyMember, 0, _playerUnit.Monster.Moves.Count - 1);

        //_dialogBox.UpdateMoveSelection(_currentMove, _playerUnit.Monster.Moves[_currentMove]);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_currentAction == 0)
            {
                target = _ennemyUnits[0];
            }
            else if (_currentAction == 1)
            {
                target = _ennemyUnits[1];
            }
            else if (_currentAction == 2)
            {
                target = _ennemyUnits[2];
            }
            else if (_currentAction == 3)
            {
                target = _ennemyUnits[3];
            }
            else if (_currentAction == 4)
            {
                target = _ennemyUnits[4];
            }
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

        //_partyScreen.UpdateSelectedMember(_currentMember);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            var selectedMember = _playerParty.Monsters[_currentMember];
            if (selectedMember.HP <= 0)
            {
                _partyEnnemiScreen.SetMessageText("You cant choose a fainted monster");
                return;
            }
            if (selectedMember == _playerUnits[0].Monster)
            {
                _partyEnnemiScreen.SetMessageText("cant switch with same monster");
                return;
            }
            _partyEnnemiScreen.gameObject.SetActive(false);
            _state = BattleState.Busy;
            StartCoroutine(SwitchMonster(selectedMember));
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _partyEnnemiScreen.gameObject.SetActive(false);
            ActionSelection();
        }


    }

    IEnumerator SwitchMonster(Monster newMonster)
    {
        if (_playerUnits[0].Monster.HP > 0)
        {
            yield return _dialogBox.TypeDialog($"Come back {_playerUnits[0].Monster.Base.Name}");
            _playerUnits[0].PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        _playerUnits[0].Setup(newMonster);

        _dialogBox.SetMoveNames(newMonster.Moves);


        yield return _dialogBox.TypeDialog($@"Go {newMonster.Base.Name}");
        //_playerUnit.Show();

        StartCoroutine(EnemyMove());
        _state = BattleState.ActionSelection;
    }

    IEnumerator SandNextEnnemiMonster(Monster nextMonster)
    {
        _state = BattleState.Busy;
        _ennemyUnits[0].Setup(nextMonster);
        yield return _dialogBox.TypeDialog($"{_ennemi.Name} sand out {nextMonster.Base.Name}!");
        _state = BattleState.ActionSelection;
    }

    IEnumerator TryToEscape()
    {
        _state = BattleState.Busy;

        if (isEnnemiBattle)
        {
            yield return _dialogBox.TypeDialog("You can run from ennemi battle");
            _state = BattleState.ActionSelection;
            yield break;
        }
        else
        {
            if (UnityEngine.Random.Range(1, 101) <= 50)
            {
                yield return _dialogBox.TypeDialog("Ran away safely!");
                BattleOver(true);
            }
            else
            {
                yield return _dialogBox.TypeDialog("Can't escape");
                StartCoroutine(EnemyMove());
            }
        }
    }
}
