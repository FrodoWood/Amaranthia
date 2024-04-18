using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float damage;
    private PlayerController player;
    void Start()
    {
        
    }

    void Update()
    {
        Destroy(gameObject, 4f);
    }

    public void Setup(float fireballDamage, PlayerController playerController)
    {
        damage = fireballDamage;
        player = playerController;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(damage, EntityType.Allied);
        

        IRagdoll ragdoll = collision.gameObject.GetComponent<IRagdoll>();
        if(ragdoll == null)
        {
            ragdoll = collision.gameObject.GetComponentInParent<IRagdoll>();
        }
        ragdoll?.Explode(transform.forward);

        Destroy(gameObject);
    }

    //private IEnumerator ApplyExplosionForce()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    ragdoll?.Explode();
    //}
}
