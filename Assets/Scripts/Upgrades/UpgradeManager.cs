using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> upgradesPool;

    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        upgradesPool = FindObjectsOfType<Upgrade>().ToList();
    }
    void Start()
    {
        foreach(Upgrade upgrade in upgradesPool)
        {
            if (upgrade.isActivated)
            {
                upgrade.Initialise(playerStats);
            }
        }
    }

    void Update()
    {
    }

    public void ActivateUpgrade(Upgrade upgrade)
    {
        if (!upgradesPool.Contains(upgrade)) return;
        if (upgrade.isActivated) return;
        upgrade.Initialise(playerStats);
        upgrade.isActivated = true;
    }
}
