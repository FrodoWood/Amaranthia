using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Destroy(gameObject, 4f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(10f);
        }

        Destroy(gameObject);
    }
}
