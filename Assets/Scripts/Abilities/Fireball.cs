using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float damage;
    private PlayerController player;
    public GameObject collisionParticlesPrefab;
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
        GameObject collisionParticles = Instantiate(collisionParticlesPrefab, transform.position, Quaternion.identity);

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
        ragdoll?.Explode(transform.forward, ForceMode.Impulse);

        Destroy(gameObject);
    }

    //private IEnumerator ApplyExplosionForce()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    ragdoll?.Explode();
    //}
}
