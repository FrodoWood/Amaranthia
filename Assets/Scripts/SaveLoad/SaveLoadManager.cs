using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private string fileName;
    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> saveables;
    public static SaveLoadManager instance { get; private set; }



    public void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one SaveLoadManager");
        }
        instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.saveables = FindAllSaveables();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // Load saved game
        this.gameData = dataHandler.Load();

        // if saved data cannot be loaded, then create a new game
        if(this.gameData == null)
        {
            Debug.Log("No save data was found, starting new game.");
            NewGame();
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
        // give gameData to other scripts so they can update it
        foreach (ISaveable saveable in saveables)
        {
            saveable.SaveData(ref gameData);
        }
        // save data to file
        dataHandler.Save(gameData);
    }

    private List<ISaveable> FindAllSaveables()
    {
        IEnumerable<ISaveable> saveables = FindObjectsOfType<MonoBehaviour>()
            .OfType<ISaveable>();
        return new List<ISaveable>(saveables);
    }
}
