using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float maxHealth;
    public float moveSpeed;
    public float damage;
    public float visionRange;
}
