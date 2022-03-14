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

    private int _currentAction;
    private int _currentMove;
    private int _currentMember = 0;
    private int _currentEnnemyMember = 0;

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
                    _ennemyUnits[i].Setup3(_ennemiParty.Monsters[_currentMember + i]);
                }
                else
                    _ennemyUnits[i].gameObject.SetActive(false);


            }

            //_ennemiParty.Monsters. y a un truc a faire avec ça
            /*for (int i = 0; i < _memberSlots.Length; i++)
            {
                if (i < monsters.Count)
                {
                    _memberSlots[i].SetData(monsters[i]);
                }
                else
                    _memberSlots[i].gameObject.SetActive(false);
                //  _dialogBox.SetMoveNames(monsters[0].Moves);

            }*/
            foreach (var _playerUnit in _playerUnits)
            {
                _playerUnit.Show();
            }
            foreach (var _ennemyUnit in _ennemyUnits)
            {
                _ennemyUnit.Show();
            }
            _dialogBox.SetMoveNames(_playerUnits[0].Monster.Moves);

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

        _dialogBox.EnableActionSelector(false);
        _dialogBox.EnableDialogText(false);
        _dialogBox.EnableMoveSelector(true);
    }


    IEnumerator PlayerMove()
    {
        _state = BattleState.PerformMove;
        var move = _playerUnits[0].Monster.Moves[_currentMove];

        //int random = _playerUnits[0].Monster.GetRandomEnnemi();
        //target = _ennemyUnits[random];
        target = _ennemyUnits[Random.Range(0, _ennemiParty.Monsters.Count)];

        //HandleEnnemySelection();
        yield return RunMove(_playerUnits[0], target, move);

        if (_state == BattleState.PerformMove)
        {
            if (_playerUnits[1].Monster.HP > 0)
                StartCoroutine(PlayerMove2());
            else if (_playerUnits[2].Monster.HP > 0)
                StartCoroutine(PlayerMove3());
            else
                StartCoroutine(EnemyMove());
        }

    }
    IEnumerator PlayerMove2()
    {
        _state = BattleState.PerformMove;
        if (_playerUnits[1].Monster.HP > 0)
        {
            var move = _playerUnits[1].Monster.GetRandomMove();
            //int random = _playerUnits[0].Monster.GetRandomEnnemi();
            target = _ennemyUnits[Random.Range(0, _ennemiParty.Monsters.Count)];

            yield return RunMove(_playerUnits[1], target, move);
        }
        if (_state == BattleState.PerformMove)
        {
            if (_playerUnits[2].Monster.HP > 0)
                StartCoroutine(PlayerMove3());
            else
                StartCoroutine(EnemyMove());

        }

    }
    IEnumerator PlayerMove3()
    {
        _state = BattleState.PerformMove;
        if (_playerUnits[2].Monster.HP > 0)
        {
            var move = _playerUnits[2].Monster.GetRandomMove();
            //int random = _playerUnits[0].Monster.GetRandomEnnemi();
            //target = _ennemyUnits[random];
            target = _ennemyUnits[Random.Range(0, _ennemiParty.Monsters.Count)];

            yield return RunMove(_playerUnits[2], target, move);
        }
        if (_state == BattleState.PerformMove)
        {
            StartCoroutine(EnemyMove());

        }

    }
    BattleUnit target;
    IEnumerator EnemyMove()
    {
        _state = BattleState.PerformMove;
        int random = _playerUnits[0].Monster.GetRandomEnnemi();
        target = _ennemyUnits[random];
        var move = _ennemyUnits[0].Monster.GetRandomMove();
        if (random == 0)
        {
            if (_playerUnits[0].Monster.HP > 0)
                target = _playerUnits[0];
            else if (_playerUnits[1].Monster.HP > 0)
                target = _playerUnits[1];
            else
                target = _playerUnits[2];
        }

        else if (random == 1)
        {
            if (_playerUnits[1].Monster.HP > 0)
                target = _playerUnits[1];
            else if (_playerUnits[2].Monster.HP > 0)
                target = _playerUnits[2];
            else
                target = _playerUnits[0];

        }
        else if (random == 2)
        {
            if (_playerUnits[2].Monster.HP > 0)
                target = _playerUnits[2];
            else if (_playerUnits[1].Monster.HP > 0)
                target = _playerUnits[1];
            else
                target = _playerUnits[0];

        }
        yield return RunMove(_ennemyUnits[0], target, move);

        if (_state == BattleState.PerformMove)
        {
            if (_playerUnits[0].Monster.HP > 0)
                ActionSelection();
            else
                StartCoroutine(PlayerMove2());
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
            //targetUnit.Clear();
            yield return new WaitForSeconds(2f);

            CheckForBattleOver(targetUnit);
        }
    }
    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextMonster = _playerParty.GetHealthyMonster();
            /*if (nextMonster != null)
                OpenPartyScreen();
            else
                BattleOver(false);
        */
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

        _currentMove = Mathf.Clamp(_currentMove, 0, _playerUnits[0].Monster.Moves.Count - 1);

        _dialogBox.UpdateMoveSelection(_currentMove, _playerUnits[0].Monster.Moves[_currentMove]);

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
            _currentEnnemyMember++;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _currentEnnemyMember--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _currentEnnemyMember += 2;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            _currentEnnemyMember -= 2;
        }

        if (_currentEnnemyMember > 4)
            _currentEnnemyMember = 0;
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
