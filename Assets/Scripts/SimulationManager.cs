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
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private GameObject sheepPrefab;
    private CreatureFactory creatureFactory;
    private EcosystemMap activeMap;
    private GameObject watchedCreature;

    void Start()
    {
        creatureFactory = new CreatureFactory(wolfPrefab, sheepPrefab);
        DefaultSimulation();
        SwitchToRandomCreature();
    }

    void Update()
    {
        //Switch to random creature as the main camera whenever the watched on dies
        if (watchedCreature == null)
            SwitchToRandomCreature();
    }

    private void SwitchToRandomCreature()
    {
        //For edge cases I haven't thought of
        if (watchedCreature != null)
        {
            var oldCam = watchedCreature.GetComponentInChildren<Camera>(true);
            if (oldCam != null) oldCam.gameObject.SetActive(false);
        }

        //Remove dead creatures from list
        activeMap.Creatures.RemoveAll(c => c == null);

        if (activeMap.Creatures.Count == 0)
        {
            watchedCreature = null;
            return;
        }

        watchedCreature = activeMap.Creatures[Random.Range(0, activeMap.Creatures.Count)];
        var cam = watchedCreature.GetComponentInChildren<Camera>(true);
        if (cam != null) cam.gameObject.SetActive(true);
    }
    private void DefaultSimulation()
    {
        EcosystemMapBuilder mapBuilder = new EcosystemMapBuilder();
        activeMap = mapBuilder.SetBounds(simulationMinXBound, simulationMaxXBound, simulationMinZBound, simulationMaxZBound, floorLevel)
        .SpawnResources(startingNumberOfResources, resourcePrefabs)
        .SpawnCreatures(startingNumberOfCreatures, creatureFactory).Build();
    }
}
