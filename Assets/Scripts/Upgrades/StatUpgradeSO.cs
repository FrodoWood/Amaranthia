using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Stat Upgrade")]
public class StatUpgradeSO : UpgradeSO
{
    public float cooldownReduction;
    public float abilityDamage;
    public float movementSpeed;
    public float maxHealth;
    public float healthRegen;
    public override void Initialise(PlayerStats _playerStats)
    {
        base.Initialise(_playerStats);
        playerStats.cooldownReduction += cooldownReduction;
        playerStats.abilityDamage += abilityDamage;
        playerStats.movementSpeed += movementSpeed;
        playerStats.maxHealth += maxHealth;
        playerStats.healthRegen += healthRegen;
    }

    public override void OnUpdate()
    {

    }
}
