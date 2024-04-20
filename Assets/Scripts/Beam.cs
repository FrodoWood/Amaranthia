using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private float damage;
    private PlayerController player;
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void Setup(float beamDamage, PlayerController playerController)
    {
        damage = beamDamage;
        player = playerController;
    }

    private void OnCollisionEnter(Collision collision)
    {

        //IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        //damageable?.TakeDamage(damage, EntityType.Allied);


        //IRagdoll ragdoll = collision.gameObject.GetComponent<IRagdoll>();
        //if (ragdoll == null)
        //{
        //    ragdoll = collision.gameObject.GetComponentInParent<IRagdoll>();
        //}
        //ragdoll?.Explode(transform.forward);

        //Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(damage * Time.deltaTime, EntityType.Allied);

        IRagdoll ragdoll = other.gameObject.GetComponent<IRagdoll>();
        if (ragdoll == null)
        {
            ragdoll = other.gameObject.GetComponentInParent<IRagdoll>();
        }
        ragdoll?.Explode(transform.forward *0.5f, ForceMode.Force);
    }

    //private IEnumerator ApplyExplosionForce()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    ragdoll?.Explode();
    //}
}
