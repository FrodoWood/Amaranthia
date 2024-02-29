using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeReference]
    private EnemySpawnerState currentState;

    [System.Serializable]
    public class EnemyToSpawn
    {
        public GameObject prefab;
        public int amount;
    }

    public List<EnemyToSpawn> enemiesToSpawn;
    private bool isSpawning = false;
    private bool isIdleing = false;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float idleInterval = 5f;

    void Start()
    {
        currentState = EnemySpawnerState.Idle;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemySpawnerState.Idle:
                if (!isIdleing)
                {
                    isIdleing = true;
                    StartCoroutine(WaitSecondsAndChangeState(idleInterval, EnemySpawnerState.Spawning));
                }
                break;

            case EnemySpawnerState.Spawning:
                if (!isSpawning)
                {
                    isSpawning = true;
                    StartCoroutine(SpawnEnemyAtInterval(spawnInterval));
                    changeState(EnemySpawnerState.Idle);
                }
                break;

            case EnemySpawnerState.Respawning:

                break;
            case EnemySpawnerState.Destroyed:

                break;
            default:

                break;
        }
    }

    public void changeState(EnemySpawnerState newState) { currentState = newState; }

    [ContextMenu("Spawn Enemies")]
    public void SpawnEnemies()
    {
        StartCoroutine(SpawnEnemyAtInterval(spawnInterval));
    }
    private IEnumerator SpawnEnemyAtInterval(float interval)
    {
        foreach (var enemy in enemiesToSpawn)
        {
            for (int i = 0; i < enemy.amount; i++)
            {
                Instantiate(enemy.prefab, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(2);
            }
        }
        isSpawning = false;
    }

    private IEnumerator WaitSecondsAndChangeState(float seconds, EnemySpawnerState newState)
    {
        yield return new WaitForSeconds(seconds);
        changeState(newState);
        isIdleing = false;
    }
}


public enum EnemySpawnerState
{
    Idle,
    Spawning,
    Respawning,
    Destroyed
}
