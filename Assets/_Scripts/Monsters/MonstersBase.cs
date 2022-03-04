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


    public MonsterType Type1;
    public MonsterType Type2;


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


public class TypeChart
{
    static float[][] chart =
    {

        //                     {NOR, FIR, WAT, ELEC, GRASS, ICE, FIGHT, POIS, GROUND, FLY, PSY, BUG, ROCK, GHOST, DRAG}
        /*NOR*/   new float[]  {1f,   1f, 1f,   1f ,  1f,    1f,  1f,    1f,   1f,    1f,  1f,  1f,  0.5f,  0f,    1f},
        /*FIR*/   new float[]  {1f, 0.5f, 0.5f, 1f ,  2f,    2f,  1f,    1f,   1f,    1f,  1f,  2f,  0.5f,  1f,   0.5f},
        /*WAT*/   new float[]  {1f,   2f, 0.5f, 1f ,  0.5f,  1f,  1f,    1f,   2f,    1f,  1f,  1f,   2f,   1f,   0.5f},
        /*ELEC*/  new float[]  {1f,   1f,  2f, 0.5f , 0.5f,  1f,  1f,    1f,   0f,    2f,  1f,  1f,   1f,   1f,   0.5f},
        /*GRASS*/ new float[]  {1f, 0.5f,  2f,  1f ,  0.5f,  1f,  1f,   0.5f,  2f,   0.5f, 1f, 0.5f,  2f,   1f,   0.5f},
        /*ICE*/   new float[]  {1f, 0.5f, 0.5f,  1f , 2f,   0.5f, 1f,    1f,   2f,    2f,  1f,  1f,   1f,   1f,    2f},
        /*FIGHT*/ new float[]  {2f,   1f,  1f,  1f ,  1f,    2f,  1f,   0.5f,  1f,   0.5f,0.5f,0.5f,  2f,   0f,    1f},
        /*POIS*/  new float[]  {1f,   1f,  1f,  1f ,  2f,    1f,  1f,   0.5f, 0.5f,   1f,  1f,  1f,  0.5f, 0.5f,   1f},
        /*GROUND*/new float[]  {1f,   2f,  1f,  2f ,  0.5f,  1f,  1f,    2f,   1f,    0f,  1f, 0.5f,  2f,   1f,    1f},
        /*FLY*/   new float[]  {1f, 0.5f,  1f, 0.5f , 2f,    1f,  2f,    1f,   1f,    1f,  1f,  2f,  0.5f,  1f,    1f},
        /*PSY*/   new float[]  {1f,   1f,  1f,  1f ,  1f,    1f,  2f,    2f,   1f,    1f, 0.5f, 1f,   1f,   1f,    1f},
        /*BUG*/   new float[]  {1f, 0.5f,  1f,  1f ,  2f,    1f, 0.5f,  0.5f,  1f,   0.5f, 2f,  1f,   1f,  0.5f,   1f},
        /*ROCK*/  new float[]  {1f,   2f,  1f,  1f ,  1f,    2f, 0.5f,   1f,  0.5f,   2f,  1f,  2f,   1f,   1f,    1f},
        /*GHOST*/ new float[]  {0f,   1f,  1f,  1f ,  1f,    1f,  1f,    1f,   1f,    1f,  2f,  1f,   1f,   2f,    1f},
        /*DRAG*/  new float[]  {1f,   1f,  1f,  1f ,  1f,    1f,  1f,    1f,   1f,    1f,  1f,  1f,   1f,   1f,    2f}
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