using UnityEngine;

abstract public class Resource : MonoBehaviour, IEdible
{
    private bool isConsumed = false;

    public bool IsAvailable => !isConsumed;

    public virtual void OnEaten()
    {
        if (isConsumed) return;
        isConsumed = true;
        Destroy(this.gameObject);
    }
}
