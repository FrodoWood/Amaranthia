using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : Enemy
{
    [SerializeField] private float patrolRadius = 20f;
    [SerializeField] private float perlinScale = 3f;
    private Vector3 spawnPosition;

    protected override void Start()
    {
        base.Start();
        spawnPosition = transform.position;
    }
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }

    // OnEnter
    protected override void OnEnterAttacking()
    {

    }

    protected override void OnEnterChasing()
    {
        base.OnEnterChasing();
    }

    protected override void OnEnterDead()
    {
        Debug.Log($"{enemyName} Entered Dead");
        agent.enabled = false;
        StopAllCoroutines();
        Destroy(gameObject);
    }

    protected override void OnEnterIdle()
    {
        Debug.Log($"{enemyName} Entered Idle");
        StartCoroutine(ChangeStateAfter(EnemyState.Patrolling, 2f));
    }

    protected override void OnEnterPatrolling()
    {
        Debug.Log($"{enemyName} Entered Patrolling");
        agent.isStopped = false;
        //agent.SetDestination(GetRandomNavMeshWayPoint(transform.position, 30f));
        //StartCoroutine(ChangeStateAfter(EnemyState.Idle, 2f));

    }

    // Update
    protected override void UpdateAttacking()
    {
        
    }

    protected override void UpdateChasing()
    {
        
    }

    protected override void UpdateDead()
    {
        
    }

    protected override void UpdateIdle()
    {
        
    }

    protected override void UpdatePatrolling()
    {
        if(!agent.pathPending && (agent.remainingDistance - agent.stoppingDistance) < 0.5f)
        {
            agent.SetDestination(GetRandomNavMeshWayPoint(transform.position, patrolRadius));
            if (Random.value < 0f) ChangeState(EnemyState.Idle);
        }
    }

    // OnExit
    protected override void OnExitAttacking()
    {

    }

    protected override void OnExitChasing()
    {

    }

    protected override void OnExitDead()
    {

    }

    protected override void OnExitIdle()
    {
        Debug.Log($"{enemyName} Exited Idle");
    }

    protected override void OnExitPatrolling()
    {
        Debug.Log($"{enemyName} Exited Patrolling");
        agent.isStopped = true;
    }

    private Vector3 GetRandomNavMeshWayPoint(Vector3 centre, float radius)
    {
        //Vector3 randomDir = Random.insideUnitSphere * radius;
        //randomDir += centre;
        Vector3 randomDir = centre + GetRandomDirection() * radius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

    Vector3 GetRandomDirection()
    {
        float x = Mathf.PerlinNoise(Time.time * perlinScale, 0f) * 2f - 1f;
        float z = Mathf.PerlinNoise(0f, Time.time * perlinScale) * 2f - 1f;
        Vector3 directionBias = (spawnPosition - transform.position).normalized;
        Vector3 perlinDireciton =  new Vector3(x, 0f, z).normalized;
        return Vector3.Lerp(perlinDireciton, directionBias, 0.6f);
    }
}
