using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityR : BaseAbility
{
    [SerializeField] private string abilityName;
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float damage = 10f;
    GameObject newBeam;


    public override void TriggerAbility()
    {
        Debug.Log(abilityName + " ability used!");
        base.TriggerAbility();

        Quaternion rotation = Quaternion.LookRotation(player.transform.forward);
        newBeam = Instantiate(beamPrefab, player.projectileSpawnPoint.transform.position, rotation);
        Beam beam = newBeam.GetComponent<Beam>();
        beam?.Setup(damage, player);

    }

    protected override void Update()
    {
        base.Update();
        UpdateStats();
        if (player.CurrentAnimationFinished() && newBeam != null)
        {
            Destroy(newBeam);
        }
    }

    public void UpdateStats()
    {
        damage = baseDamage + playerStats.abilityDamage;
        cooldown = baseCooldown * (1 - (playerStats.cooldownReduction / 100));
    }
}
