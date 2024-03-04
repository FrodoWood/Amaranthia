using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : Enemy
{
    public override void TakeDamage(float amount)
    {
        Debug.Log("I have been hurt");
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

    }

    protected override void OnEnterIdle()
    {
        Debug.Log($"{enemyName} Entered Idle");
        StartCoroutine(ChangeStateAfter(EnemyState.Patrolling, 2f));
    }

    protected override void OnEnterPatrolling()
    {
        Debug.Log($"{enemyName} Entered Patrolling");
        agent.SetDestination(GetRandomNavMeshWayPoint(transform.position, 30f));
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
            agent.SetDestination(GetRandomNavMeshWayPoint(transform.position, 30f));
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

    }

    private Vector3 GetRandomNavMeshWayPoint(Vector3 centre, float radius)
    {
        Vector3 randomDir = Random.insideUnitSphere * radius;
        randomDir += centre;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }
}
