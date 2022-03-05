using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Monster> _wildMonsters;

    public Monster GetRandomWildMonster()
    {
        var wildMonster =  _wildMonsters[Random.Range(0, _wildMonsters.Count)];
        wildMonster.Init();
        return wildMonster;
    }
}
