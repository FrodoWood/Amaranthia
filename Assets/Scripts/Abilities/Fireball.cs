using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Fireball : MonoBehaviour
{
    private float damage;
    private PlayerController player;
    public GameObject collisionParticlesPrefab;
    public AudioClip fireballInitialSound;
    public AudioClip fireballExplodeSound;
    public AudioMixerGroup fireballMixerGroup;

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

    public void Setup(float fireballDamage, PlayerController playerController)
    {
        damage = fireballDamage;
        player = playerController;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Audio
        GameObject audio = new GameObject("fireballExplodeSound");
        AudioSource source = audio.AddComponent<AudioSource>();
        source.clip = fireballExplodeSound;
        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = fireballMixerGroup;
        source.Play();
        Destroy(audio, fireballExplodeSound.length);

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
}
