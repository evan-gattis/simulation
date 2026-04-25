using System;
using UnityEngine;

public interface ICreatureFactory
{
    GameObject createWolf(UnityEngine.Vector3 position);
    GameObject createSheep(UnityEngine.Vector3 positon);
    GameObject createRandomCreature(UnityEngine.Vector3 positon);
}