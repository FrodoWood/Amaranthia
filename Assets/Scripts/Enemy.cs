using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Attacking,
    Patrolling,
    Chasing,
    Dead,
    Ability1,
    Ability2
}
public abstract class Enemy : MonoBehaviour, IDamageable
{
    public EnemyState currentState;

    protected string enemyName;
    public EnemyData enemyData;
    protected Transform player;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Collider myCollider;


    [SerializeField] protected float currentHealth;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float damage;
    [SerializeField] protected float visionRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected bool canAttack;
    protected Vector3 lastKnownPlayerPosition;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();

    }
    protected virtual void Start()
    {
        ChangeState(EnemyState.Patrolling);
        
        // Setting scriptable object enemy data values
        enemyName = enemyData.name;
        maxHealth = enemyData.maxHealth;
        damage = enemyData.damage;
        agent.speed = enemyData.moveSpeed;
        
        currentHealth = maxHealth;
        visionRange = enemyData.visionRange;
    }

    protected virtual void ChangeState(EnemyState newState)
    {
        OnExitState(currentState);
        currentState = newState;
        OnEnterState(currentState);
    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdle();
                break;
            case EnemyState.Attacking:
                UpdateAttacking();
                break;
            case EnemyState.Patrolling:
                UpdatePatrolling();
                break;
            case EnemyState.Chasing:
                UpdateChasing();
                break;
            case EnemyState.Dead:
                UpdateDead();
                break;
            case EnemyState.Ability1:
                UpdateAbility1();
                break;
            case EnemyState.Ability2:
                UpdateAbility2();
                break;
        }
    }
    protected abstract void UpdateIdle();
    protected abstract void UpdateAttacking();
    protected abstract void UpdatePatrolling();
    protected abstract void UpdateChasing();
    protected abstract void UpdateDead();
    protected abstract void UpdateAbility1();
    protected abstract void UpdateAbility2();

    

    protected virtual void OnEnterState(EnemyState currentState)
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                OnEnterIdle();
                break;
            case EnemyState.Attacking:
                OnEnterAttacking();
                break;
            case EnemyState.Patrolling:
                OnEnterPatrolling();
                break;
            case EnemyState.Chasing:
                OnEnterChasing();
                break;
            case EnemyState.Dead:
                OnEnterDead();
                break;
            case EnemyState.Ability1:
                OnEnterAbility1();
                break;
            case EnemyState.Ability2:
                OnEnterAbility1();
                break;
        }
    }

    protected abstract void OnEnterDead();
    protected virtual void OnEnterChasing()
    {
        agent.SetDestination(player.position);
        lastKnownPlayerPosition = player.position;
    }
    protected abstract void OnEnterPatrolling();
    protected abstract void OnEnterAttacking();
    protected abstract void OnEnterIdle();
    protected abstract void OnEnterAbility1();
    protected abstract void OnEnterAbility2();

    protected virtual void OnExitState(EnemyState currentState)
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                OnExitIdle();
                break;
            case EnemyState.Attacking:
                OnExitAttacking();
                break;
            case EnemyState.Patrolling:
                OnExitPatrolling();
                break;
            case EnemyState.Chasing:
                OnExitChasing();
                break;
            case EnemyState.Dead:
                OnExitDead();
                break;
            case EnemyState.Ability1:
                OnExitAbility1();
                break;
            case EnemyState.Ability2:
                OnExitAbility2();
                break;
        }
    }

    protected abstract void OnExitDead();
    protected abstract void OnExitChasing();
    protected abstract void OnExitPatrolling();
    protected abstract void OnExitAttacking();
    protected abstract void OnExitIdle();
    protected abstract void OnExitAbility1();
    protected abstract void OnExitAbility2();

    public virtual void TakeDamage(float amount, EntityType entityType)
    {
        if (entityType == EntityType.Enemy) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log($"{enemyName} has taken {amount} damage!");

        if (currentHealth == 0)
        {
            ChangeState(EnemyState.Dead);
        }
    }

    public virtual void AttackPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            Debug.Log($"{enemyName} has attacked the player!");
            IDamageable damageable = player.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage, EntityType.Enemy);
            }
        }
    }

    protected virtual float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    protected virtual bool PlayerInRange()
    {
        return DistanceToPlayer() <= visionRange;
    }

    protected IEnumerator ChangeStateAfter(EnemyState newState,float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ChangeState(newState);
    }
}
