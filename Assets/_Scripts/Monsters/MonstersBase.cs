using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/Create new monster")]
public class MonstersBase : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] string _description;
    public Sprite EnnemiSprite;
    public Sprite PlayerSprite;

    public MonsterType type1;
    public MonsterType type2;

    //Base stats
    [SerializeField] int _maxHp;
    [SerializeField] int _attack;
    [SerializeField] int _defense;
    [SerializeField] int _spAttack;
    [SerializeField] int _spDefense;
    [SerializeField] int _speed;

    [SerializeField] List<LearnableMove> _learnableMoves;

    [SerializeField] List<int> _XPByLevel;


    public string Name{get { return _name; }}
    public string Description{get { return _description; }}
    public int MaxHp{get { return _maxHp; }}
    public int Attack{get { return _attack; }}
    public int Defense{get { return _defense; }}
    public int SpAttack{get { return _spAttack; }}
    public int SpDefense{get { return _spDefense; }}
    public int Speed{get { return _speed; }}

    public List<LearnableMove> LearnableMoves{get { return _learnableMoves; }}
   public List<int> XPByLevel { get { return _XPByLevel; }}

}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase _moveBase;
    [SerializeField] int _level;

    public MoveBase Base{get { return _moveBase; }}
    public int Level{get { return _level; }}

}


public enum Stat
{
    Attack, Defense, SpAttack, SpDefense, Speed
}

public enum MonsterType
{
    None,
    Normal,
    Virus
}


public class TypeChart
{
    static float[][] chart =
    {

        //                     {NOR, VIRUS}
        /*NOR*/   new float[]  {1f,   2f},
        /*VIRUS*/ new float[]  {1f,   1f}
    };

    public static float GetEffectiveness(MonsterType attackType, MonsterType defenseType)
    {
        if (attackType == MonsterType.None || defenseType == MonsterType.None)
            return 1;

        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];

    }

}