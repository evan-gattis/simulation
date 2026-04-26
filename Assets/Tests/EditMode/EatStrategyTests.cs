using NUnit.Framework;
using UnityEngine;

public class EatStrategyTests
{
    //Fake creature/resource that is simple enough to test in editor mode
    private class FakeFood : IEdible
    {
        public bool wasEaten = false;
        public bool IsAvailable => !wasEaten;
        public void OnEaten() { wasEaten = true; }
    }

    [Test]
    public void CarnivoreEatStrategyEatCallsOnEatenOnFood()
    {
        var strategy = new CarnivoreEatStrategy();
        var food = new FakeFood();

        strategy.eat(food);

        Assert.IsTrue(food.wasEaten);
    }

    [Test]
    public void CarnivoreEatStrategyCanEatTrueWhenInRange()
    {
        var strategy = new CarnivoreEatStrategy();
        var eater = new GameObject("eater");
        var target = new GameObject("target");
        eater.transform.position = Vector3.zero;
        target.transform.position = new Vector3(1.0f, 0, 0);

        try { Assert.IsTrue(strategy.CanEat(eater, target)); }
        finally
        {
            Object.DestroyImmediate(eater);
            Object.DestroyImmediate(target);
        }
    }

    [Test]
    public void CarnivoreEatStrategyCanEatFalseWhenOutOfRange()
    {
        var strategy = new CarnivoreEatStrategy();
        var eater = new GameObject("eater");
        var target = new GameObject("target");
        eater.transform.position = Vector3.zero;
        target.transform.position = new Vector3(5.0f, 0, 0);

        try { Assert.IsFalse(strategy.CanEat(eater, target)); }
        finally
        {
            Object.DestroyImmediate(eater);
            Object.DestroyImmediate(target);
        }
    }

    [Test]
    public void HerbivoreEatStrategyEatCallsOnEatenOnFood()
    {
        var strategy = new HerbivoreEatStrategy();
        var food = new FakeFood();

        strategy.eat(food);

        Assert.IsTrue(food.wasEaten);
    }

    [Test]
    public void HerbivoreEatStrategyCanEatTrueWhenInRange()
    {
        var strategy = new HerbivoreEatStrategy();
        var eater = new GameObject("eater");
        var target = new GameObject("target");
        eater.transform.position = Vector3.zero;
        target.transform.position = new Vector3(1.0f, 0, 0);

        try { Assert.IsTrue(strategy.CanEat(eater, target)); }
        finally
        {
            Object.DestroyImmediate(eater);
            Object.DestroyImmediate(target);
        }
    }
}
