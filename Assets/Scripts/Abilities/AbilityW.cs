using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityW : BaseAbility
{
    [SerializeField] private string abilityName;
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 4f;
    [SerializeField] private GameObject explosionPrefab;

    public override void TriggerAbility()
    {
        Debug.Log(abilityName + " ability used!");
        base.TriggerAbility();

        Collider[] colliders = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Enemies"));
        foreach(Collider collider in colliders)
        {
            // Apply damage to each IDamageable 
            IDamageable damageable = collider.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage, EntityType.Allied);

            //Apply force to each ragdoll
            IRagdoll ragdoll = collider.gameObject.GetComponent<IRagdoll>();
            Vector3 directionToCollider = collider.transform.position - transform.position;
            ragdoll?.Explode(directionToCollider.normalized);
        }

    }

    public void InstantiateExplosionEffects()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, range);
    }

}
