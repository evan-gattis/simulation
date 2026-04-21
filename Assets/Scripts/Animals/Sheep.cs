using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Sheep : Creature, IEdible, IPreySubject
{
    private List<IPreyObserver> observers = new List<IPreyObserver>();
    void Awake()
    {
        SetEatStrategy(new HerbivoreEatStrategy());
    }
    public void OnEaten()
    {
        Debug.Log("Sheep was eaten");
        NotifyObservers();
        Destroy(gameObject);
    }

    public void Add(IPreyObserver observer)
    {
        observers.Add(observer);
    }

    public void Remove(IPreyObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (IPreyObserver observer in observers)
        {
            observer.Notify(this);
        }
    }
}
