using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text _levelText;
    [SerializeField] HPBar _hpBar;
    [SerializeField] Image _image;
    [SerializeField] BattleUnit _unit;

    private Monster _monster;

    public void SetData(Monster monster)
    {
        _monster = monster;

        _levelText.text = "Lvl" + monster.Level;
        monster.HP = monster.MaxHp;
    }

    public IEnumerator UpdateHP()
    {
        if (_monster.HpChanged)
        {
            yield return _hpBar.SetHPSmooth((float)_monster.HP / _monster.MaxHp);
            _monster.HpChanged = false;
        }
    }

}
