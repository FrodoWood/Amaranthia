using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour, ISaveable
{
    public int baseLevel = 1;
    public int currentLevel;
    public float currentExp = 0;
    public float totalExp;
    public float maxExp = 100;
    public LevelsBar levelsBar;
    public static event Action OnLevelUp;
    private void Update()
    {
        if(currentExp >= maxExp) IncreaseLevel();
        levelsBar.UpdateLevelsBar(maxExp, currentExp, currentLevel, totalExp);
    }

    private void Start()
    {
        //currentLevel = baseLevel;
        levelsBar.UpdateLevelsBar(maxExp, currentExp, currentLevel, totalExp);
    }

    public void LoadData(GameData data)
    {
        this.currentLevel = data.currentLevel;
        this.currentExp = data.currentExp;
        this.totalExp = data.totalExp;
    }

    public void SaveData(ref GameData data)
    {
        data.currentLevel = this.currentLevel;
        data.currentExp = (int)this.currentExp;
        data.totalExp = (int)this.totalExp;
    }

    public void IncreaseLevel()
    {
        currentLevel += 1;
        currentExp = 0f;
        OnLevelUp?.Invoke();
    }

    public void AddExp(float _exp)
    {
        currentExp += _exp;
        totalExp += _exp;
    }

    private void OnEnable()
    {
        SimpleEnemy.onDeath += HandleEnemyDeath;
        ComplexEnemy.onDeath += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        SimpleEnemy.onDeath -= HandleEnemyDeath;
        ComplexEnemy.onDeath -= HandleEnemyDeath;
    }

    public void HandleEnemyDeath()
    {
        AddExp(5);
    }

    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.gameObject.GetComponent<ICollectable>();
        if (collectable != null)
        {
            collectable.OnCollect(this);
        }
    }
}
