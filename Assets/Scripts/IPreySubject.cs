using System;
using UnityEngine;

public interface IPreySubject
{
    void Add(IPreyObserver observer);
    void Remove(IPreyObserver observer);
    void NotifyObservers();
}
