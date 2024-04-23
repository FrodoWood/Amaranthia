using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private string fileName;
    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> saveables;
    public static SaveLoadManager instance { get; private set; }
    public static event Action OnGameSave;
    public static event Action OnNewGame;

    public void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.LogError("Found more than one SaveLoadManager");
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //if(saveables.Count != 0) SaveGame();
        Debug.Log("OnSceneLoaded: Scene has been loaded!");
        this.saveables = FindAllSaveables();
        LoadGame();
    }
    public void OnSceneUnloaded(Scene scene)
    {
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            SaveGame();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        this.gameData.playerPosition = new Vector3(28.53f, 0f, -357.6f);
        OnNewGame?.Invoke();
    }

    public void LoadGame()
    {
        // Load saved game
        this.gameData = dataHandler.Load();

        // if saved data cannot be loaded, then create a new game
        if(this.gameData == null)
        {
            Debug.Log("No save data was found, starting new game.");
            return;
        }

        // Give loaded data to all other scripts
        foreach (ISaveable saveable in saveables)
        {
            saveable.LoadData(gameData);
        }
    }

    [ContextMenu("SaveGame")]
    public void SaveGame()
    {
        if(this.gameData == null)
        {
            Debug.LogWarning("No data found.");
            return;
        }
        // give gameData to other scripts so they can update it
        foreach (ISaveable saveable in saveables)
        {
            saveable.SaveData(ref gameData);
        }
        // save data to file
        dataHandler.Save(gameData);
        // Invoke OnGameSave event so that remote highscore can be updated 
        OnGameSave?.Invoke();
        Debug.Log("OnGameSave event evoked!");
    }

    private List<ISaveable> FindAllSaveables()
    {
        IEnumerable<ISaveable> saveables = FindObjectsOfType<MonoBehaviour>()
            .OfType<ISaveable>();
        return new List<ISaveable>(saveables);
    }

    public bool GameDataExists()
    {
        return this.gameData != null;
    }
}
