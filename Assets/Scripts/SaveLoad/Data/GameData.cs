using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentLevel;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> upgradesActivated;

    public GameData()
    {
        this.currentLevel = 1;
        this.playerPosition = Vector3.zero;
        this.upgradesActivated = new SerializableDictionary<string, bool>();
    }
}
