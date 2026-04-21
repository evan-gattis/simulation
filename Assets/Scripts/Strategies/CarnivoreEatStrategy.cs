using UnityEngine;

public class CarnivoreEatStrategy : IEatStrategy
{
    public void eat(IEdible food)
    {
        if (food == null) return;

        food.OnEaten();
    }
}
