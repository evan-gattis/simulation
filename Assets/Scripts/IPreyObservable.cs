using System;
using UnityEngine;

public interface IPreyObservable
{
    void Add(IPreyObserver observer);
    void Remove(IPreyObserver observer);
}
