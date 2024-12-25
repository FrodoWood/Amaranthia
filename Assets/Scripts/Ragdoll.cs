using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ragdoll : MonoBehaviour, IRagdoll
{
    Rigidbody[] rigidbodies;
    Collider[] colliders;
    Animator animator;
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            PhysicsMaterial material = new PhysicsMaterial();
            material.dynamicFriction = 2f;
            material.staticFriction = 2f;
            material.bounciness = 0f;
            collider.material = material;
        }
        animator = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        foreach(var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
        animator.enabled = true;
    }
    public void ActivateRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }
        animator.enabled = false;
    }

    public void Explode(Vector3 forceDirection, ForceMode forceMode)
    {
        foreach (var rigidbody in rigidbodies)
        {
            //forceDirection = Random.onUnitSphere;
            float forceMagnitude = Random.Range(35f, 50f);
            rigidbody.AddForce(forceDirection * forceMagnitude, forceMode);
            //rigidbody.drag = 1.2f;
        }
    }

}
