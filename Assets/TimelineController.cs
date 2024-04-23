using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    PlayableDirector timelineDirector;

    private void Awake()
    {
        timelineDirector = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        
    }

    public void StartTimeline()
    {
        Debug.Log("Playing timelinne");
        timelineDirector?.Play();
    }

    public void StopTimeline()
    {
        timelineDirector?.Stop();
    }

    private void OnEnable()
    {
        GameManager.OnNewGamePlayTimeline += StartTimeline;
    }

    private void OnDisable()
    {
        GameManager.OnNewGamePlayTimeline -= StartTimeline;
    }
}
