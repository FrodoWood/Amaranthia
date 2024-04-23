using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UpgradeSO : ScriptableObject
{
    public string upgradeName = "Upgrade";
    public string upgradeDescription = "Description";
    protected PlayerStats playerStats;

    public virtual void OnStart()
    {
    }
    public virtual void Initialise(PlayerStats _playerStats)
    {
        playerStats = _playerStats;
    }
    public abstract void OnUpdate();

}
