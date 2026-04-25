using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.WSA;

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
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private GameObject sheepPrefab;
    private ICreatureFactory creatureFactory;
    private EcosystemMap activeMap;
    void Start()
    {
        //Test cases:
        //StartCoroutine(TestWolfTargetsSheep(wolfPrefab, sheepPrefab));
        //StartCoroutine(TestWolfIdle(wolfPrefab));
        //StartCoroutine(TestSheepIdle(sheepPrefab));
        //StartCoroutine(TestWolfEatsSheep(wolfPrefab, sheepPrefab));
        //StartCoroutine(TestCreatureStarves(wolfPrefab));
        creatureFactory = new CreatureFactory(wolfPrefab, sheepPrefab);
        DefaultSimulation();
    }
    private void DefaultSimulation()
    {
        // for (int i = 0; i < startingNumberOfResources; i++)
        // {
        //     GameObject resourceToSpawn = resourcePrefabs[Random.Range(0, resourcePrefabs.Count)];
        //     int randomXValue = Random.Range((int)simulationMinXBound, (int)simulationMaxXBound + 1);
        //     int randomZValue = Random.Range((int)simulationMinZBound, (int)simulationMaxZBound + 1);
        //     Instantiate(resourceToSpawn, new Vector3(randomXValue, floorLevel + 1, randomZValue), Quaternion.identity);
        // }
        // for (int i = 0; i < startingNumberOfCreatures; i++)
        // {
        //     GameObject creatureToSpawn = creaturePrefabs[Random.Range(0, creaturePrefabs.Count)];
        //     int randomXValue = Random.Range((int)simulationMinXBound, (int)simulationMaxXBound + 1);
        //     int randomZValue = Random.Range((int)simulationMinZBound, (int)simulationMaxZBound + 1);
        //     Instantiate(creatureToSpawn, new Vector3(randomXValue, floorLevel + 1, randomZValue), Quaternion.identity);
        // }
        IMapBuilder mapBuilder = new EcosystemMapBuilder();
        activeMap = mapBuilder.SetBounds(simulationMinXBound, simulationMaxXBound, simulationMinZBound, simulationMaxZBound, floorLevel)
        .SpawnResources(startingNumberOfResources, resourcePrefabs)
        .SpawnCreatures(startingNumberOfCreatures, creatureFactory).Build();
    }

    IEnumerator TestWolfTargetsSheep(GameObject wolfPrefab, GameObject sheepPrefab)
    {
        GameObject wolf = Instantiate(wolfPrefab, new Vector3(0, floorLevel + 1, 0), Quaternion.identity);
        GameObject sheep = Instantiate(sheepPrefab, new Vector3(0, floorLevel + 1, 10), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Debug.Assert(wolf.GetComponent<Creature>().GetCurrentTarget() == sheep);
    }
    IEnumerator TestWolfIdle(GameObject wolfPrefab)
    {
        GameObject wolf = Instantiate(wolfPrefab, new Vector3(0, floorLevel + 1, 0), Quaternion.identity);
        Vector3 wolfPos = wolf.transform.position;
        yield return new WaitForSeconds(5f);
        Debug.Assert(wolf.transform.position != wolfPos);

    }
    IEnumerator TestSheepIdle(GameObject sheepPrefab)
    {
        GameObject sheep = Instantiate(sheepPrefab, new Vector3(0, floorLevel + 1, 10), Quaternion.identity);
        Vector3 sheepPos = sheep.transform.position;
        yield return new WaitForSeconds(5f);
        Debug.Assert(sheep.transform.position != sheepPos);
    }
    IEnumerator TestWolfEatsSheep(GameObject wolfPrefab, GameObject sheepPrefab)
    {
        GameObject wolf = Instantiate(wolfPrefab, new Vector3(0, floorLevel + 1, 0), Quaternion.identity);
        GameObject sheep = Instantiate(sheepPrefab, new Vector3(0, floorLevel + 1, 0), Quaternion.identity);
        yield return new WaitForSeconds(2f);
        Debug.Assert(sheep == null);
    }
    IEnumerator TestCreatureStarves(GameObject sheepPrefab)
    {
        GameObject wolf = Instantiate(wolfPrefab, new Vector3(0, floorLevel + 1, 0), Quaternion.identity);
        yield return new WaitForSeconds(30f);
        Debug.Assert(wolf == null);
    }

}
