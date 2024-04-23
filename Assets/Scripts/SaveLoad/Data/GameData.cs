using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentLevel;
    public int score;
    public int currentExp;
    public int totalExp;
    public float currentHealth;
    public float maxHealth;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> upgradesActivated;

    public GameData()
    {
        this.currentLevel = 1;
        this.score = 0;
        this.currentExp = 0;
        this.totalExp = 0;
        this.currentHealth = 100;
        this.maxHealth = 100;
        this.playerPosition = Vector3.zero;
        this.upgradesActivated = new SerializableDictionary<string, bool>();
    }
}
