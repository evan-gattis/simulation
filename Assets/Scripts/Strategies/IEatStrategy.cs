using UnityEngine;

public interface IEatStrategy
{
    void eat(IEdible food);
    bool CanEat(GameObject eater, GameObject target);
}
