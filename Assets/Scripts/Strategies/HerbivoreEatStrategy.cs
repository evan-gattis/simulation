using UnityEngine;

public class HerbivoreEatStrategy : IEatStrategy
{
    public void eat(IEdible food)
    {
        if (food == null) return;

        food.OnEaten();
    }
}
