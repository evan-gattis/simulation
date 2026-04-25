using System;
using System.Collections.Generic;
using UnityEngine;

public interface IMapBuilder
{
    IMapBuilder SetBounds(float minX, float maxX,float minZ, float maxZ, float floorLevel);
    IMapBuilder SpawnResources(int count, List<GameObject> resourcePrefabs);
    IMapBuilder SpawnCreatures(int count, ICreatureFactory factory);
    EcosystemMap Build();
}