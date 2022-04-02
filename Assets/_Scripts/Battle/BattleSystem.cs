using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MoveBase;
using Random = UnityEngine.Random;
using DG.Tweening;

public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, EnnemiSelection, Busy, PartyScreen, BattleOver }


public class BattleSystem : MonoBehaviour
{
    public bool isEnnemiBattle = false;

    public List<BattleUnit> _playerUnits;
    public List<BattleUnit> _playerUnitsDead;
    [SerializeField] List<BattleUnit> _ennemyUnits;
    [SerializeField] GameController _gameController;
    //[SerializeField] List<MonsterParty> _playerListParty;

    [SerializeField] BattleDialogBox _dialogBox;
    [SerializeField] PrecisionBar _precision;

    public BattleUnit _playerSelectedUnit;
    public BattleUnit _targetSelectedUnit;

    [SerializeField] GameObject _pouvoirBarre;

    public event Action<bool> onBattleOver;

    public bool canSelected = true;
    public bool canSelectedEnnemi = false;
    public bool powerUsed = false;

    private BattleState _state;
    private int _currentAction;
    private int _currentMove;
    private int _currentMember = 0;
    private MonsterParty _playerParty;
    private MonsterParty _ennemiParty;

    private PlayerController _player;
    private EnnemiController _ennemi;
    private bool _isEnnemiTurn = false;
    BattleUnit target;
    Move currentMove;

    public void StartEnnemiBattle(MonsterParty playerParty, MonsterParty ennemiParty)
    {
        _state = BattleState.Start;
        this._playerParty = playerParty;
        this._ennemiParty = ennemiParty;

        isEnnemiBattle = true;
        _player = playerParty.GetComponent<PlayerController>();
        _ennemi = ennemiParty.GetComponent<EnnemiController>();
        StartCoroutine(SetupBattle());
    }
    private void ResetBattleState()
    {
        canSelected = true;
        canSelectedEnnemi = false;
        _playerSelectedUnit = null;
        _targetSelectedUnit = null;
        foreach (var item in _playerUnits)
        {
            item.PlayNormalAnimation();
            item.isPowerUsed = false;
            item.isSelected = false;
            item.isAttacking = false;
            item.boxCollider.enabled = true;
            item._image.material = item.originalMaterial;
        }
        foreach (var item in _ennemyUnits)
        {
            item.PlayNormalAnimation();
            item.isPowerUsed = false;
            item.isAttacking = false;
            item.boxCollider.enabled = true;
        }

        _playerUnitsDead.RemoveRange(0, _playerUnitsDead.Count);
    }

