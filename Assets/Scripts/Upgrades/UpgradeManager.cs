using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> upgradesPool = new List<Upgrade>();
    public List<Upgrade> activeUpgrades = new List<Upgrade>();
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }
    void Start()
    {
    }

    void Update()
    {
        foreach(Upgrade upgrade in activeUpgrades)
        {
            upgrade.OnUpdate();
        }
    }

    public void ActivateUpgrade(Upgrade _upgrade)
    {
        upgradesPool.Remove(_upgrade);
        activeUpgrades.Add(_upgrade);
        _upgrade.Initialise(playerStats);
    }
}
