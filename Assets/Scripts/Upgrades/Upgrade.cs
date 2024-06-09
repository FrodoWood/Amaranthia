using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour, ISaveable
{
    public UpgradeSO upgradeSO;
    [SerializeField] private string id;
    public bool isActivated = false;

    public void Initialise(PlayerStats _playerStats)
    {
        upgradeSO.Initialise(_playerStats);
    }
    private void Awake()
    {

    }
    private void Update()
    {
        if (!isActivated) return;
        upgradeSO.OnUpdate();
    }

    public string GetName()
    {
        return upgradeSO.upgradeName;
    }

    public string GetDescription()
    {
        return upgradeSO.upgradeDescription;
    }

    public void LoadData(GameData data)
    {
        data.upgradesActivated.TryGetValue(id, out isActivated);
    }

    public void SaveData(ref GameData data)
    {
        if (data.upgradesActivated.ContainsKey(id))
        {
            data.upgradesActivated.Remove(id);
        }
        data.upgradesActivated.Add(id, isActivated);
    }
}
