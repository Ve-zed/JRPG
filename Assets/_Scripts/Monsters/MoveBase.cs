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
    [SerializeField] int _pp;
    [SerializeField] MoveCategory _category;
    [SerializeField] MoveEffects _effects;
    [SerializeField] MoveTarget _target;


    public string Name { get { return _name; } }
    public string Description { get { return _description; } }
    public MonsterType Type { get { return _type; } }
    public int Power { get { return _power; } }
    public int Accuary { get { return _accuary; } }
    public int PP { get { return _pp; } }
    public MoveCategory Category { get { return _category; } }
    public MoveEffects Effects { get { return _effects; } }
    public MoveTarget Target { get { return _target; } }


    //chantier
    [System.Serializable]
    public class MoveEffects
    {
        [SerializeField] List<StatBoost> boosts;
        [SerializeField] ConditionID _status;

        public List<StatBoost> Boosts { get { return boosts; } }

        public ConditionID Status { get { return _status; } }

    }

    [System.Serializable]
    public class StatBoost
    {
        public Stat stat;
        public int boost;
    }

    public enum MoveCategory
    {
        Physical, Special, Status
    }
    public enum MoveTarget
    {
        Foe, Self, AllEnnemi, All
    }



}
