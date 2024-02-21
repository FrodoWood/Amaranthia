using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAbility : BaseAbility
{
    public GameObject fireballPrefab;
    public FireballAbility()
    {
        abilityName = "Fireball";
        cooldown = 2f;

    }
    public override void TriggerAbility(PlayerController player)
    {
        Debug.Log(abilityName + " ability used!");

    }
}
