using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster
{
    [SerializeField] MonstersBase _base;
    [SerializeField] int _level;
    public MonstersBase Base
    {
        get
        {
            return _base;
        }
    }
    public int Level
    {
        get
        {
            return _level;

        }
    }

    public int HP { get; set; }

    public List<Move> Moves { get; set; }


    public void Init()
    {
        HP = MaxHp;

        //Generate moves
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }
    }
    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
    }
    public int SpAttack
    {
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }
    }
    public int SpDefense
    {
        get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }
    public int MaxHp
    {
        get { return Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10; }
    }


    public DamageDetails TakeDamage(Move move, Monster attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false
        };


        float attack = (move.Base.IsSpecial) ? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.IsSpecial) ? SpDefense : Defense;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float atk = (2 * attacker.Level + 10) / 250f;
        float def = atk * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(def * modifiers);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }

        return damageDetails;


    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }

    public int GetRandomEnnemi()
    {
        int random = Random.Range(0, 3);
        return random;
    }
}


public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}

















