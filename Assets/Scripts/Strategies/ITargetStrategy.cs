using UnityEngine;

public interface ITargetStrategy
{
    GameObject FindTargetIfAny(GameObject thisCreature);
    GameObject CurrentTarget { get; }
}
