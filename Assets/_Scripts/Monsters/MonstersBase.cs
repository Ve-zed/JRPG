using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/Create new monster")]
public class MonstersBase : ScriptableObject
{
    [SerializeField] string _name;

    [SerializeField] string _description;
    public Sprite FrontSprite;
    public Sprite BackSprite;


    [SerializeField] MonsterType _type1;
    [SerializeField] MonsterType _type2;


    //Base stats
    [SerializeField] int _maxHp;
    [SerializeField] int _attack;
    [SerializeField] int _defense;
    [SerializeField] int _spAttack;
    [SerializeField] int _spDefense;
    [SerializeField] int _speed;


    [SerializeField] List<LearnableMove> _learnableMoves;

    public string Name
    {
        get { return _name; }
    }
    public string Description
    {
        get { return _description; }
    }
    public int MaxHp
    {
        get { return _maxHp; }
    }
    public int Attack
    {
        get { return _attack; }
    }
    public int Defense
    {
        get { return _defense; }
    }
    public int SpAttack
    {
        get { return _spAttack; }
    }
    public int SpDefense
    {
        get { return _spDefense; }
    }
    public int Speed
    {
        get { return _speed; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return _learnableMoves; }
    }

}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase _moveBase;
    [SerializeField] int _level;

    public MoveBase Base
    {
        get { return _moveBase; }
    }
    public int Level
    {
        get { return _level; }
    }

}




public enum MonsterType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon
}
