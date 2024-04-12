using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileDataHandler
{
    private string filePath = "";
    private string fileName = "";

    public FileDataHandler(string filePath, string fileName)
    {
        this.filePath = filePath;
        this.fileName = fileName;
    }
    
    public GameData Load()
    {
        string fullPath = Path.Combine(filePath, fileName);
        GameData loadeddData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadeddData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        return loadeddData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(filePath, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize data to json
            string dataToStore = JsonUtility.ToJson(data,true);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"Error occured while saving: {e.Message} {e.StackTrace}");
        }
    }
}
