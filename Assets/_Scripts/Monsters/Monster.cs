using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MoveBase;

[System.Serializable]
public class Monster
{
    [SerializeField] MonstersBase _base;
    public int _level;
    
    public int Exp = 0;
    public MonstersBase Base { get { return _base; } }
    public int Level { get { return _level; } }
    public int HP { get; set; }
    public List<Move> Moves { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public Condition Status { get; private set; }
    public bool HpChanged { get; set; }
    public int StatusTime { get; set; }
    public int MaxHp { get; private set; }
    public int Damage {get; set;}


    public void Init()
    {
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 5)
                break;
        }
       
        CalculateStats();

        HP = MaxHp;

        ResetStatBoost();
    }



    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Base.Attack );
        Stats.Add(Stat.Defense, Base.Defense );
        Stats.Add(Stat.SpAttack, Base.SpAttack );
        Stats.Add(Stat.SpDefense, Base.SpDefense);
        Stats.Add(Stat.Speed, Base.Speed );

        MaxHp = Base.MaxHp;
    }

    public void ResetStatBoost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0},
            {Stat.Defense, 0},
            {Stat.SpAttack, 0},
            {Stat.SpDefense, 0},
            {Stat.Speed, 0},
        };
    }


    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        int boost = StatBoosts[stat];
        var boostValues = new float[] { 10f, 15f,20f,25f, 1f, 10000f, 20000f };

        if (boost >= 0)
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        else
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);
        return statVal;
    }

    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach (var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

        }
    }


    public int Attack{get { return GetStat(Stat.Attack); }}
    public int Defense{get { return GetStat(Stat.Defense); }}
    public int SpAttack{get { return GetStat(Stat.SpAttack); }}
    public int SpDefense{get { return GetStat(Stat.SpDefense); }}
    public int Speed{get { return GetStat(Stat.Speed); }}


    public DamageDetails TakeDamage(Move move, Monster attacker, int multiplicateur)
    {
        Damage = 0;
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


        float attack = (move.Base.Category == MoveCategory.Special) ? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.Category == MoveCategory.Special) ? SpDefense : Defense;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float atk = (2 * attacker.Level + 10) / 250f;
        float def = atk * move.Base.Power * ((float)attack / defense) + 2;
        if (defense > 50000)
            def = 0;

        Damage = Mathf.FloorToInt(def * modifiers);

        
        UpdateHP(Damage * multiplicateur);
        Debug.Log(Damage * multiplicateur);
        return damageDetails;

    }
    public void CureStatus()
    {
        Status = null;
    }
    public void UpdateHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHp);
        HpChanged = true;
    }

    public void SetStatus(ConditionID conditionID)
    {
        Status = ConditionDB.Conditions[conditionID];
        Status?.OnStart?.Invoke(this);
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
    public bool OnBeforeMove()
    {
        if (Status?.OnBeforeMove != null)
        {
            return Status.OnBeforeMove(this);
        }
        return true;
    }
    public bool OnFocusTarget()
    {
        if (Status?.OnFocusTarget != null)
        {
            return Status.OnFocusTarget(this);
        }
        return false;
    }
    public void OnAfterTurn(BattleUnit sourceUnit)
    {
        Status?.OnAfterTurn?.Invoke(this);
        sourceUnit.Hud.UpdateHP();
    }

    public void OnBattleOver()
    {
        ResetStatBoost();
    }

}


public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
