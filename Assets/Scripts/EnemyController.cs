using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    public EnemyData enemyData;
    private Transform player;
    private NavMeshAgent agent;
    private string enemyName;
    private float currentHealth;
    private float maxHealth;
    private float damage;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        enemyName = enemyData.name;
        maxHealth = enemyData.maxHealth;
        currentHealth = maxHealth;
        damage = enemyData.damage;
        agent.speed = enemyData.moveSpeed;  

    }

    void Update()
    {
        if (agent.enabled)
        {
            ChasePlayer();
        }
        AttackPlayer();

    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void takeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log($"{enemyName} has taken {amount} damage!");

        if(currentHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    private void AttackPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if(distance <= 2f)
        {
            Debug.Log($"{enemyName} has attacked the player!");
        }
    }
}
