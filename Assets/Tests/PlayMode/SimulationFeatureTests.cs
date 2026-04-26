using System.Collections;
using NUnit.Framework;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

public class SimulationFeatureTests
{
    private const string WOLF_PATH = "TestPrefabs/Wolf";
    private const string SHEEP_PATH = "TestPrefabs/Sheep";
    private const string GRASS_PATH = "TestPrefabs/Grass";

    private GameObject wolfPrefab;
    private GameObject sheepPrefab;
    private GameObject grassPrefab;
    private GameObject ground;

    //Allowance for the true object position to differ from what we asked to spawn the object at.
    private const float PositionTolerance = 1f;

    private static void AssertVectorApproxEqual(Vector3 expected, Vector3 actual, string message = "")
    {
        Assert.LessOrEqual(Vector3.Distance(expected, actual), PositionTolerance, message);
    }

    [UnitySetUp]
    public IEnumerator UnitySetUp()
    {
        wolfPrefab = Resources.Load<GameObject>(WOLF_PATH);
        sheepPrefab = Resources.Load<GameObject>(SHEEP_PATH);
        grassPrefab = Resources.Load<GameObject>(GRASS_PATH);

        Assert.IsNotNull(wolfPrefab, $"Wolf prefab not found at Resources/{WOLF_PATH}. See setup instructions in test file.");
        Assert.IsNotNull(sheepPrefab, $"Sheep prefab not found at Resources/{SHEEP_PATH}.");
        Assert.IsNotNull(grassPrefab, $"Grass prefab not found at Resources/{GRASS_PATH}.");

        ground = BuildNavMeshGround();

        //Wait a frame to ensure the navmesh is visible
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator UnityTearDown()
    {
        //Destroy everything before next test
        foreach (var go in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (go.scene.IsValid())
            {
                Object.Destroy(go);
            }
        }
        yield return null;
    }

    private static GameObject BuildNavMeshGround()
    {
        var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "TestGround";
        ground.transform.position = new Vector3(0, -0.5f, 0);
        ground.transform.localScale = new Vector3(200, 1, 200);

        //boilerplate for baking a navmesh
        var surface = ground.AddComponent<NavMeshSurface>();
        surface.collectObjects = CollectObjects.All;
        surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        surface.layerMask = ~0; //Get every layer
        surface.BuildNavMesh();

        return ground;
    }

    [UnityTest]
    public IEnumerator WolfTargetsNearbySheep()
    {
        var wolf = Object.Instantiate(wolfPrefab, Vector3.zero, Quaternion.identity);
        var sheep = Object.Instantiate(sheepPrefab, new Vector3(5, 0, 0), Quaternion.identity);

        yield return new WaitForSeconds(1f);

        var wolfCreature = wolf.GetComponent<Creature>();
        Assert.AreEqual(sheep, wolfCreature.GetCurrentTarget(),
            "Wolf should target the nearby sheep.");
    }

    [UnityTest]
    public IEnumerator WolfEatsSheepWhenInRange()
    {
        var wolf = Object.Instantiate(wolfPrefab, Vector3.zero, Quaternion.identity);
        var sheep = Object.Instantiate(sheepPrefab, new Vector3(0.5f, 0, 0), Quaternion.identity);

        yield return new WaitForSeconds(2f);

        Assert.IsTrue(sheep == null, "Sheep should be destroyed after being eaten.");
    }

    [UnityTest]
    public IEnumerator TwoWolvesCompetingForSheepOneWolfDies()
    {
        var wolfA = Object.Instantiate(wolfPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        var wolfB = Object.Instantiate(wolfPrefab, new Vector3(0.5f, 0, 0), Quaternion.identity);
        var sheep = Object.Instantiate(sheepPrefab, new Vector3(8f, 0, 0.5f), Quaternion.identity);

        //Wait long enough for one wolf to kill another
        yield return new WaitForSeconds(4f);

        bool aDead = (wolfA == null);
        bool bDead = (wolfB == null);

        Assert.IsTrue(aDead || bDead, "At least one wolf should have died from competition.");
        Assert.IsFalse(aDead && bDead, "Both wolves should not die in the same encounter.");
    }

    [UnityTest]
    public IEnumerator SheepEatsGrassWhenNearby()
    {
        var sheep = Object.Instantiate(sheepPrefab, Vector3.zero, Quaternion.identity);
        var grass = Object.Instantiate(grassPrefab, new Vector3(0.5f, 0, 0), Quaternion.identity);

        yield return new WaitForSeconds(2f);

        Assert.IsTrue(grass == null, "Grass should be eaten by the nearby sheep.");
    }

    [UnityTest]
    [Timeout(45000)] //45s to allow the full 30s starve cycle
    public IEnumerator WolfStarvesWhenNoFoodPresent()
    {
        var wolf = Object.Instantiate(wolfPrefab, Vector3.zero, Quaternion.identity);

        //Wait a bit longer in case of error
        yield return new WaitForSeconds(31f);

        Assert.IsTrue(wolf == null, "Wolf should starve and die when no food is available.");
    }


    [UnityTest]
    public IEnumerator CreatureFactoryCreatesWolfAtPosition()
    {
        var factory = new CreatureFactory(wolfPrefab, sheepPrefab);
        var spawned = factory.createWolf(new Vector3(3, 0, 4));

        yield return null; //wait for awake method to run

        Assert.IsNotNull(spawned);
        Assert.IsNotNull(spawned.GetComponent<Wolf>(), "Spawned object should be a Wolf.");
        AssertVectorApproxEqual(new Vector3(3, 0, 4), spawned.transform.position,
            "Wolf should spawn near the requested position.");
    }

    [UnityTest]
    public IEnumerator CreatureFactoryCreatesSheepAtPosition()
    {
        var factory = new CreatureFactory(wolfPrefab, sheepPrefab);
        var spawned = factory.createSheep(new Vector3(-2, 0, 1));

        yield return null;

        Assert.IsNotNull(spawned);
        Assert.IsNotNull(spawned.GetComponent<Sheep>(), "Spawned object should be a Sheep.");
        AssertVectorApproxEqual(new Vector3(-2, 0, 1), spawned.transform.position,
            "Sheep should spawn near the requested position.");
    }

    [UnityTest]
    public IEnumerator MapBuilderSpawnsResourcesWithinBounds()
    {
        var builder = new EcosystemMapBuilder();
        var map = builder
            .SetBounds(-10, 10, -10, 10, 0)
            .SpawnResources(5, new System.Collections.Generic.List<GameObject> { grassPrefab })
            .Build();

        yield return null;

        Assert.AreEqual(5, map.Resources.Count, "Builder should spawn the requested number of resources.");
        foreach (var resource in map.Resources)
        {
            Assert.IsNotNull(resource);
            var pos = resource.transform.position;
            Assert.GreaterOrEqual(pos.x, -10 - PositionTolerance);
            Assert.LessOrEqual(pos.x, 10 + PositionTolerance);
            Assert.GreaterOrEqual(pos.z, -10 - PositionTolerance);
            Assert.LessOrEqual(pos.z, 10 + PositionTolerance);
        }
    }

    [UnityTest]
    public IEnumerator MapBuilderSpawnsCreaturesInRequestedCount()
    {
        var factory = new CreatureFactory(wolfPrefab, sheepPrefab);
        var builder = new EcosystemMapBuilder();
        var map = builder
            .SetBounds(-10, 10, -10, 10, 0)
            .SpawnCreatures(4, factory)
            .Build();

        yield return null;

        Assert.AreEqual(4, map.Creatures.Count);
    }
}
