using UnityEngine;

public class Wolf : Creature, IEdible
{
    public void OnEaten()
    {
        Debug.Log("Wolf was eaten by a competing wolf!");
        Die();
    }

    void Awake()
    {
        SetEatStrategy(new CarnivoreEatStrategy());
        var targetStrategy = new CarnivoreTargetStrategy();
        targetStrategy.OwnerCreature = gameObject;
        SetTargetStrategy(targetStrategy);
    }
}
