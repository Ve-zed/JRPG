using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Move", menuName = "Monster/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string _name;

    [TextArea]
    [SerializeField] string _description;

    [SerializeField] MonsterType _type;
    [SerializeField] int _power;
    [SerializeField] int _accuary;
    [SerializeField] int _pp; //number of times a move can be performed

    public string Name
    {
        get { return _name; }
    }
    public string Description
    {
        get { return _description; }
    }
    public MonsterType Type
    {
        get { return _type; }
    }
    public int Power
    {
        get { return _power; }
    }
    public int Accuary
    {
        get { return _accuary; }
    }
    public int PP
    {
        get { return _pp; }
    }
}
