using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Stat Upgrade")]
public class StatUpgrade : Upgrade
{
    public float cooldownReduction;
    public float abilityDamage;
    public float movementSpeed;
    public override void Initialise(PlayerStats _playerStats)
    {
        base.Initialise(_playerStats);
        playerStats.cooldownReduction += cooldownReduction;
        playerStats.abilityDamage += abilityDamage;
        playerStats.movementSpeed += movementSpeed;
    }

    public override void OnUpdate()
    {

    }
}
