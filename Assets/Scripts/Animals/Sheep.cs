using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Sheep : Creature, IEdible, IPreyObservable
{
    private List<IPreyObserver> observers = new List<IPreyObserver>();
    private List<IPreyObserver> pendingAdd = new List<IPreyObserver>();
    private List<IPreyObserver> pendingRemove = new List<IPreyObserver>();
    void Awake()
    {
        SetEatStrategy(new HerbivoreEatStrategy());
        SetTargetStrategy(new HerbivoreTargetStrategy());
    }
    public void OnEaten()
    {
        Debug.Log("Sheep was eaten");
        Die();
    }
    void Update()
    {
        NotifyObservers();

        foreach (var o in pendingRemove)
            observers.Remove(o);

        foreach (var o in pendingAdd)
            observers.Add(o);

        pendingRemove.Clear();
        pendingAdd.Clear();
    }

    public void Add(IPreyObserver observer)
    {
        pendingAdd.Add(observer);
    }

    public void Remove(IPreyObserver observer)
    {
        pendingRemove.Add(observer);
    }

    private void NotifyObservers()
    {
        List<IPreyObserver> otherPredators = new List<IPreyObserver>(observers);
        foreach (IPreyObserver observer in otherPredators)
        {
            //Notifying predators of other predators tracking this sheep.
            observer.Notify(otherPredators);
        }
    }
}
