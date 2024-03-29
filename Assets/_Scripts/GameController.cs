using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum GameState { FreeRoam, Battle, Dialog, CutScene, Tuto }


public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] BattleSystem _battleSystem;
    [SerializeField] Camera _worldCamera;
    private EnnemiController _ennemi;

    public int mHR = 0;


    private GameState _state;

    public static GameController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _playerController.onEncountered += StartBattle;
        _battleSystem.onBattleOver += EndBattle;

        _playerController.onEnterEnnemisView += (Collider2D ennemiCollider) =>
        {
            var ennemi = ennemiCollider.GetComponentInParent<EnnemiController>();
            if (ennemi != null)
            {
                _state = GameState.CutScene;
                StartCoroutine(ennemi.TriggerEnnemiBattle(_playerController));
            }
        };
        DialogManager.Instance.onShowDialog += () =>
        {
            _state = GameState.Dialog;
        };
        DialogManager.Instance.onCloseDialog += () =>
        {
            if (_state == GameState.Dialog)
                _state = GameState.FreeRoam;
        };
        DialogManager.Instance.onShowDialogTuto += () =>
        {
            _state = GameState.Tuto;
        };
        DialogManager.Instance.onCloseDialogTuto += () =>
        {
            if (_state == GameState.Tuto)
                _state = GameState.FreeRoam;
        };
    }

    private void StartBattle()
    {
        _state = GameState.Battle;
        _battleSystem.gameObject.SetActive(true);
        _worldCamera.gameObject.SetActive(false);
    }
    public void StartEnnemiBattle(EnnemiController ennemi)
    {
        //FadeBattle.Instance.imageFadeInBattle.DOFade(0, 2.5f);

        _state = GameState.Battle;
        _battleSystem.gameObject.SetActive(true);
        _worldCamera.gameObject.SetActive(false);

        _ennemi = ennemi;
        var playerParty = _playerController.GetComponent<MonsterParty>();
        var ennemiParty = ennemi.GetComponent<MonsterParty>();


        _battleSystem.StartEnnemiBattle(playerParty, ennemiParty);

    }

    private void EndBattle(bool won)
    {
        if (_ennemi != null && won == true)
        {
            _ennemi.BattleLost();
            _ennemi = null;
            _battleSystem.isEnnemiBattle = false;

        }
        _state = GameState.FreeRoam;
        _battleSystem.gameObject.SetActive(false);
        _worldCamera.gameObject.SetActive(true);
    }


    private void Update()
    {
        if (_state == GameState.FreeRoam)
        {
            _playerController.HandleUpdate();
        }
        else if (_state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (_state == GameState.Tuto)
        {
            DialogManager.Instance.HandleUpdateTuto();
        }
    }


}
