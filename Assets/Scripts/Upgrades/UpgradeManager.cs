using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> upgrades = new List<Upgrade>();
    public PlayerStats playerStats;
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        
    }
}
