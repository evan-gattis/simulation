using UnityEngine;
using System.Collections.Generic;

public class EcosystemMap
{
    public List<GameObject> Creatures = new List<GameObject>();
    public List<GameObject> Resources = new List<GameObject>();
}

public class EcosystemMapBuilder
{
    private EcosystemMap map = new EcosystemMap();
    private float minX, maxX, minZ, maxZ, floorLevel;

    public EcosystemMapBuilder SetBounds(float minX, float maxX, float minZ, float maxZ, float floorLevel)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minZ = minZ;
        this.maxZ = maxZ;
        this.floorLevel = floorLevel;
        return this;
    }

    public EcosystemMapBuilder SpawnResources(int count, List<GameObject> resourcePrefabs)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject prefab = resourcePrefabs[Random.Range(0, resourcePrefabs.Count)];
            Vector3 position = GetRandomPosition();
            GameObject resource = Object.Instantiate(prefab, position, Quaternion.identity);
            map.Resources.Add(resource);
        }
        return this;
    }

    public EcosystemMapBuilder SpawnCreatures(int count, CreatureFactory factory)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = GetRandomPosition();
            GameObject creature = factory.createRandomCreature(position);
            map.Creatures.Add(creature);
        }
        return this;
    }

    public EcosystemMap Build()
    {
        return map;
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);
        return new Vector3(x, floorLevel + 1, z);
    }
}
