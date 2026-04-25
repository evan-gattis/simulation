using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;


public class CreatureFactory : ICreatureFactory
{
    private GameObject baseWolf;
    private GameObject baseSheep;

    public CreatureFactory(GameObject wolf, GameObject sheep)
    {
        this.baseWolf = wolf;
        this.baseSheep = sheep;
    }

    public GameObject createWolf(UnityEngine.Vector3 position)
    {
        return UnityEngine.Object.Instantiate(baseWolf, position, UnityEngine.Quaternion.identity);
    }

    public GameObject createSheep(UnityEngine.Vector3 position)
    {
        return UnityEngine.Object.Instantiate(baseSheep, position, UnityEngine.Quaternion.identity);
    }

    public GameObject createRandomCreature(UnityEngine.Vector3 position)
    {
        int coin = UnityEngine.Random.Range(0,2);
        if(coin == 0)
        {
            return createWolf(position);
        }
        else
        {
            return createSheep(position);
        }
    }
}