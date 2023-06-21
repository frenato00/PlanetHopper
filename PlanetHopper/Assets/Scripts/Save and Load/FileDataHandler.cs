using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFilePath = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFilePath = Path.Combine(dataDirPath, dataFileName);
    }

    public GameData LoadData(){
        string fullPath = Path.Combine(dataDirPath, dataFilePath);
        GameData gameData = null;

        if(File.Exists(fullPath)){
            try{
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open)){
                    using (StreamReader reader = new StreamReader(stream)){
                        dataToLoad = reader.ReadToEnd();

                    }
                }
                gameData = JsonUtility.FromJson<GameData>(dataToLoad);  
            }catch(Exception e){
                Debug.LogError("Error loading file: " + e.Message);
            }
        }
        return gameData;
    }

    public void Save(GameData gameData){
        string fullPath = Path.Combine(dataDirPath, dataFilePath);
        try{
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(gameData, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create)){
                using (StreamWriter writer = new StreamWriter(stream)){
                    writer.Write(dataToStore);
                }
            }

        }catch(Exception e){
            Debug.LogError("Error saving file: " + e.Message);
        }
    }


}