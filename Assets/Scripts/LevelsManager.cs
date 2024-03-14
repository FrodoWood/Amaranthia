using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    public int baseLevel = 1;
    public int currentLevel;
    public float currentExp;
    public float maxExp = 100;
    public LevelsBar levelsBar;
    private void Update()
    {
        if(currentExp >= maxExp) IncreaseLevel();
        levelsBar.UpdateLevelsBar(maxExp, currentExp, currentLevel);
    }

    private void Start()
    {
        currentLevel = baseLevel;
        currentExp = 0;
    }
    public void IncreaseLevel()
    {
        currentLevel += 1;
        currentExp = 0f;
    }

    public void AddExp(float _exp)
    {
        currentExp += _exp;
    }

    private void OnEnable()
    {
        SimpleEnemy.onDeath += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        SimpleEnemy.onDeath -= HandleEnemyDeath;
    }

    public void HandleEnemyDeath()
    {
        AddExp(5);
    }
}
