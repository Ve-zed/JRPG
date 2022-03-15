using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterParty : MonoBehaviour
{
    [SerializeField] List<Monster> _monsters;

    public List<Monster> Monsters
    {
        get
        {
            return _monsters;
        }
    }
    private void Start()
    {
        foreach (var monster in _monsters)
        {
            monster.Init();
        }
    }


    public Monster GetHealthyMonster()
    {
        return _monsters.Where(x => x.HP > 0).FirstOrDefault();
        //return _monsters.Where(x => x.HP > 0).LastOrDefault();
        

    }


}
