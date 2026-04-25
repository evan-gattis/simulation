using UnityEngine;

public class HerbivoreTargetStrategy : ITargetStrategy
{
    private float detectionRadius = 20f;
    private int edibleResourcesLayer = 1 << LayerMask.NameToLayer("EdibleResource");
    private GameObject currentTarget = null;

    public GameObject CurrentTarget => currentTarget;

    public GameObject FindTargetIfAny(GameObject thisCreature)
    {
        Collider[] hits = Physics.OverlapSphere(thisCreature.transform.position,
            detectionRadius,
            edibleResourcesLayer
            );

        currentTarget = GetClosestEdibleResource(thisCreature.transform, hits);
        return currentTarget;
    }

    private GameObject GetClosestEdibleResource(Transform thisTransform, Collider[] hits)
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            float distance = Vector3.Distance(thisTransform.position, hit.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = hit.gameObject;
            }
        }
        return closest;
    }
}
