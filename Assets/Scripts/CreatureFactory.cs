using System.Collections.Generic;
using UnityEngine;

public class CreatureFactory
{
    private GameObject baseWolf;
    private GameObject baseSheep;

    public CreatureFactory(GameObject wolf, GameObject sheep)
    {
        this.baseWolf = wolf;
        this.baseSheep = sheep;
    }

    public GameObject createWolf(Vector3 position)
    {
        return Object.Instantiate(baseWolf, position, Quaternion.identity);
    }

    public GameObject createSheep(Vector3 position)
    {
        return Object.Instantiate(baseSheep, position, Quaternion.identity);
    }

    public GameObject createRandomCreature(Vector3 position)
    {
        int coin = Random.Range(0, 2);
        return coin == 0 ? createWolf(position) : createSheep(position);
    }
}
