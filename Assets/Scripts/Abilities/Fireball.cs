using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Audio;

public class Fireball : MonoBehaviour
{
    private float damage;
    public GameObject collisionParticlesPrefab;
    public AudioClip fireballInitialSound;
    public AudioClip fireballExplodeSound;
    [Range(0, 1f)]
    public float explosionVolume;    
    [Range(0, 1f)]
    public float fireballVolume;
    public AudioMixerGroup fireballMixerGroup;
    public EntityType fireballEntityType;
    private bool canExplode = false;
    public bool canGoThrough = false;

    void Start()
    {
        GameObject audio = new GameObject("fireballInitialSound");
        AudioSource source = audio.AddComponent<AudioSource>();
        source.clip = fireballInitialSound;
        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = fireballMixerGroup;
        source.volume = fireballVolume;
        source.Play();
        Destroy(audio, fireballInitialSound.length);
    }

    void Update()
    {
    }

    public void Setup(float fireballDamage)
    {
        damage = fireballDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Fireball>() != null)
        {
            canExplode = false;
        }

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, fireballEntityType);
            canExplode = true;
        }
        
        IRagdoll ragdoll = other.gameObject.GetComponent<IRagdoll>();
        if(ragdoll != null)
        {
            ragdoll.Explode(transform.forward, ForceMode.Impulse);
            canExplode = true;
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            canExplode = true;
        }

        if (canExplode && canGoThrough)
        {
            //Audio
            GameObject audio = new GameObject("fireballExplodeSound");
            AudioSource source = audio.AddComponent<AudioSource>();
            source.clip = fireballExplodeSound;
            source.spatialBlend = 0f;
            source.playOnAwake = false;
            source.volume = explosionVolume;
            source.Play();
            Destroy(audio, fireballExplodeSound.length);

            // Hit Particles
            GameObject collisionParticles = Instantiate(collisionParticlesPrefab, transform.position, Quaternion.identity);
        }

        else if (canExplode && !canGoThrough)
        {
            //Audio
            GameObject audio = new GameObject("fireballExplodeSound");
            AudioSource source = audio.AddComponent<AudioSource>();
            source.clip = fireballExplodeSound;
            source.spatialBlend = 0f;
            source.playOnAwake = false;
            source.volume = explosionVolume;
            source.Play();
            Destroy(audio, fireballExplodeSound.length);

            // Hit Particles
            GameObject collisionParticles = Instantiate(collisionParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }

        canExplode = false;
    }
}
