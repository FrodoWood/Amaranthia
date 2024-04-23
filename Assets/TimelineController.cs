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
        // Check if the Timeline has finished playing
        if (timelineDirector.state == PlayState.Paused)
        {
            // Timeline has finished playing, do something
            Debug.Log("Timeline has finished playing");
            Destroy(gameObject);
        }
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
