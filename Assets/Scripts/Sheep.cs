using UnityEngine;
using UnityEngine.Rendering;

public class Sheep : Creature, IEdible
{
    void Awake()
    {
        SetEatStrategy(new HerbivoreEatStrategy());
    }
    public void OnEaten()
    {
        Debug.Log("Sheep was eaten");
        Destroy(gameObject);
    }
}
