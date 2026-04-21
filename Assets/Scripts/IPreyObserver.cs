using UnityEngine;

public interface IPreyObserver
{
    //All monobehaviors have an Update method, so must change method name to Notify
    void Notify(IPreySubject subject);
}