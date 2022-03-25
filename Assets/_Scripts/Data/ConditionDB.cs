using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionID
{
    none, Psn, Confus, Provocation, BuffDebuff, ArmureTotal
}
public class ConditionDB
{
    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {

        {
            ConditionID.Psn,
            new Condition()
            {

                Name = "Poison",
                StartMessage = "has been poisoned",
                OnStart = (Monster monster) =>
                {
                    if(PrecisionBar.Instance.barreSplit == 1)
                    {
                        //red
                        monster.StatusTime = 2;
                    }
                    else if (PrecisionBar.Instance.barreSplit == 2)
                    {
                        //orange
                        monster.StatusTime = 4;
                    }
                    else if (PrecisionBar.Instance.barreSplit == 3)
                    {
                        //green
                        monster.StatusTime = 6;
                    }
                },
                OnAfterTurn = (Monster monster) =>
                {
                    if (monster.StatusTime <= 0)
                    {
                        monster.CureStatus();
                    }
                    monster.StatusTime--;
                    monster.UpdateHP(monster.MaxHp / 8);
                }
            }
        },
        {
            ConditionID.Confus,
            new Condition()
            {
                Name = "Confus",
                StartMessage = "has been confued",
                OnStart = (Monster monster) =>
                {
                    monster.StatusTime = 1;
                    Debug.Log(monster.StatusTime);
                },
                OnBeforeMove = (Monster monster) =>
                {
                    if (monster.StatusTime <= 0)
                    {
                    return true;
                    }
                    monster.StatusTime--;
                        return false;

                },
            }
        },
        {
            ConditionID.Provocation,
            new Condition()
            {
                Name = "Provocation",
                StartMessage = "has been provoqued",
                OnStart = (Monster monster) =>
                {
                    monster.StatusTime = 1;
                },
                OnFocusTarget = (Monster monster) =>
                {
                    if (monster.StatusTime <= 0)
                    {
                        return true;
                    }
                    monster.StatusTime--;
                    return false;
                },
            }
        },
        {
            ConditionID.BuffDebuff,
            new Condition()
            {
                Name = "Buff",
                StartMessage = "has been buffed",
                OnStart = (Monster monster) =>
                {
                    monster.StatusTime = 1;
                },
                OnAfterTurn = (Monster monster) =>
                {
                    if (monster.StatusTime <= 0)
                    {
                        monster.ResetStatBoost();
                    }
                    monster.StatusTime--;
                }
            }
        },
        {
            ConditionID.ArmureTotal,
            new Condition()
            {
                Name = "ArmureTotal",
                StartMessage = "has been ArmureTotaled",
                OnStart = (Monster monster) =>
                {
                    monster.StatusTime = 1;
                },
                OnAfterTurn = (Monster monster) =>
                {
                    if (monster.StatusTime <= 0)
                    {
                        monster.ResetStatBoost();
                    }
                    monster.StatusTime--;
                }
            }
        }
    };
}
