using UnityEngine;

public class CarnivoreTargetStrategy : ITargetStrategy, IPreyObserver
{
    private float detectionRadius = 20f;
    private int edibleCreaturesLayer = 1 << LayerMask.NameToLayer("EdibleCreature");
    public GameObject FindTargetIfAny(GameObject thisCreature)
    {
        Collider[] hits = Physics.OverlapSphere(thisCreature.transform.position,
            detectionRadius,
            edibleCreaturesLayer
            );

        return GetClosestEdibleCreature(thisCreature.transform, hits);
    }

    public void Notify(IPreySubject subject)
    {

    }

    private GameObject GetClosestEdibleCreature(Transform thisTransform, Collider[] hits)
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach(Collider hit in hits)
        {
            float distance = Vector3.Distance(thisTransform.position, hit.transform.position);

            if (distance < minDistance) {
                minDistance = distance;
                closest = hit.gameObject;
            }
        }
        if(closest != null)
        {
            closest.GetComponent<IPreySubject>().Add(this);
        }
        return closest;
    }
}
