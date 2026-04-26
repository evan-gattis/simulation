using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class CarnivoreTargetStrategyTests
{
    private GameObject ownerGo;
    private GameObject competitorGo;
    private CarnivoreTargetStrategy strategy;
    private CarnivoreTargetStrategy competitorStrategy;

    //Setup gets run before any test in this class
    [SetUp]
    public void SetUp()
    {
        ownerGo = new GameObject("OwnerWolf");
        competitorGo = new GameObject("CompetitorWolf");

        strategy = new CarnivoreTargetStrategy {OwnerCreature = ownerGo};
        competitorStrategy = new CarnivoreTargetStrategy {OwnerCreature = competitorGo};
    }

    [TearDown]
    public void TearDown()
    {
        if (ownerGo != null) Object.DestroyImmediate(ownerGo);
        if (competitorGo != null) Object.DestroyImmediate(competitorGo);
    }

    [Test]
    public void NotifyWithCompetitorTargetsTheCompetitor()
    {
        var observers = new List<IPreyObserver> { strategy, competitorStrategy };

        strategy.Notify(observers);

        Assert.AreEqual(competitorGo, strategy.CurrentTarget);
    }

    [Test]
    public void NotifyWithOnlySelfDoesNotSetTarget()
    {
        var observers = new List<IPreyObserver> { strategy };

        strategy.Notify(observers);

        Assert.IsNull(strategy.CurrentTarget);
    }

    [Test]
    public void NotifyNeverTargetsSelf()
    {
        var observers = new List<IPreyObserver> { strategy, competitorStrategy };

        strategy.Notify(observers);

        Assert.AreNotEqual(ownerGo, strategy.CurrentTarget);
    }
}
