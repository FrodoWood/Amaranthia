using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour, ISaveable
{
    public int baseLevel = 1;
    public int currentLevel;
    public float currentExp;
    public float totalExp;
    public float maxExp = 100;
    public LevelsBar levelsBar;
    private void Update()
    {
        if(currentExp >= maxExp) IncreaseLevel();
        levelsBar.UpdateLevelsBar(maxExp, currentExp, currentLevel);
    }

    private void Start()
    {
        //currentLevel = baseLevel;
        currentExp = 0;
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
    }

    public void AddExp(float _exp)
    {
        currentExp += _exp;
        totalExp += _exp;
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

    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.gameObject.GetComponent<ICollectable>();
        if (collectable != null)
        {
            collectable.OnCollect(this);
        }
    }
}
