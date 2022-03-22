using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionID
{
    none, psn
}
public class ConditionDB
{

    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.psn,
            new Condition()
            {
                Name = "Poison",
                StartMessage = "has been poisoned",
                OnStart = (Monster monster) =>
                {
                    monster.StatusTime = 3;
                        Debug.Log("Start Poison");
                },
                OnAfterTurn = (Monster monster) =>
                {
                        Debug.Log(monster.StatusTime);
                    if (monster.StatusTime <= 0)
                    {
                        monster.CureStatus();
                        Debug.Log("poison fini chef");
                    }
                    monster.StatusTime--;
                    monster.UpdateHP(monster.MaxHp / 8);
                }

            }
        }
    };

}
