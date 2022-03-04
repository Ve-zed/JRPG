using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text _nameText;
    [SerializeField] Text _levelText;
    [SerializeField] HPBar _hpBar;

    Monster _monster;

    public void SetData(Monster monster)
    {
        _monster = monster;

        _nameText.text = monster.Base.Name;
        _levelText.text = "Lvl" + monster.Level;
        _hpBar.SetHP((float)monster.HP / monster.MaxHp);
    }

    public IEnumerator UpdateHP()
    {
        yield return _hpBar.SetHPSmooth((float)_monster.HP / _monster.MaxHp);
    }

}
