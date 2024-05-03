using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public float transitionDuration = 2f;
    public List<AudioClip> audioClips = new List<AudioClip>();
    public List<AudioSource> audioSources = new List<AudioSource>();


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach(AudioClip clip in audioClips)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.playOnAwake = false;
            source.loop = true;
            audioSources.Add(source);

        }
    }



    private void Start()
    {
        audioSources[0].Play();
    }

    private void Update()
    {

    }

    public void SmoothTransition(string clipName)
    {
        AudioClip newClip = audioClips.Find(clip => clip.name == clipName);
        if(newClip == null)
        {
            Debug.LogWarning("Audio Clip not found in the audio source");
            return;
        }

        StartCoroutine(TransitionAudio(newClip));
    }

    private IEnumerator TransitionAudio(AudioClip newClip)
    {
        AudioSource currentSource = GetCurrentPlayingSource();
        if (currentSource == null)
        {
            audioSources[0].Play();
            audioSources[0].volume = 1.0f;
            yield break;
        }
        AudioSource newSource = audioSources.Find(source => source.clip == newClip);

        if(newSource == currentSource) yield break;

        float timer = 0f;
        float initialVolume = currentSource.volume;
        newSource.Play();
        while(timer < transitionDuration)
        {
            float t = timer / transitionDuration;
            currentSource.volume = Mathf.Lerp(initialVolume, 0f, t);
            newSource.volume = Mathf.Lerp(0f, 0.5f, t);
            timer += Time.deltaTime;
            yield return null;
        }
        currentSource.Stop();
    }

    public void PlayClipOnce(string clipName)
    {
        AudioClip newClip = audioClips.Find(clip => clip.name == clipName);
        AudioSource newSource = audioSources.Find(source => source.clip == newClip);
        newSource.loop = false;
        newSource.volume = 1;
        newSource.Play();
        //StartCoroutine(StopAudioAfter(newSource,newClip.length));
    }

    private IEnumerator StopAudioAfter(AudioSource audioSource, float time)
    {
        yield return new WaitForSeconds(time);
        audioSource.Stop();
    }

    private AudioSource GetCurrentPlayingSource()
    {
        foreach(AudioSource source in audioSources)
        {
            if (source.isPlaying) return source;
        }
        return null;
    }

    [ContextMenu("SwitchTheme")]
    public void SwitchToCombatTheme()
    {
        SmoothTransition("theme_combat_02");
    }
    
    public void SwitchToMainTheme()
    {
        SmoothTransition("theme_main_01");
    }
    
    [ContextMenu("PlayWinSound")]
    public void PlayWinSound()
    {
        PlayClipOnce("sound_win_01");
    }

    private void OnEnable()
    {
        LevelsManager.OnLevelUp += PlayWinSound;
        PlayerController.OnHighHealth += SwitchToMainTheme;
        PlayerController.OnLowHealth += SwitchToCombatTheme;
        PlayerController.OnPlayerDeath += SwitchToCombatTheme;
    }

    private void OnDisable()
    {
        LevelsManager.OnLevelUp -= PlayWinSound;
        PlayerController.OnHighHealth -= SwitchToMainTheme;
        PlayerController.OnLowHealth -= SwitchToCombatTheme;
        PlayerController.OnPlayerDeath -= SwitchToCombatTheme;

    }
}
