using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    PlayableDirector timelineDirector;
    public GameObject HUD;

    private void Awake()
    {
        timelineDirector = GetComponent<PlayableDirector>();
    }

    void Update()
    {
    }

    public void StartTimeline()
    {
        timelineDirector?.Play();
        HUD.SetActive(false);
        Debug.Log("Playing timelinne");
    }

    public void StopTimeline()
    {
        timelineDirector?.Stop();
    }

    private void OnDirectorStopped(PlayableDirector director)
    {
        HUD.SetActive(true);
    }

    private void OnEnable()
    {
        GameManager.OnNewGamePlayTimeline += StartTimeline;
        timelineDirector.stopped += OnDirectorStopped;
    }

    private void OnDisable()
    {
        GameManager.OnNewGamePlayTimeline -= StartTimeline;
        timelineDirector.stopped -= OnDirectorStopped;

    }
}
