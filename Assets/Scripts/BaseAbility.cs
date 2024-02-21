using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility
{
    public string abilityName;
    public float cooldown;

    public abstract void TriggerAbility(PlayerController player);
}
