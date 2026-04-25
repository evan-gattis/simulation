using UnityEngine;

public class HerbivoreEatStrategy : IEatStrategy
{
    private const float eatRadius = 1.5f;

    public void eat(IEdible food)
    {
        if (food == null) return;

        food.OnEaten();
    }

    public bool CanEat(GameObject eater, GameObject target)
    {
        if (eater == null || target == null) return false;
        float sqrDistance = (eater.transform.position - target.transform.position).sqrMagnitude;
        return sqrDistance < eatRadius * eatRadius;
    }
}
