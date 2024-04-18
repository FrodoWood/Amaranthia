using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHighScoreManager : MonoBehaviour, ISaveable
{
    public int initialHighScore;
    public int newHighScore;
    public int score;
    private bool newHighScoreReached;
    public LevelsManager levelsManager;
    public bool realTimeRemoteHighScoreUpdate = false;
    private void Start()
    {
        initialHighScore = RemoteHighScoreManager.Instance.highScoreData.Score;
        newHighScoreReached = false;
    }

    private void Update()
    {
        score = (int)levelsManager.totalExp;

        if(score > initialHighScore && !newHighScoreReached)
        {
            // Trigger ONE OFF EVENT of BEATING HIGH SCORE
            newHighScore = score;
            RemoteHighScoreManager.Instance.SetHighScore(newHighScore);
            Debug.Log("ONE TIME EVENT! NEW HIGH SCORE REACHED.");
            newHighScoreReached = true;

            // Start updating the high score periodically
            //StartCoroutine(UpdateHighScorePeriodically(5));
        }

        if (newHighScoreReached && score > newHighScore)
        {
            newHighScore = score;
            // IF REAL-TIME SCORE SYNC is ON then update the remote score every time a new high score is reached (to avoid reaching 
            if (realTimeRemoteHighScoreUpdate)
            {
                RemoteHighScoreManager.Instance.SetHighScore(newHighScore);
            }
        }
    }

    private void UpdateRemoteHighScore()
    {
        if (newHighScore > RemoteHighScoreManager.Instance.highScoreData.Score)
        {
            RemoteHighScoreManager.Instance.SetHighScore(newHighScore);
        }
    }

    //private IEnumerator UpdateHighScorePeriodically(float seconds)
    //{
    //    RemoteHighScoreManager.Instance.SetHighScore(newHighScore);
    //    yield return new WaitForSeconds(seconds);
    //    while (true)
    //    {
    //        if(newHighScore > RemoteHighScoreManager.Instance.highScoreData.Score)
    //        {
    //            RemoteHighScoreManager.Instance.SetHighScore(newHighScore);
    //        }
    //        yield return new WaitForSeconds(seconds);
    //        Debug.Log("Checked for highscore update.");
    //    }

    //}

    public void LoadData(GameData data)
    {
        score = data.score;
    }

    public void SaveData(ref GameData data)
    {
        data.score = score;
    }

    private void OnEnable()
    {
        SaveLoadManager.OnGameSave += UpdateRemoteHighScore;
    }
    private void OnDisable()
    {
        SaveLoadManager.OnGameSave -= UpdateRemoteHighScore;
    }
}
