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
    private Color chaseColour = new Color(1f, 0f, 0f, 0.3f);
    private Color patrolColour = new Color(0f, 1f, 0f, 0.3f);
    public static event Action onDeath;
    private GameObject gemPrefab;
    public bool hasRagdoll = false;
    private Ragdoll ragdoll;
    private float smoothDampInjuredVelocity;
    private AnimatorStateInfo animatorStateInfo;


    [Header("Abilities")]
    private WitchAbility1 ability1;
    private WitchAbility2 ability2;

    protected override void Awake()
    {
        base.Awake();
        ability1 = GetComponent<WitchAbility1>();
        ability2 = GetComponent<WitchAbility2>();
    }

    protected override void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        base.Start();
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

    //CHASING
    protected override void OnEnterChasing()
    {
        animator.SetTrigger("Chasing");
    }
    protected override void UpdateChasing()
    {
        if (PlayerInRange())
        {
            lastKnownPlayerPosition = player.position;
            agent.SetDestination(lastKnownPlayerPosition);
            Debug.Log("Last known player position:" + lastKnownPlayerPosition);

            if (DistanceToPlayer() <= attackRange && AnyAbilityAvailable())
            {
                ChangeState(PickRandomAvailableAbility());
                return;
            }
            
        }
        // If the player is out of range => the enemy should go to the last known position of the player and then switch to idle
        if (!PlayerInRange())
        {
            ChangeState(EnemyState.Idle);
            return;
        }


    }
    protected override void OnExitChasing()
    {

    }

    //ATTACKING
    protected override void OnEnterAttacking()
    {
        Debug.Log($"{enemyName} Entered Attacking");
        //agent.ResetPath();
        // pick which ability to use

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

    //ABILITY1
    protected override void OnEnterAbility1()
    {
        animator.SetTrigger("Ability1");
        ability1.TriggerAbility();
        agent.isStopped = true;
    }
    protected override void UpdateAbility1()
    {
        if (CurrentAnimationFinished())
        {
            ChangeState(EnemyState.Patrolling);
        }
    }
    protected override void OnExitAbility1()
    {
    }

    //ABILITY2
    protected override void OnEnterAbility2()
    {
        animator.SetTrigger("Ability1");
        ability1.TriggerAbility();
        agent.isStopped = true;
    }
    protected override void UpdateAbility2()
    {
        if (CurrentAnimationFinished())
        {
            ChangeState(EnemyState.Patrolling);
        }
    }
    protected override void OnExitAbility2()
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


    private bool AnyAbilityAvailable()
    {
        return ability1.Ready();
    }

    private EnemyState PickRandomAvailableAbility()
    {
        List<EnemyBaseAbility> enemyBaseAbilities = new List<EnemyBaseAbility>();
        enemyBaseAbilities.Add(ability1);
        enemyBaseAbilities.Add(ability2);
        foreach(EnemyBaseAbility enemyBaseAbility in enemyBaseAbilities)
        {
            if (!enemyBaseAbility.Ready())
            {
                enemyBaseAbilities.Remove(enemyBaseAbility);
            }
        }
        
        EnemyBaseAbility randomAbility = enemyBaseAbilities[Random.Range(0,enemyBaseAbilities.Count)];
        return randomAbility.GetAbilityState();

    }

    public bool CurrentAnimationFinished()
    {
        animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.normalizedTime >= 1 && !animator.IsInTransition(0)) return true;
        else return false;
    }

}
