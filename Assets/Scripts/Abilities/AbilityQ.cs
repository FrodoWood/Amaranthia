using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class AbilityQ : BaseAbility
{
    [SerializeField] private string abilityName;
    [SerializeField] private float fireballSpeed = 25f;
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject fireballPrefab;

    public override void TriggerAbility()
    {

        Debug.Log(abilityName + " ability used!");
        base.TriggerAbility(); // Starts the cooldown timer and sets the ability on cooldown
        GameObject newFireball = GameObject.Instantiate(fireballPrefab, player.transform.position + 1.04f * player.transform.forward, Quaternion.identity);
        Fireball fireball = newFireball.GetComponent<Fireball>();
        fireball?.Setup(damage, player);
        fireball.transform.forward = player.transform.forward;
        Rigidbody fireballRigidbody = fireball.GetComponent<Rigidbody>();
        fireballRigidbody?.AddForce(player.transform.forward * fireballSpeed, ForceMode.Impulse);
        GameObject.Destroy(newFireball, 2f);

    }

    protected override void Update()
    {
        base.Update();
        UpdateStats();
    }

    public void UpdateStats()
    {
        damage = baseDamage + playerStats.abilityDamage;
        cooldown = baseCooldown * (1 - (playerStats.cooldownReduction / 100));
    }
}