    public IEnumerator SetupBattle()
    {
        AudioManager.Instance.audioSourceMusic.Stop();
        StartCoroutine(AudioManager.Instance.IEPlayMusicSound("snd_music_fight"));
        if (powerUsed)
        {
            PrecisionBar.Instance.ResetFillAmount();
            powerUsed = false;
        }
        ResetBattleState();
        foreach (var _playerUnit in _playerUnits)
        {
            _playerUnit.Clear();
            _playerUnit.isAttacking = false;
        }
        foreach (var _ennemyUnit in _ennemyUnits)
        {
            if (!_ennemi.isVirus)
                _ennemyUnit.isVirus = false;
            else
                _ennemyUnit.isVirus = true;
            _ennemyUnit.Clear();
        }



        for (int i = 0; i < _playerUnits.Count; i++)
        {
            _playerUnits[i].UpdateLevel(_playerParty.Monsters[_currentMember + i]);
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
        foreach (var monster in _playerParty.Monsters)
        {
            monster.Init();
        }
        yield return new WaitForSeconds(0.5f);
    }

    void BattleOver(bool won)
    {
        _state = BattleState.BattleOver;
        _playerParty.Monsters.ForEach(p => p.OnBattleOver());
        if (won)
        {

            _gameController.mHR += _ennemi.moneyAfterBattle;
            AudioManager.Instance.PlaySFXSound("snd_victory");
            FadeBattle.Instance.imageFadeBattle.DOFade(0, 1);

        }
        //FadeBattle.Instance.imageFadeInBattle.DOFade(1, 0.5f);
        AudioManager.Instance.audioSourceMusic.Stop();
        StartCoroutine(AudioManager.Instance.IEPlayMusicSound("snd_music_exploration"));
        StartCoroutine(AudioManager.Instance.IEPlayMusicSound("snd_ambiance_exploration"));
        onBattleOver(won);
    }

    public void MoveSelection()
    {
        _state = BattleState.MoveSelection;
        _dialogBox.SetMoveNames(_playerSelectedUnit.Monster.Moves);
    }
    public void EnnemiSelection()
    {
        _state = BattleState.EnnemiSelection;
        foreach (var item in _playerUnits)
        {
            if (item.isSelected)
            {
                canSelected = false;
                break;
            }
            else
                canSelected = true;
        }
        canSelectedEnnemi = true;
    }

    public IEnumerator PlayerMove()
    {
        _state = BattleState.PerformMove;

        foreach (var item in _playerUnits)
        {
            if (item.isSelected)
            {
                canSelected = false;
                break;
            }
            else
                canSelected = true;
        }

        var move = _playerSelectedUnit.Monster.Moves[_currentMove];
        if (move.Base.Target == MoveTarget.AllEnnemi)
            yield return RunMoveAttackAll(_playerSelectedUnit, _ennemyUnits, move);
        else if (move.Base.Target == MoveTarget.AllPlayer)
            yield return RunMoveForAllPlayer(_playerSelectedUnit, move);
        else
        {
            if (powerUsed)
            {
                if (PrecisionBar.Instance.barreSplit == 1)
                    for (int i = 0; i < 3; i++)
                    {
                        StartCoroutine(RunMove(_playerSelectedUnit, _targetSelectedUnit, move));
                        yield return new WaitForSeconds(0.2f);
                    }
                else if (PrecisionBar.Instance.barreSplit == 2)
                    for (int i = 0; i < 5; i++)
                    {
                        StartCoroutine(RunMove(_playerSelectedUnit, _targetSelectedUnit, move));
                        yield return new WaitForSeconds(0.2f);
                    }
                else if (PrecisionBar.Instance.barreSplit == 3)
                    for (int i = 0; i < 7; i++)
                    {
                        StartCoroutine(RunMove(_playerSelectedUnit, _targetSelectedUnit, move));
                        yield return new WaitForSeconds(0.2f);
                    }
            }
            else
                StartCoroutine(RunMove(_playerSelectedUnit, _targetSelectedUnit, move));
        }

        _playerSelectedUnit._image.material = _playerSelectedUnit.originalMaterial;

        foreach (var player in _playerUnits)
        {
            if (player.isSelected)
            {
                player.isSelected = false;
                break;
            }
        }

        _precision.ResetFillAmount();
    }
    IEnumerator EnnemiTurn()
    {
        Debug.Log("enemi");
        int playerCount = 0;
        bool enemiTurn = false;
        for (int i = 0; i < _playerUnits.Count; i++)
        {

            if (_playerUnits[i].isAttacking)
                playerCount++;

            if (playerCount >= 3 - _playerUnitsDead.Count)
                enemiTurn = true;
        }
        if (playerCount >= 3 - _playerUnitsDead.Count && !_isEnnemiTurn && enemiTurn)
        {
            Debug.Log("turn enemi");
            _isEnnemiTurn = true;
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyMove());
            //break;
        }

    }
    IEnumerator EnemyMove()
    {
        _state = BattleState.PerformMove;
        for (int i = 0; i < _ennemiParty.Monsters.Count; i++)
        {
            if (_ennemyUnits[i].Monster.HP > 0)
            {
                var move = _ennemyUnits[i].Monster.GetRandomMove();

                bool provoqued = _ennemyUnits[i].Monster.OnFocusTarget();
                if (provoqued)
                {
                    if (_playerUnits[2].Monster.HP > 0)
                        target = _playerUnits[2];
                    /*else
                    {
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
                    }*/
                }
                else
                {
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
                    // target = _playerUnits[2];
                }
                if (move.Base.Target == MoveTarget.AllEnnemi)
                {
                    yield return RunMoveAttackAll(_ennemyUnits[i], _playerUnits, move);
                }
                else if (move.Base.Target == MoveTarget.Self)
                    yield return EnnemiHealMove(_ennemyUnits[i]);
                else
                    yield return RunMove(_ennemyUnits[i], target, move);
            }
        }

        var playerParty = _playerParty.GetHealthyMonster();
        yield return new WaitForSeconds(1f);
        if (playerParty != null)
        {
            foreach (var player in _playerUnits)
            {
                player.isAttacking = false;
                if (player.Monster.HP > 0)
                    player.PlayNormalAnimation();
            }
            _isEnnemiTurn = false;
        }
    }
    IEnumerator EnnemiHealMove(BattleUnit sourceUnit)
    {
        bool canRunMove = sourceUnit.Monster.OnBeforeMove();
        if (!canRunMove)
        {
            yield break;
        }
        if (sourceUnit.Monster.HP > 0)
        {
            sourceUnit.Monster.HP += sourceUnit.Monster.MaxHp / 5;
            if (sourceUnit.Monster.HP > sourceUnit.Monster.MaxHp)
                sourceUnit.Monster.HP = sourceUnit.Monster.MaxHp;

            sourceUnit.PlayHealAnimation();
            sourceUnit.Monster.HpChanged = true;
            StartCoroutine(sourceUnit.Hud.UpdateHP());
        }
        yield return new WaitForSeconds(1f);
    }
    IEnumerator RunMoveSelf(BattleUnit sourceUnit, Move move)
    {

        AudioManager.Instance.PlaySFXSound(move.Base.Sound);


        var effects = move.Base.Effects;

        sourceUnit.Monster.ApplyBoosts(effects.Boosts);
        sourceUnit.Monster.SetStatus(effects.Status);
        StartCoroutine(sourceUnit.PlayBoostAnimation());
        yield return new WaitForSeconds(1f);
        sourceUnit.PlayFadeAnimation();

        sourceUnit.Monster.OnAfterTurn(sourceUnit);
        yield return sourceUnit.Hud.UpdateHP();


        if (sourceUnit.Monster.HP <= 0)
        {

            if (sourceUnit.isPlayerUnit)
            {
                _playerUnitsDead.Add(_playerSelectedUnit);
            }

            yield return _dialogBox.TypeDialog($"{sourceUnit.Monster.Base.Name} Fainted");
            sourceUnit.PlayFaintAnimation();

            var _hudTarget = sourceUnit.GetComponentInChildren<BattleHud>(true);
            _hudTarget.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);

            CheckForBattleOver(sourceUnit);
        }
        if (sourceUnit.IsPlayerUnit)
            yield return EnnemiTurn();
        canSelected = true;
        sourceUnit._image.material = sourceUnit.originalMaterial;
    }

    IEnumerator RunMoveForAllPlayer(BattleUnit sourceUnit, Move move)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < _playerParty.Monsters.Count; i++)
        {
            var player = _playerUnits[i];
            AudioManager.Instance.PlaySFXSound(move.Base.Sound);
            if (move.Base.Category == MoveCategory.Status)
            {
                StartCoroutine(RunMoveEffects(move, sourceUnit.Monster, _playerUnits[i].Monster, sourceUnit));
                if (player.Monster.HP > 0)
                    StartCoroutine(_playerUnits[i].PlayBoostAnimation());
            }
            else
            {
                if (player.Monster.HP > 0)
                {
                    player.Monster.HP += player.Monster.MaxHp / 5;
                    if (player.Monster.HP > player.Monster.MaxHp)
                        player.Monster.HP = player.Monster.MaxHp;
                    player.Monster.HpChanged = true;

                    StartCoroutine(player.Hud.UpdateHP());
                    if (player.Monster.HP > 0)
                        StartCoroutine(player.PlayHealAnimation());

                }
            }
        }
        yield return new WaitForSeconds(1.5f);

        sourceUnit.Monster.OnAfterTurn(sourceUnit);
        yield return sourceUnit.Hud.UpdateHP();
        if (sourceUnit.IsPlayerUnit)
            sourceUnit.PlayFadeAnimation();
        if (sourceUnit.Monster.HP <= 0)
        {
            if (sourceUnit.isPlayerUnit)
            {

                _playerUnitsDead.Add(_playerSelectedUnit);
            }
            yield return _dialogBox.TypeDialog($"{sourceUnit.Monster.Base.Name} Fainted");
            sourceUnit.PlayFaintAnimation();



            var _hudTarget = sourceUnit.GetComponentInChildren<BattleHud>(true);
            _hudTarget.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);

            CheckForBattleOver(sourceUnit);
        }
        if (sourceUnit.IsPlayerUnit)
            yield return EnnemiTurn();
        canSelected = true;
        _playerSelectedUnit._image.material = _playerSelectedUnit.originalMaterial;

    }

    /*IEnumerator AttacksEnemies( List<Monster> monsters )
    {
       
        yield return AttacksEnemies(_ennemiParty.Monsters);
        yield return AttacksEnemies(_playerParty.Monsters);
    }*/



    IEnumerator RunMoveAttackAll(BattleUnit sourceUnit, List<BattleUnit> targetUnits, Move move)
    {

        bool canRunMove = sourceUnit.Monster.OnBeforeMove();
        if (!canRunMove)
        {
            yield break;
        }

        sourceUnit.PlayAttackAnimation();
        AudioManager.Instance.PlaySFXSound(move.Base.Sound);
        yield return new WaitForSeconds(1f);
        if (sourceUnit.isPlayerUnit)
        {
            for (int i = 0; i < _ennemiParty.Monsters.Count; i++)
            {
                if (targetUnits[i].Monster.HP > 0)
                {
                    StartCoroutine(targetUnits[i].PlayHitAnimation());
                    if (targetUnits[i].isVirus)
                        AudioManager.Instance.PlaySFXSound("snd_virus_hurt");
                    else
                        AudioManager.Instance.PlaySFXSound("snd_player_hurt");
                    if (powerUsed && move.Base.Effects.Status == ConditionID.none)
                    {
                        targetUnits[i].Monster.TakeDamage(move, sourceUnit.Monster, PrecisionBar.Instance.barreSplit);

                        StartCoroutine(targetUnits[i].Hud.UpdateHP());
                    }
                    else
                    {
                        if (move.Base.Category == MoveCategory.Status)
                        {
                            StartCoroutine(RunMoveEffects(move, sourceUnit.Monster, targetUnits[i].Monster, sourceUnit));
                        }

                        else
                        {
                            if (_targetSelectedUnit == targetUnits[i])
                            {
                                sourceUnit.Monster.ApplyBoosts(move.Base.Effects.Boosts);
                                targetUnits[i].Monster.TakeDamage(move, sourceUnit.Monster, 1);
                                StartCoroutine(targetUnits[i].Hud.UpdateHP());
                                sourceUnit.Monster.ResetStatBoost();

                            }
                            else
                            {
                                targetUnits[i].Monster.TakeDamage(move, sourceUnit.Monster, 1);
                                StartCoroutine(targetUnits[i].Hud.UpdateHP());
                            }
                        }
                    }
                    sourceUnit.PlayFadeAnimation();
                }
            }
            if (powerUsed && move.Base.Effects.Status == ConditionID.none)
            {
                if (sourceUnit.Monster.HP > 0)
                {
                    if (PrecisionBar.Instance.barreSplit == 1)
                        sourceUnit.Monster.HP -= 35; //valeur a mettre pour les gd

                    else if (PrecisionBar.Instance.barreSplit == 2)
                        sourceUnit.Monster.HP -= 50; //valeur a mettre pour les gd
                    else if (PrecisionBar.Instance.barreSplit == 3)
                        sourceUnit.Monster.HP -= 65; //valeur a mettre pour les gd
                    sourceUnit.Monster.HpChanged = true;
                    StartCoroutine(sourceUnit.Hud.UpdateHP());
                }
            }


            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < _ennemiParty.Monsters.Count; i++)
            {
                if (targetUnits[i].Monster.HP <= 0)
                {
                    for (int y = 0; y < _playerUnits.Count; y++)
                    {
                        //.WinXP(_playerUnits, targetUnit);
                        _playerSelectedUnit.WinXP(_playerUnits[y], targetUnits[i]);
                    }

                    if (targetUnits[i].isPlayerUnit)
                        _playerUnitsDead.Add(_playerSelectedUnit);

                    targetUnits[i].PlayFaintAnimation();
                    var _hudTarget = targetUnits[i].GetComponentInChildren<BattleHud>(true);
                    _hudTarget.gameObject.SetActive(false);
                }

                if (i == _ennemiParty.Monsters.Count - 1)
                {
                    yield return new WaitForSeconds(1f);
                    CheckForBattleOver(targetUnits[i]);
                }

            }
            _pouvoirBarre.SetActive(false);
            _precision.ResetFillAmount();
            _playerSelectedUnit._image.material = _playerSelectedUnit.originalMaterial;
            canSelected = true;
            powerUsed = false;
        }
        else
        {
            for (int i = 0; i < _playerParty.Monsters.Count; i++)
            {
                if (targetUnits[i].Monster.HP > 0)
                {
                    StartCoroutine(targetUnits[i].PlayHitAnimation());

                    if (move.Base.Category == MoveCategory.Status)
                    {
                        StartCoroutine(RunMoveEffects(move, sourceUnit.Monster, targetUnits[i].Monster, sourceUnit));
                    }

                    else
                    {
                        targetUnits[i].Monster.TakeDamage(move, sourceUnit.Monster, 1);
                        StartCoroutine(targetUnits[i].Hud.UpdateHP());
                    }
                }
            }

            yield return new WaitForSeconds(1.5f);

            for (int i = 0; i < _playerParty.Monsters.Count; i++)
            {
                if (targetUnits[i].Monster.HP <= 0)
                {
                    targetUnits[i].PlayFaintAnimation();

                    var _hudTarget = targetUnits[i].GetComponentInChildren<BattleHud>(true);
                    _hudTarget.gameObject.SetActive(false);
                }

                if (i == _playerParty.Monsters.Count - 1)
                {
                    yield return new WaitForSeconds(1f);
                    CheckForBattleOver(targetUnits[i]);
                }
            }
        }

        sourceUnit.Monster.OnAfterTurn(sourceUnit);

        if (sourceUnit.Monster.HP <= 0)
        {

            if (sourceUnit.isPlayerUnit)
                _playerUnitsDead.Add(_playerSelectedUnit);



            sourceUnit.PlayFaintAnimation();

            var _hudTarget = sourceUnit.GetComponentInChildren<BattleHud>(true);
            _hudTarget.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);

            CheckForBattleOver(sourceUnit);
        }



        var nextEnnemi = _ennemiParty.GetHealthyMonster();

        if (nextEnnemi != null)
        {
            StartCoroutine(EnnemiTurn());
        }
        //      yield return EnnemiTurn();


    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {


        bool canRunMove = sourceUnit.Monster.OnBeforeMove();
        if (!canRunMove)
        {
            yield break;
        }

        sourceUnit.PlayAttackAnimation();
        AudioManager.Instance.PlaySFXSound(move.Base.Sound);

        yield return new WaitForSeconds(1f);

        _pouvoirBarre.SetActive(false);

        StartCoroutine(targetUnit.PlayHitAnimation());

        if (sourceUnit.IsPlayerUnit)
            sourceUnit.PlayFadeAnimation();
        yield return new WaitForSeconds(1f);

        if (targetUnit.isVirus)
            AudioManager.Instance.PlaySFXSound("snd_virus_hurt");
        else
            AudioManager.Instance.PlaySFXSound("snd_player_hurt");




        if (move.Base.Category == MoveCategory.Special)
        {
            //Damage
            var damageDetails = targetUnit.Monster.TakeDamage(move, sourceUnit.Monster, 1);
            yield return targetUnit.Hud.UpdateHP();
            yield return ShowDamageDetails(damageDetails);
            yield return new WaitForSeconds(1f);

            //Heal
            if (sourceUnit.Monster.HP > 0 && sourceUnit.Monster.Damage > 0)
            {
                sourceUnit.Monster.HP += sourceUnit.Monster.Damage / 2;
                if (sourceUnit.Monster.HP > sourceUnit.Monster.MaxHp)
                    sourceUnit.Monster.HP = sourceUnit.Monster.MaxHp;
                StartCoroutine(sourceUnit.PlayHealAnimation());
                sourceUnit.Monster.HpChanged = true;
                StartCoroutine(sourceUnit.Hud.UpdateHP());
            }
        }
        else
        {
            if (move.Base.Category == MoveCategory.Status)
            {
                yield return RunMoveEffects(move, sourceUnit.Monster, targetUnit.Monster, sourceUnit);
            }
            else
            {
                var damageDetails = targetUnit.Monster.TakeDamage(move, sourceUnit.Monster, 1);
                yield return targetUnit.Hud.UpdateHP();
                yield return ShowDamageDetails(damageDetails);
            }
        }

        if (targetUnit.Monster.HP <= 0)
        {
            if (!targetUnit.isPlayerUnit)
            {
                for (int i = 0; i < _playerUnits.Count; i++)
                {
                    //.WinXP(_playerUnits, targetUnit);
                    _playerSelectedUnit.WinXP(_playerUnits[i], targetUnit);
                }

            }
            else
            {
                _playerUnitsDead.Add(_playerSelectedUnit);

            }

            yield return _dialogBox.TypeDialog($"{targetUnit.Monster.Base.Name} Fainted");
            targetUnit.PlayFaintAnimation();

            var _hudTarget = targetUnit.GetComponentInChildren<BattleHud>(true);
            _hudTarget.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);

            CheckForBattleOver(targetUnit);
        }
        sourceUnit.Monster.OnAfterTurn(sourceUnit);
        yield return sourceUnit.Hud.UpdateHP();
        if (sourceUnit.Monster.HP <= 0)
        {
            if (sourceUnit.isPlayerUnit)
            {
                _playerUnitsDead.Add(_playerSelectedUnit);
            }

            yield return _dialogBox.TypeDialog($"{sourceUnit.Monster.Base.Name} Fainted");
            sourceUnit.PlayFaintAnimation();

            var _hudTarget = sourceUnit.GetComponentInChildren<BattleHud>(true);
            _hudTarget.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);

            CheckForBattleOver(sourceUnit);
        }
        canSelected = true;
        powerUsed = false;

        if (sourceUnit.IsPlayerUnit)
            yield return EnnemiTurn();
    }

    IEnumerator RunMoveEffects(Move move, Monster source, Monster target, BattleUnit sourceUnit = null)
    {
        var effects = move.Base.Effects;

        //Boost
        if (effects.Boosts != null)
        {
            if (move.Base.Target == MoveTarget.Self)
            {
                source.ApplyBoosts(effects.Boosts);
                StartCoroutine(sourceUnit.PlayBoostAnimation());
            }
            else
                target.ApplyBoosts(effects.Boosts);
        }
        //Status
        if (effects.Status != ConditionID.none)
        {
            if (move.Base.Target == MoveTarget.Self)
                source.SetStatus(effects.Status);
            else
                target.SetStatus(effects.Status);
        }

        yield return new WaitForSeconds(0.1f);
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

    private void Update()
    {

        if (_state == BattleState.EnnemiSelection && Input.GetKeyDown(KeyCode.Escape))
        {
            _playerSelectedUnit.isAttacking = false;
            foreach (var item in _playerUnits)
            {
                if (item.isSelected)
                    canSelected = true;
            }
            canSelectedEnnemi = false;
            powerUsed = false;
            _playerSelectedUnit.isPowerUsed = false;
        }


    }
    public void onClickMove(int move)
    {
        _state = BattleState.MoveSelection;
        _playerSelectedUnit.isAttacking = true;
        _dialogBox.EnableMoveSelector(false);
        _currentMove = move;
        var curmove = _playerSelectedUnit.Monster.Moves[_currentMove];
        if (move >= 4 && _playerSelectedUnit.isPowerUsed)
        {
            _playerSelectedUnit.isAttacking = false;
        }
        else if (curmove.Base.Target == MoveTarget.AllPlayer)
        {
            canSelected = false;
            StartCoroutine(RunMoveForAllPlayer(_playerSelectedUnit, curmove));
        }
        else if (curmove.Base.Target == MoveTarget.Self)
        {
            canSelected = false;
            StartCoroutine(RunMoveSelf(_playerSelectedUnit, curmove));
        }
        else if (move >= 4 && !_playerSelectedUnit.isPowerUsed)
        {
            _precision.ResetFillAmount();
            powerUsed = true;
            EnnemiSelection();
            _playerSelectedUnit.isPowerUsed = true;
        }
        else
        {
            EnnemiSelection();
        }
    }
}