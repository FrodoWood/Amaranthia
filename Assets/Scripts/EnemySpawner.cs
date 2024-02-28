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

    void Start()
    {
        currentState = EnemySpawnerState.Idle;
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

    public void changeState(EnemySpawnerState newState) { currentState = newState; }

    [ContextMenu("Spawn Enemies")]
    public void SpawnEnemies()
    {
        foreach(var enemy in enemiesToSpawn)
        {
            if (enemy.prefab == null) return;
            for(int i = 0; i < enemy.amount; i++)
            {
                Instantiate(enemy.prefab, transform.position, Quaternion.identity);
            }
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
