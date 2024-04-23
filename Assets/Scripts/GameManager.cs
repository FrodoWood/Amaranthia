using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Idle,
    NewGame,
    LoadedGame,
    NewHighScore,
    LevelUp,
    Defeat
} 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController player;

    public static event Action OnNewGamePlayTimeline;
    private TimelineController newSceneTimeline;
    private GameState currentState;
    public int maxEnemyCount = 60;
    public int currentEnemyCount;
    public bool isGamePaused = false;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        currentState = GameState.Idle;
    }

    private void Update()
    {
        OnUpdateState(currentState);

        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }

    }

    private void ChangeState(GameState newState)
    {
        currentState = newState;
        OnEnterState(currentState);
    }

    private void OnEnterState(GameState currentState)
    {
        switch (currentState)
        {
            case GameState.NewGame:
                Debug.Log("Game manager entered NEW GAME state");
                if (newSceneTimeline != null)
                {
                    newSceneTimeline.StartTimeline();
                    ChangeState(GameState.Idle);
                }
                break;
            case GameState.LoadedGame:
                break;
            case GameState.NewHighScore:
                break;
            case GameState.LevelUp:
                break;
            case GameState.Defeat:
                break;
            case GameState.Idle:
                break;
        }
    }

    private void OnUpdateState(GameState currentState)
    {
        switch (currentState)
        {
            case GameState.NewGame:
                if (newSceneTimeline == null)
                {
                    GameObject obj = GameObject.Find("NewSceneTimeline");
                    if (obj != null)
                    {
                        newSceneTimeline = obj.GetComponent<TimelineController>();
                    }
                }
                else
                {
                    newSceneTimeline.StartTimeline();
                    ChangeState(GameState.Idle);
                }
                break;
            case GameState.LoadedGame:
                break;
            case GameState.NewHighScore:
                break;
            case GameState.LevelUp:
                break;
            case GameState.Defeat:
                break;
            case GameState.Idle:
                break;
        }
    }

    private void OnEnable()
    {
        SaveLoadManager.OnNewGame += HandleNewGame;
    }
    private void OnDisable()
    {
        SaveLoadManager.OnNewGame -= HandleNewGame;
    }

    private void HandleNewGame()
    {
        //OnNewGamePlayTimeline?.Invoke();
        ChangeState(GameState.NewGame);
    }
}
