using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityE : BaseAbility
{
    [SerializeField] private string abilityName;
    [SerializeField] private float dashSpeed;
    public AudioClip dashAudio;
    [Range(0, 1f)]
    public float dashAudioVolume;

    public override void TriggerAbility()
    {
        Debug.Log(abilityName + " ability used!");
        base.TriggerAbility();

        // Audio
        GameObject audio = new GameObject("dashAudio");
        AudioSource source = audio.AddComponent<AudioSource>();
        source.clip = dashAudio;
        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.volume = dashAudioVolume;
        source.Play();
        Destroy(audio, dashAudio.length);
    }

    protected override void Update()
    {
        base.Update();
        UpdateStats();

        if (!Complete())
        {
            player.agent.Move(player.transform.forward * dashSpeed * Time.deltaTime);
        }
    }

    public void UpdateStats()
    {
        cooldown = baseCooldown * (1 - (playerStats.cooldownReduction / 100));
    }
}
