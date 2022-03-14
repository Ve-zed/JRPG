using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text _nameText;
    [SerializeField] Text _levelText;
    [SerializeField] HPBar _hpBar;
    [SerializeField] Image _image;
    [SerializeField] BattleUnit _unit;

    private Monster _monster;



    public void SetData(Monster monster)
    {
        _monster = monster;

        _nameText.text = monster.Base.Name;
        _levelText.text = "Lvl" + monster.Level;
        monster.HP = monster.MaxHp;
        /*if(_unit.isPlayerUnit)
        _image.sprite = monster.Base.BackSprite;
        else
        _image.sprite = monster.Base.FrontSprite;
        _image.color = _unit.originalColor;
        _unit.PlayEnterAnimation();*/

    }

    public IEnumerator UpdateHP()
    {
        yield return _hpBar.SetHPSmooth((float)_monster.HP / _monster.MaxHp);
    }

}
