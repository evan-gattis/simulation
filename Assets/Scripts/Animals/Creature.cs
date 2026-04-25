using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

abstract public class Creature : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshComponent;
    IEatStrategy eatStrategy;
    ITargetStrategy targetStrategy;
    private CreatureState currentState;
    Coroutine idleCoroutine;
    Coroutine targetCoroutine;
    float idleWanderRadius = 5f;
    float idleWanderInterval = 3f;
    float targetInterval = 1f;
    float checkStateInterval = 0.5f;
    protected float starveTime = 30f;
    float currentHungerTime;
    private bool isDead = false;
    IEnumerator WanderRoutine()
    {
        while (true)
        {
            SetNewRandomDestination();

            yield return new WaitUntil(() =>
                !navMeshComponent.pathPending &&
                navMeshComponent.remainingDistance <= navMeshComponent.stoppingDistance
            );

            if(navMeshComponent == null || !navMeshComponent.isActiveAndEnabled)
            {
                yield break;
            }

            yield return new WaitForSeconds(idleWanderInterval);
        }
    }
    IEnumerator TargetRoutine()
    {
        while (true)
        {
            if(targetStrategy.CurrentTarget != null)
            {
                if (NavMesh.SamplePosition(targetStrategy.CurrentTarget.transform.position,
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
        currentHungerTime = starveTime;
    }
    private void Update()
    {
        currentHungerTime -= Time.deltaTime;
        if(currentHungerTime <= 0f)
        {
            Die();
        }
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
        if (isDead) return; //Destroy() is called at the end of every frame, so need to check if we are dead.
        eatStrategy.eat(food);
        currentHungerTime = starveTime;
    }
    private void Move(Vector3 target)
    {
        navMeshComponent.SetDestination(target);
    }
    private void checkState()
    {
        if(this.targetStrategy == null) return;
        if(isDead) return;
        targetStrategy.FindTargetIfAny(gameObject);
        GameObject currentTarget = targetStrategy.CurrentTarget;
        if (currentTarget == null)
        {
            currentState = CreatureState.Idle;
        }
        else
        {
            currentState = CreatureState.Targeting;
            if (eatStrategy != null && eatStrategy.CanEat(gameObject, currentTarget))
            {
                var edible = currentTarget.GetComponent<IEdible>();
                if (edible != null && edible.IsAvailable)
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
    protected void Die()
    {
        if (isDead) return;
        isDead = true;
        StopAllCoroutines();
        CancelInvoke();
        if(navMeshComponent != null && navMeshComponent.isActiveAndEnabled)
        {
            navMeshComponent.isStopped = true;
            navMeshComponent.enabled = false;
        }
        Destroy(this.gameObject);
    }
    public bool IsDead => isDead;
    public bool IsAvailable => !isDead;
    public GameObject GetCurrentTarget()
    {
        return targetStrategy?.CurrentTarget;
    }
}

public enum CreatureState
{
    Idle,
    Targeting
}
