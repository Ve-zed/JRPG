using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, Busy, PartyScreen, BattleOver }


public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit _playerUnit;
    [SerializeField] BattleUnit _playerUnit2;
    [SerializeField] BattleUnit _playerUnit3;
    [SerializeField] BattleUnit _ennemyUnit;
    [SerializeField] BattleDialogBox _dialogBox;
    [SerializeField] PartyScreen _partyScreen;
    [SerializeField] Image _playerImage;
    [SerializeField] Image _ennemiImage;


    public event Action<bool> onBattleOver;

    private BattleState _state;

    private int _currentAction;
    private int _currentMove;
    private int _currentMember = 0;

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
        _playerUnit.Clear();
        _playerUnit2.Clear();
        _playerUnit3.Clear();
        _ennemyUnit.Clear();
        if (!isEnnemiBattle)
        {
            //Wild ennemi
            _playerUnit.Setup(_playerParty.Monsters[_currentMember]);
            _playerUnit2.Setup2(_playerParty.Monsters[_currentMember + 1]);
            _playerUnit3.Setup2(_playerParty.Monsters[_currentMember + 2]);
            _ennemyUnit.Setup(_wildMonster);
            _dialogBox.SetMoveNames(_playerUnit.Monster.Moves);
            _playerUnit.Show();
            _playerUnit2.Show();
            _playerUnit3.Show();
            _ennemyUnit.Show();
            yield return _dialogBox.TypeDialog($@"A wild {_ennemyUnit.Monster.Base.Name} appeared.");

        }
        else
        {
            //ennemi
            //_playerUnit.gameObject.SetActive(false);
            //_ennemyUnit.gameObject.SetActive(false);
            //_playerImage.gameObject.SetActive(true);
            //_ennemiImage.gameObject.SetActive(true);
            //_playerImage.sprite = _player.Sprite;
            //_ennemiImage.sprite = _ennemi.Sprite;
            //yield return _dialogBox.TypeDialog($"{_ennemi.Name} want battle");

            //_ennemiImage.gameObject.SetActive(false);
            //_ennemyUnit.gameObject.SetActive(true);
            var ennemiMonster = _ennemiParty.GetHealthyMonster();
            _ennemyUnit.Setup(ennemiMonster);
            //yield return _dialogBox.TypeDialog($@"{_ennemi.Name} sand out {ennemiMonster.Base.Name}.");

            //_playerImage.gameObject.SetActive(false);
            //_playerUnit.gameObject.SetActive(true);
            /*var playerMonster = _playerParty.GetHealthyMonster();
            _playerUnit.Setup(playerMonster);
            */
            _playerUnit.Setup(_playerParty.Monsters[_currentMember]);
            _playerUnit2.Setup2(_playerParty.Monsters[_currentMember + 1]);
            _playerUnit3.Setup2(_playerParty.Monsters[_currentMember + 2]); 
            //yield return _dialogBox.TypeDialog($@"Go {playerMonster.Base.Name}.");

            _playerUnit.Show();
            _playerUnit2.Show();
            _playerUnit3.Show();
            _ennemyUnit.Show();
            _dialogBox.SetMoveNames(_playerUnit.Monster.Moves);

        }
        _partyScreen.Init();
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
        _partyScreen.SetPartyData(_playerParty.Monsters);
        _partyScreen.gameObject.SetActive(true);
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
        var move = _playerUnit.Monster.Moves[_currentMove];
        yield return RunMove(_playerUnit, _ennemyUnit, move);

        if (_state == BattleState.PerformMove)
        {
            if (_playerUnit2.Monster.HP > 0)
                StartCoroutine(PlayerMove2());
            else if (_playerUnit3.Monster.HP > 0)
                StartCoroutine(PlayerMove3());
            else
                StartCoroutine(EnemyMove());
        }

    }
    IEnumerator PlayerMove2()
    {
        _state = BattleState.PerformMove;
        if (_playerUnit2.Monster.HP > 0)
        {
            var move = _playerUnit2.Monster.GetRandomMove();
            yield return RunMove(_playerUnit2, _ennemyUnit, move);
        }
        if (_state == BattleState.PerformMove)
        {
            if (_playerUnit3.Monster.HP > 0)
                StartCoroutine(PlayerMove3());
            else
                StartCoroutine(EnemyMove());

        }

    }
    IEnumerator PlayerMove3()
    {
        _state = BattleState.PerformMove;
        if (_playerUnit3.Monster.HP > 0)
        {
            var move = _playerUnit3.Monster.GetRandomMove();
            yield return RunMove(_playerUnit3, _ennemyUnit, move);
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
        target = _playerUnit;
        var move = _ennemyUnit.Monster.GetRandomMove();
        int random = _ennemyUnit.Monster.GetRandomEnnemi();
        if (random == 0)
        {
            if (_playerUnit.Monster.HP > 0)
                target = _playerUnit;
            else if (_playerUnit2.Monster.HP > 0)
                target = _playerUnit2;
            else
                target = _playerUnit3;
        }

        else if (random == 1)
        {
            if (_playerUnit2.Monster.HP > 0)
                target = _playerUnit2;
            else if (_playerUnit3.Monster.HP > 0)
                target = _playerUnit3;
            else
                target = _playerUnit;

        }
        else if (random == 2)
        {
            if (_playerUnit3.Monster.HP > 0)
                target = _playerUnit3;
            else if (_playerUnit2.Monster.HP > 0)
                target = _playerUnit2;
            else
                target = _playerUnit;

        }
        yield return RunMove(_ennemyUnit, target, move);

        if (_state == BattleState.PerformMove)
        {
            if (_playerUnit.Monster.HP > 0)
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
            targetUnit.Clear();
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
                if (nextEnnemi != null)
                {
                    StartCoroutine(SandNextEnnemiMonster(nextEnnemi));
                }
                else
                {

                    BattleOver(true);
                }
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

        _currentMove = Mathf.Clamp(_currentMove, 0, _playerUnit.Monster.Moves.Count - 1);

        _dialogBox.UpdateMoveSelection(_currentMove, _playerUnit.Monster.Moves[_currentMove]);

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
            ActionSelection();
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

        _dialogBox.SetMoveNames(newMonster.Moves);


        yield return _dialogBox.TypeDialog($@"Go {newMonster.Base.Name}");
        _playerUnit.Show();

        StartCoroutine(EnemyMove());
        _state = BattleState.ActionSelection;
    }

    IEnumerator SandNextEnnemiMonster(Monster nextMonster)
    {
        _state = BattleState.Busy;
        _ennemyUnit.Setup(nextMonster);
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
