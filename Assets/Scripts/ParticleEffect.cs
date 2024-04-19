using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    private ParticleSystem[] collisionParticleSystems;

    private void Start()
    {
        collisionParticleSystems = GetComponentsInChildren<ParticleSystem>();
        PlayParticleSystem();
        Destroy(gameObject, 2f);
    }

    public void PlayParticleSystem()
    {
        foreach (ParticleSystem p in collisionParticleSystems)
        {
            p.Play();
        }
    }
}
