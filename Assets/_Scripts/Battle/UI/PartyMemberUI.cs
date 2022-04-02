using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text _nameText;
    [SerializeField] Text _levelText;
    [SerializeField] HPBar _hpBar;


    [SerializeField] Color _highlightedColor;

    private Monster _monster;

    public void SetData(Monster monster)
    {
        _monster = monster;

        _nameText.text = monster.Base.Name;
        _levelText.text = "Lvl" + monster.Level;
        _hpBar.SetHP((float)monster.HP / monster.MaxHp);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            _nameText.color = _highlightedColor;
        }
        else
            _nameText.color = Color.black;
    }
}
