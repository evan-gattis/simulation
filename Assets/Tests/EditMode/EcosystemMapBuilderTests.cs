using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EcosystemMapBuilderTests
{
    [Test]
    public void BuildReturnsMapWithEmptyListsWhenNothingSpawned()
    {
        var builder = new EcosystemMapBuilder();
        var map = builder.SetBounds(-10, 10, -10, 10, 0).Build();

        Assert.IsNotNull(map);
        Assert.IsNotNull(map.Creatures);
        Assert.IsNotNull(map.Resources);
        Assert.AreEqual(0, map.Creatures.Count);
        Assert.AreEqual(0, map.Resources.Count);
    }

    [Test]
    public void SetBoundsReturnsSelfForFluentChaining()
    {
        var builder = new EcosystemMapBuilder();
        var result = builder.SetBounds(-10, 10, -10, 10, 0);

        Assert.AreSame(builder, result, "SetBounds should return the builder for chaining.");
    }

    [Test]
    public void SpawnResourcesReturnsSelfForFluentChaining()
    {
        var builder = new EcosystemMapBuilder();
        var result = builder.SetBounds(-10, 10, -10, 10, 0)
            .SpawnResources(0, new List<GameObject>());

        Assert.AreSame(builder, result);
    }
}

public class EcosystemMapTests
{
    [Test]
    public void NewMapHasEmptyCreatureAndResourceLists()
    {
        var map = new EcosystemMap();

        Assert.IsNotNull(map.Creatures);
        Assert.IsNotNull(map.Resources);
        Assert.AreEqual(0, map.Creatures.Count);
        Assert.AreEqual(0, map.Resources.Count);
    }
}
