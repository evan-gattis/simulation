using UnityEngine;

public interface IEdible
{
    void OnEaten();
    bool IsAvailable { get; }
}
