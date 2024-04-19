using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    private ParticleSystem[] collisionParticleSystems;
    public float duration;

    private void Start()
    {
        collisionParticleSystems = GetComponentsInChildren<ParticleSystem>();
        PlayParticleSystem();
        Destroy(gameObject, duration);
    }

    public void PlayParticleSystem()
    {
        foreach (ParticleSystem p in collisionParticleSystems)
        {
            p.Play();
        }
    }
}
