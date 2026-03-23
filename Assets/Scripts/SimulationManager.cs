using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SimulationManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> creaturePrefabs;
    [SerializeField] private List<GameObject> resourcePrefabs;
    [SerializeField] private int startingNumberOfCreatures = 10;
    [SerializeField] private int startingNumberOfResources = 20;
    [SerializeField] private float simulationMinXBound = -50;
    [SerializeField] private float simulationMaxXBound = 50;
    [SerializeField] private float simulationMinZBound = -50;
    [SerializeField] private float simulationMaxZBound = 50;
    [SerializeField] private float floorLevel = -1;
    void Start()
    {
        for (int i = 0; i < startingNumberOfResources; i++)
        {
            GameObject resourceToSpawn = resourcePrefabs[Random.Range(0, resourcePrefabs.Count)];
            int randomXValue = Random.Range((int)simulationMinXBound, (int)simulationMaxXBound+1);
            int randomZValue = Random.Range((int)simulationMinZBound, (int)simulationMaxZBound + 1);
            Instantiate(resourceToSpawn, new Vector3(randomXValue, floorLevel+1, randomZValue), Quaternion.identity);
        }
        for (int i = 0; i < startingNumberOfCreatures; i++)
        {
            GameObject creatureToSpawn = creaturePrefabs[Random.Range(0, creaturePrefabs.Count)];
            int randomXValue = Random.Range((int)simulationMinXBound, (int)simulationMaxXBound + 1);
            int randomZValue = Random.Range((int)simulationMinZBound, (int)simulationMaxZBound + 1);
            Instantiate(creatureToSpawn, new Vector3(randomXValue, floorLevel+1, randomZValue), Quaternion.identity);
        }
    }
}
