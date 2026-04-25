using UnityEngine;
using System.Collections.Generic;

public class CarnivoreTargetStrategy : ITargetStrategy, IPreyObserver
{
    private float detectionRadius = 20f;
    private int edibleCreaturesLayer = 1 << LayerMask.NameToLayer("EdibleCreature");
    private GameObject currentTarget = null;
    private IPreyObservable currentObservable = null;
    private bool targetLockedByNotify = false;

    public GameObject CurrentTarget => currentTarget;

    public GameObject FindTargetIfAny(GameObject thisCreature)
    {
        //So we don't loop between other predators and the prey we want to target
        if (targetLockedByNotify)
        {
            if (currentTarget != null)
            {
                return currentTarget;
            }
            targetLockedByNotify = false;
        }

        Collider[] hits = Physics.OverlapSphere(thisCreature.transform.position,
            detectionRadius,
            edibleCreaturesLayer
            );

        GameObject closest = GetClosestEdibleCreature(thisCreature.transform, hits);

        //New target? Update observers
        if (closest != currentTarget)
        {
            //Unregister from old prey if needed
            if (currentObservable != null)
            {
                currentObservable.Remove(this);
                currentObservable = null;
            }

            currentTarget = closest;

            if (currentTarget != null)
            {
                currentObservable = currentTarget.GetComponent<IPreyObservable>();
                currentObservable?.Add(this);
            }
        }

        return currentTarget;
    }

    //Called when prey is being targeted by other predators.
    //Switch our target to a competing predator so the wolves will fight over prey
    public void Notify(List<IPreyObserver> otherPredators)
    {
        foreach (IPreyObserver other in otherPredators)
        {
            if (other == this) continue; //Don;t target ourselves

            //Make sure we are targeting another creature
            if (other is CarnivoreTargetStrategy otherStrategy && otherStrategy.OwnerCreature != null)
            {

                //Switching targets
                if (currentObservable != null)
                {
                    currentObservable.Remove(this);
                    currentObservable = null;
                }

                currentTarget = otherStrategy.OwnerCreature;
                targetLockedByNotify = true; //lock so we don't get into a loop switching between prey and competitor
                return;
            }
        }
    }

    //Getter so that other wolves can get our reference
    public GameObject OwnerCreature { get; set; }

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
        return closest;
    }
}
