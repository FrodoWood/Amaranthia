using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    protected float visionRange;

    protected virtual void Start()
    {
        ChangeState(EnemyState.Idle);
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
    protected virtual void UpdateChasing()
    {

    }
    protected abstract void UpdateDead();

    protected virtual void ChangeState(EnemyState newState)
    {
        OnExitState(currentState);
        currentState = newState;
        OnEnterState(currentState);
    }

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
    protected abstract void OnEnterChasing();
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

    public abstract void takeDamage(float amount);
}
