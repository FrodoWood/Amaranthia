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
            // trigger one off event of beating high score!
            newHighScore = score;
            RemoteHighScoreManager.Instance.SetHighScore(newHighScore);
            Debug.Log("ONE TIME EVENT! NEW HIGH SCORE REACHED.");
            newHighScoreReached = true;

            // Start updating the high score periodically
            StartCoroutine(UpdateHighScorePeriodically(5));
        }

        if (newHighScoreReached && score > newHighScore)
        {
            newHighScore = score;
        }
    }


    private IEnumerator UpdateHighScorePeriodically(float seconds)
    {
        RemoteHighScoreManager.Instance.SetHighScore(newHighScore);
        yield return new WaitForSeconds(seconds);
        while (true)
        {
            if(newHighScore > RemoteHighScoreManager.Instance.highScoreData.Score)
            {
                RemoteHighScoreManager.Instance.SetHighScore(newHighScore);
            }
            yield return new WaitForSeconds(seconds);
            Debug.Log("Checked for highscore update.");
        }

    }

    public void LoadData(GameData data)
    {
        score = data.score;
    }

    public void SaveData(ref GameData data)
    {
        data.score = score;
    }
}
