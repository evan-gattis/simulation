using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

abstract public class Creature : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshComponent;
    IEatStrategy eatStrategy;
    ITargetStrategy targetStrategy;
    private GameObject currentTarget;
    private CreatureState currentState;
    Coroutine idleCoroutine;
    Coroutine targetCoroutine;
    float idleWanderRadius = 5f;
    float idleWanderInterval = 3f;
    float targetInterval = 1f;
    float checkStateInterval = 0.5f;
    float eatRadius = 1.5f;
    IEnumerator WanderRoutine()
    {
        while (true)
        {
            SetNewRandomDestination();

            yield return new WaitUntil(() =>
                !navMeshComponent.pathPending &&
                navMeshComponent.remainingDistance <= navMeshComponent.stoppingDistance
            );

            yield return new WaitForSeconds(idleWanderInterval);
        }
    }
    IEnumerator TargetRoutine()
    {
        while (true)
        {
            if(currentTarget != null)
            {
                if (NavMesh.SamplePosition(currentTarget.transform.position,
                    out NavMeshHit hit,
                    5f,
                    NavMesh.AllAreas))
                {
                    Move(hit.position);
                }
            }
            yield return new WaitForSeconds(targetInterval);
        }
    }
    private void SetNewRandomDestination()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * idleWanderRadius;
        if(NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, idleWanderRadius, 1))
        {
            Move(hit.position);
        }
    }
    private void StartIdle()
    {
        if(idleCoroutine == null)
        {
            idleCoroutine = StartCoroutine(WanderRoutine());
        }
    }
    private void StartTarget()
    {
        if(targetCoroutine == null)
        {
            targetCoroutine = StartCoroutine(TargetRoutine());
        }
    }
    private void StopIdle()
    {
        if(idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine);
            idleCoroutine = null;
        }
    }
    private void StopTarget()
    {
        if(targetCoroutine != null)
        {
            StopCoroutine(targetCoroutine);
            targetCoroutine = null;
        }
    }
    private void Start()
    {
        InvokeRepeating(nameof(checkState), 0f, checkStateInterval);
        InvokeRepeating(nameof(doAction), 0f, checkStateInterval);
    }
    protected void SetEatStrategy(IEatStrategy strategy)
    {
        this.eatStrategy = strategy;
    }
    protected void SetTargetStrategy(ITargetStrategy strategy)
    {
        this.targetStrategy = strategy;
    }
    private void eat(IEdible food)
    {
        if (this.eatStrategy == null) return;
        eatStrategy.eat(food);
    }
    private void Move(Vector3 target)
    {
        navMeshComponent.SetDestination(target);
    }
    private void checkState()
    {
        if(this.targetStrategy == null) return;
        currentTarget = targetStrategy.FindTargetIfAny(gameObject);
        if (currentTarget == null)
        {
            currentState = CreatureState.Idle;
        }
        else
        {
            currentState = CreatureState.Targeting;
            float sqrDistance = (gameObject.transform.position - currentTarget.transform.position).sqrMagnitude;
            if (sqrDistance < eatRadius * eatRadius)
            {
                var edible = currentTarget.GetComponent<IEdible>();
                if (edible != null)
                {
                    eat(edible);
                }
            }
        }
    }
    private void doAction()
    {
        switch (currentState)
        {
            case CreatureState.Idle:
                StopTarget();
                StartIdle();
                break;
            case CreatureState.Targeting:
                StopIdle();
                StartTarget();
                break;
        }
    }
}

public enum CreatureState
{
    Idle,
    Targeting
}
