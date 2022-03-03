using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit _playerUnit;
    [SerializeField] BattleUnit _ennemyUnit;
    [SerializeField] BattleHud _playerHud;
    [SerializeField] BattleHud _ennemyHud;

    private void Start()
    {
        SetupBattle();
    }

    public void SetupBattle()
    {
        _playerUnit.Setup();
        _ennemyUnit.Setup();
        _playerHud.SetData(_playerUnit.Monster);
        _ennemyHud.SetData(_ennemyUnit.Monster);
    }
}
