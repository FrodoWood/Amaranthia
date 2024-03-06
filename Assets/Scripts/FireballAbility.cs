using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAbility : BaseAbility
{
    private float fireballSpeed = 25f;

    public FireballAbility()
    {
        abilityName = "Fireball";
        cooldown = 2f;

    }
    public override void TriggerAbility(PlayerController player)
    {
        Debug.Log(abilityName + " ability used!");
        GameObject fireball = GameObject.Instantiate(player.fireballPrefab, player.transform.position + 1.04f * player.transform.forward, Quaternion.identity);
        Vector3 direction = player.transform.forward;
        Rigidbody fireballRigidbody = fireball.GetComponent<Rigidbody>();
        fireballRigidbody?.AddForce(direction * fireballSpeed, ForceMode.Impulse);
        GameObject.Destroy(fireball, 2f);
    }
}
