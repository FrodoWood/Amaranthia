using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityR : BaseAbility
{
    [SerializeField] private string abilityName;

    public override void TriggerAbility()
    {
        Debug.Log(abilityName + " ability used!");
        base.TriggerAbility();
    }
}
