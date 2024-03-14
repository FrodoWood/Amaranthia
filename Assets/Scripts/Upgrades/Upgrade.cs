using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Upgrade : ScriptableObject
{
    public bool isActive = false;
    public PlayerStats playerStats;
    public virtual void Initialise(PlayerStats _playerStats)
    {
        playerStats = _playerStats;
    }
    public abstract void OnUpdate();

    public void Activate()
    {
        isActive = true;   
    }

}
