using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityR : BaseAbility
{
    [SerializeField] private string abilityName;
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float damage = 10f;
    GameObject newBeam;
    public AudioClip beamAudio;
    [Range(0, 1f)]
    public float beamAudioVolume;



    public override void TriggerAbility()
    {
        Debug.Log(abilityName + " ability used!");
        base.TriggerAbility();

        Quaternion rotation = Quaternion.LookRotation(player.transform.forward);
        newBeam = Instantiate(beamPrefab, player.projectileSpawnPoint.transform.position, rotation);
        Beam beam = newBeam.GetComponent<Beam>();
        beam?.Setup(damage, player);

        // Audio
        GameObject audio = new GameObject("beamAudio");
        AudioSource source = audio.AddComponent<AudioSource>();
        source.clip = beamAudio;
        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.volume = beamAudioVolume;
        source.Play();
        Destroy(audio, beamAudio.length);

    }

    protected override void Update()
    {
        base.Update();
        UpdateStats();
        if (player.CurrentAnimationFinished() && newBeam != null)
        {
            Destroy(newBeam);
        }
    }

    public void UpdateStats()
    {
        damage = baseDamage + playerStats.abilityDamage;
        cooldown = baseCooldown * (1 - (playerStats.cooldownReduction / 100));
    }
}
