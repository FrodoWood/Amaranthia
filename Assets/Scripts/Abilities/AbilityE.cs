using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityE : BaseAbility
{
    [SerializeField] private string abilityName;
    [SerializeField] private float dashSpeed;

    public override void TriggerAbility()
    {
        Debug.Log(abilityName + " ability used!");
        base.TriggerAbility();
    }

    protected override void Update()
    {
        base.Update();
        UpdateStats();

        if (!Complete())
        {
            player.agent.Move(player.transform.forward * dashSpeed * Time.deltaTime);
        }
    }

    public void UpdateStats()
    {
        cooldown = baseCooldown * (1 - (playerStats.cooldownReduction / 100));
    }
}
