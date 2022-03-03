using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{

    [SerializeField] MonstersBase _base;
    [SerializeField] int _level;
    [SerializeField] bool isPlayerUnit;

    public Monster Monster { get; set; }

    public void Setup()
    {
        Monster = new Monster(_base, _level);

        if (isPlayerUnit)
        {
            GetComponent<Image>().sprite = Monster.Base.BackSprite;
        }
        else
        {
            GetComponent<Image>().sprite = Monster.Base.FrontSprite;
        }

    }



}
