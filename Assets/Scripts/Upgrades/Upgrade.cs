using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour, ISaveable
{
    public UpgradeSO upgradeSO;
    [SerializeField] private string id;
    public string upgradeCustomName;
    [ContextMenu("Generate GUID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public bool isActivated = false;

    public void Initialise(PlayerStats _playerStats)
    {
        upgradeSO.Initialise(_playerStats);
    }
    private void Awake()
    {
        if(id == "")
        {
            GenerateGuid();
        }
    }
    private void Update()
    {
        upgradeSO.OnUpdate();
    }

    public string GetName()
    {
        return upgradeSO.name;
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
