using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : Enemy
{
    [SerializeField] private float patrolRadius = 20f;
    [SerializeField] private float perlinScale = 3f;
    private Vector3 spawnPosition;
    private Color chaseColour = new Color(1f, 0f, 0f, 0.3f);
    private Color patrolColour = new Color(0f, 1f, 0f, 0.3f);

    protected override void Start()
    {
        base.Start();
        spawnPosition = transform.position;
    }
    protected override void Update()
    {
        base.Update();
    }
    

    protected override void OnEnterIdle()
    {
        Debug.Log($"{enemyName} Entered Idle");
        agent.ResetPath();
        StartCoroutine(ChangeStateAfter(EnemyState.Patrolling, 2f));
    }
    protected override void UpdateIdle()
    {
        if (PlayerInRange())
        {
            StopAllCoroutines();
            ChangeState(EnemyState.Chasing);
        }
    }
    protected override void OnExitIdle()
    {
        Debug.Log($"{enemyName} Exited Idle");
    }


    protected override void OnEnterPatrolling()
    {
        Debug.Log($"{enemyName} Entered Patrolling");
        agent.isStopped = false;
        //agent.SetDestination(GetRandomNavMeshWayPoint(transform.position, 30f));
        //StartCoroutine(ChangeStateAfter(EnemyState.Idle, 2f));

    }
    protected override void UpdatePatrolling()
    {
        if(!agent.pathPending && (agent.remainingDistance - agent.stoppingDistance) < 0.5f)
        {
            agent.SetDestination(GetRandomNavMeshWayPoint(transform.position, patrolRadius));
            if (Random.value < 0f) ChangeState(EnemyState.Idle);
        }
        if (PlayerInRange()) ChangeState(EnemyState.Chasing);
    }
    protected override void OnExitPatrolling()
    {
        Debug.Log($"{enemyName} Exited Patrolling");
        agent.ResetPath();
    }
    
    
    protected override void OnEnterAttacking()
    {

    }
    protected override void UpdateAttacking()
    {
        
    }
    protected override void OnExitAttacking()
    {

    }


    protected override void OnEnterChasing()
    {
        base.OnEnterChasing();
    }
    protected override void UpdateChasing()
    {
        if (PlayerInRange())
        {
            lastKnownPlayerPosition = player.position;
            agent.SetDestination(lastKnownPlayerPosition);
        }
        // If the player is out of range => the enemy should go to the last known position of the player and then switch to idle
        if (!PlayerInRange() && (agent.remainingDistance - agent.stoppingDistance) < 0.5f)
        {
            ChangeState(EnemyState.Idle);
        }

    }
    protected override void OnExitChasing()
    {

    }

    protected override void OnEnterDead()
    {
        Debug.Log($"{enemyName} Entered Dead");
        agent.enabled = false;
        StopAllCoroutines();
        Destroy(gameObject);
    }
    protected override void UpdateDead()
    {
        
    }
    protected override void OnExitDead()
    {

    }



    public override void TakeDamage(float amount, EntityType entityType)
    {
        base.TakeDamage(amount, entityType);
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
        return Vector3.Lerp(perlinDireciton, directionBias, 0.4f);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        switch (currentState)
        {
            case EnemyState.Chasing:
                Gizmos.color = chaseColour;
                break;
            case EnemyState.Patrolling:
                Gizmos.color = patrolColour;
                break;
            default:
                Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
                break;
        }
        Gizmos.DrawSphere(transform.position, visionRange);
    }
}
