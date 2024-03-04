using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    public override void takeDamage(float amount)
    {
        Debug.Log("I have been hurt");
    }

    // OnEnter
    protected override void OnEnterAttacking()
    {

    }

    protected override void OnEnterChasing()
    {

    }

    protected override void OnEnterDead()
    {

    }

    protected override void OnEnterIdle()
    {

    }

    protected override void OnEnterPatrolling()
    {

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
        
    }

    protected override void OnExitPatrolling()
    {
        
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
        
    }
}
