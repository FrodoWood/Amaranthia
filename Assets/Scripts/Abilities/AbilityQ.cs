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
    [SerializeField] private int fireballAmount;

    public override void TriggerAbility()
    {

        Debug.Log(abilityName + " ability used!");
        base.TriggerAbility(); // Starts the cooldown timer and sets the ability on cooldown
        Vector3 initialProjectileSpawnPointHolderRotation = player.projectileSpawnPointHolder.rotation.eulerAngles;
        Vector3 holderRotation = player.projectileSpawnPointHolder.rotation.eulerAngles;

        for (int i = 0; i < fireballAmount; i++)
        {
            int sign = i%2 == 0 ? 1 : -1;

            player.projectileSpawnPointHolder.Rotate(Vector3.up, 15 * sign * i);
            Vector3 spawnPos = player.projectileSpawnPoint.transform.position;
            GameObject newFireball = GameObject.Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
            Fireball fireball = newFireball.GetComponent<Fireball>();
            fireball?.Setup(damage);
            fireball.transform.forward = player.transform.forward;
            Rigidbody fireballRigidbody = fireball.GetComponent<Rigidbody>();
            Vector3 forceDir = player.projectileSpawnPoint.position - player.transform.position;
            forceDir = new Vector3(forceDir.x, 0, forceDir.z).normalized;
            fireballRigidbody?.AddForce(forceDir * fireballSpeed, ForceMode.Impulse);
            GameObject.Destroy(newFireball, 2f);
        }
        player.projectileSpawnPointHolder.localRotation = Quaternion.identity;

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
