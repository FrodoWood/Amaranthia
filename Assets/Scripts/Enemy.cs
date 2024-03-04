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
    Dead
}
public abstract class Enemy : MonoBehaviour, IDamageable
{
    protected EnemyState currentState;

    protected string enemyName;
    public EnemyData enemyData;
    protected Transform player;
    protected NavMeshAgent agent;
    protected float currentHealth;
    protected float maxHealth;
    protected float damage;
    protected float visionRange;

    protected virtual void Start()
    {
        ChangeState(EnemyState.Idle);
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
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
        }
    }
    protected abstract void UpdateIdle();
    protected abstract void UpdateAttacking();
    protected abstract void UpdatePatrolling();
    protected abstract void UpdateChasing();
    protected abstract void UpdateDead();

    

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
        }
    }

    protected abstract void OnEnterDead();
    protected virtual void OnEnterChasing()
    {
        agent.SetDestination(player.position);
    }
    protected abstract void OnEnterPatrolling();
    protected abstract void OnEnterAttacking();
    protected abstract void OnEnterIdle();

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
        }
    }

    protected abstract void OnExitDead();
    protected abstract void OnExitChasing();
    protected abstract void OnExitPatrolling();
    protected abstract void OnExitAttacking();
    protected abstract void OnExitIdle();

    public virtual void TakeDamage(float amount)
    {
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
        if (distance <= 2f)
        {
            Debug.Log($"{enemyName} has attacked the player!");
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
