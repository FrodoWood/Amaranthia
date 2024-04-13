using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public UpgradeSO upgradeSO;
    
    public bool isActivated = false;

    public void Initialise(PlayerStats _playerStats)
    {
        upgradeSO.Initialise(_playerStats);
    }

    private void Update()
    {
        upgradeSO.OnUpdate();
    }

    public string GetName()
    {
        return upgradeSO.name;
    }
}
