using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class ComplexEnemy : Enemy
{
    [SerializeField] private float patrolRadius = 20f;
    [SerializeField] private float perlinScale = 3f;
    private Vector3 spawnPosition;
    private Color chaseColour = new Color(1f, 0f, 0f, 0.3f);
    private Color patrolColour = new Color(0f, 1f, 0f, 0.3f);
    public static event Action onDeath;
    private GameObject gemPrefab;
    public bool hasRagdoll = false;
    private Ragdoll ragdoll;
    private float smoothDampInjuredVelocity;

    protected override void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        base.Start();
        spawnPosition = transform.position + Random.insideUnitSphere * 2;
        canAttack = true;
        gemPrefab = enemyData.gemPrefab;
    }
    protected override void Update()
    {
        base.Update();
    }

    //IDLE
    protected override void OnEnterIdle()
    {
        Debug.Log($"{enemyName} Entered Idle");
        animator.SetTrigger("Idle");
        agent.ResetPath();
        StartCoroutine(ChangeStateAfter(EnemyState.Patrolling, 2f));
    }
    protected override void UpdateIdle()
    {
        if (PlayerInRange() && !GameManager.instance.player.IsDead())
        {
            StopAllCoroutines();
            ChangeState(EnemyState.Chasing);
        }
    }
    protected override void OnExitIdle()
    {
        Debug.Log($"{enemyName} Exited Idle");
    }

    //PATROLLING
    protected override void OnEnterPatrolling()
    {
        Debug.Log($"{enemyName} Entered Patrolling");
        animator.SetTrigger("Patrolling");
        agent.isStopped = false;
        //agent.SetDestination(GetRandomNavMeshWayPoint(transform.position, 30f));
        //StartCoroutine(ChangeStateAfter(EnemyState.Idle, 2f));

    }
    protected override void UpdatePatrolling()
    {
        if (!agent.pathPending && (agent.remainingDistance - agent.stoppingDistance) < 0.5f)
        {
            agent.SetDestination(GetRandomNavMeshWayPoint(transform.position, patrolRadius));
            if (Random.value < 0f) ChangeState(EnemyState.Idle);
        }
        if (PlayerInRange() && !GameManager.instance.player.IsDead()) ChangeState(EnemyState.Chasing);
    }
    protected override void OnExitPatrolling()
    {
        Debug.Log($"{enemyName} Exited Patrolling");
        agent.ResetPath();
    }

    //ATTACKING
    protected override void OnEnterAttacking()
    {
        Debug.Log($"{enemyName} Entered Attacking");
        agent.ResetPath();
        // play attack animation
    }
    protected override void UpdateAttacking()
    {
        if (GameManager.instance.player.IsDead())
        {
            ChangeState(EnemyState.Patrolling);
            return;
        }
        if (canAttack && DistanceToPlayer() <= attackRange)
        {
            animator.SetTrigger("Attacking");
            //AttackPlayer();
            canAttack = false;
            StartCoroutine(ResetAttack());
        }

        if (PlayerInRange() && DistanceToPlayer() > attackRange)
        {
            ChangeState(EnemyState.Chasing);
            return;
        }

        if (!PlayerInRange())
        {
            ChangeState(EnemyState.Patrolling);
            return;
        }
    }
    protected override void OnExitAttacking()
    {

    }

    //CHASING
    protected override void OnEnterChasing()
    {
        base.OnEnterChasing();
        animator.SetTrigger("Chasing");
    }
    protected override void UpdateChasing()
    {
        if (PlayerInRange())
        {
            lastKnownPlayerPosition = player.position;
            agent.SetDestination(lastKnownPlayerPosition);

            if (DistanceToPlayer() <= attackRange)
            {
                ChangeState(EnemyState.Attacking);
                return;
            }
        }
        // If the player is out of range => the enemy should go to the last known position of the player and then switch to idle
        if (!PlayerInRange() && (agent.remainingDistance - agent.stoppingDistance) < 0.5f)
        {
            ChangeState(EnemyState.Idle);
            return;
        }


    }
    protected override void OnExitChasing()
    {

    }

    //DEAD
    protected override void OnEnterDead()
    {
        Debug.Log($"{enemyName} Entered Dead");
        onDeath?.Invoke();
        animator.SetTrigger("Dead");
        agent.enabled = false;
        myCollider.enabled = false;
        StopAllCoroutines();
        if (hasRagdoll)
        {
            ragdoll?.ActivateRagdoll();
        }

        GameObject newGem = Instantiate(gemPrefab, transform.position, Quaternion.identity);
        if (hasRagdoll) Destroy(gameObject, 10f);
        else Destroy(gameObject, 3f);
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

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
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
        Vector3 directionBias = (player.position - transform.position).normalized;
        Vector3 perlinDirection = new Vector3(x, 0f, z).normalized;
        return Vector3.Lerp(perlinDirection, directionBias, 0.4f);
    }

    private void OnDrawGizmosSelected()
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

    private void UpdateInjuredLayerWeight()
    {
        if (animator.layerCount == 1) return;
        int layerIndex = animator.GetLayerIndex("Injured");
        float targetWeight = 1 - currentHealth / maxHealth;
        float smoothTime = 0.2f;
        float currentWeight = animator.GetLayerWeight(layerIndex);
        float newWeight = Mathf.SmoothDamp(currentWeight, targetWeight, ref smoothDampInjuredVelocity, smoothTime);
        animator.SetLayerWeight(layerIndex, newWeight);
    }
}
