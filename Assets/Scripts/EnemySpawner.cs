using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IDamageable
{
    [SerializeReference]
    private EnemySpawnerState currentState;


    public List<EnemyToSpawn> enemiesToSpawn;
    private bool isSpawning;
    private bool isIdleing;
    private bool isDestroyed;
    private bool isRespawning;

    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float idleTimer = 5f;
    [SerializeField] private float respawnTimer = 3f;
    [SerializeField] private float health = 50f;
    [SerializeField] private float destroyedTimer = 10f;

    private MeshRenderer meshRenderer;
    public Material idleMaterial;
    public Material spawningMaterial;
    public Material destroyedMaterial;
    public Material respawningMaterial;


    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        isSpawning = false;
        isIdleing = false;
        isDestroyed = false;
        isRespawning = false;
        changeState(EnemySpawnerState.Idle);
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemySpawnerState.Idle:
                
                break;

            case EnemySpawnerState.Spawning:
                
                break;

            case EnemySpawnerState.Respawning:

                break;
            case EnemySpawnerState.Destroyed:
                
                break;
            default:

                break;
        }
    }

    public void changeState(EnemySpawnerState newState) {
        onExitCurrentState();
        currentState = newState;
        OnEnterCurrentState();
    }

    private void OnEnterCurrentState()
    {
        switch (currentState)
        {
            case EnemySpawnerState.Idle:
                isIdleing = true;
                StartCoroutine(WaitSecondsAndChangeState(idleTimer, EnemySpawnerState.Spawning));
                meshRenderer.material = idleMaterial;
                break;

            case EnemySpawnerState.Spawning:
                isSpawning = true;
                StartCoroutine(SpawnEnemyAtIntervalAndChangeState(spawnInterval, EnemySpawnerState.Idle));
                meshRenderer.material = spawningMaterial;
                break;

            case EnemySpawnerState.Respawning:
                isRespawning = true;
                StartCoroutine(WaitSecondsAndChangeState(respawnTimer, EnemySpawnerState.Idle));
                meshRenderer.material = respawningMaterial;
                break;

            case EnemySpawnerState.Destroyed:
                isDestroyed = true;
                StopAllCoroutines();
                meshRenderer.material = destroyedMaterial;
                StartCoroutine(WaitSecondsAndChangeState(destroyedTimer, EnemySpawnerState.Respawning));

                break;

            default:
                break;
        }
    }

    private void onExitCurrentState()
    {
        switch (currentState)
        {
            case EnemySpawnerState.Idle:
                isIdleing = false;
                break;
            case EnemySpawnerState.Spawning:
                isSpawning = false;
                break;
            case EnemySpawnerState.Respawning:
                isRespawning = false;
                health = 50f;
                break;
            case EnemySpawnerState.Destroyed:
                isDestroyed = false;
                break;
            default:
                break;
        }
    }

    [ContextMenu("Spawn Enemies")]
    public void SpawnEnemies()
    {
        StartCoroutine(SpawnEnemyAtIntervalAndChangeState(spawnInterval, EnemySpawnerState.Idle));
    }
    private IEnumerator SpawnEnemyAtIntervalAndChangeState(float interval, EnemySpawnerState newState)
    {
        if (enemiesToSpawn != null)
        {
            foreach (var enemy in enemiesToSpawn)
            {
                for (int i = 0; i < enemy.amount; i++)
                {
                    Instantiate(enemy.prefab, transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(interval);
                }
            }
        }
        yield return new WaitForSeconds(interval);
        changeState(newState);
    }

    private IEnumerator WaitSecondsAndChangeState(float seconds, EnemySpawnerState newState)
    {
        yield return new WaitForSeconds(seconds);
        changeState(newState);
    }

    public void TakeDamage(float amount, EntityType entityType)
    {
        if (entityType == EntityType.Enemy) return;
        if(currentState != EnemySpawnerState.Respawning)
        {
            health -= amount;
            health = Mathf.Max(health, 0);
        }
        if (health <= 0 && currentState != EnemySpawnerState.Destroyed && currentState != EnemySpawnerState.Respawning)
        {
            changeState(EnemySpawnerState.Destroyed);
        }
    }
}


public enum EnemySpawnerState
{
    Idle,
    Spawning,
    Respawning,
    Destroyed
}

[System.Serializable]
public class EnemyToSpawn
{
    public GameObject prefab;
    public int amount;
}