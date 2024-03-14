using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Upgrades/Cooldown Reduction Upgrade")]
public class CooldownReductionUpgrade : Upgrade
{
    public float cooldownReduction;
    public override void Initialise(PlayerStats _playerStats)
    {
        base.Initialise(_playerStats);
        playerStats.cooldownReduction += cooldownReduction;
    }

    public override void OnUpdate()
    {
        
    }
}
