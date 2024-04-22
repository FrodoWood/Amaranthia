using System.Collections;
using System.Collections.Generic;
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
    public AudioMixerGroup fireballMixerGroup;
    public EntityType fireballEntityType;

    void Start()
    {
        GameObject audio = new GameObject("fireballInitialSound");
        AudioSource source = audio.AddComponent<AudioSource>();
        source.clip = fireballInitialSound;
        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = fireballMixerGroup;
        source.Play();
        Destroy(audio, fireballInitialSound.length);
    }

    void Update()
    {
        Destroy(gameObject, 4f);
    }

    public void Setup(float fireballDamage)
    {
        damage = fireballDamage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(damage, fireballEntityType);
            Debug.Log("Damageable found!!!");
        }
        //damageable?.TakeDamage(damage, fireballEntityType);

        //Audio
        GameObject audio = new GameObject("fireballExplodeSound");
        AudioSource source = audio.AddComponent<AudioSource>();
        source.clip = fireballExplodeSound;
        source.spatialBlend = 0f;
        source.playOnAwake = false;
        //source.outputAudioMixerGroup = fireballMixerGroup;
        source.volume = explosionVolume;
        source.Play();
        Destroy(audio, fireballExplodeSound.length);

        GameObject collisionParticles = Instantiate(collisionParticlesPrefab, transform.position, Quaternion.identity);

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }


        IRagdoll ragdoll = collision.gameObject.GetComponent<IRagdoll>();
        if(ragdoll == null)
        {
            ragdoll = collision.gameObject.GetComponentInParent<IRagdoll>();
        }
        ragdoll?.Explode(transform.forward, ForceMode.Impulse);

        Destroy(gameObject);
    }
}
