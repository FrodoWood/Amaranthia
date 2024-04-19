using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityW : BaseAbility
{
    [SerializeField] private string abilityName;

    public override void TriggerAbility()
    {
        Debug.Log(abilityName + " ability used!");
        base.TriggerAbility();

        Collider[] colliders = Physics.OverlapSphere(transform.position, 4f, LayerMask.GetMask("Enemies"));
        foreach(Collider collider in colliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            damageable?.TakeDamage(40f, EntityType.Allied);
        }
    }

    private void CheckForDamageable()
    {

    }
}
