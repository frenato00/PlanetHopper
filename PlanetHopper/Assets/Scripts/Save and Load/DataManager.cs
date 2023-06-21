using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header("File Storage Config")]
    public string dataFileName = "gameData.json";

    private GameData gameData;

    private FileDataHandler fileDataHandler;

    public LevelInformation level1;
    public LevelInformation level2;
    public LevelInformation level3;

    private void Awake(){

        DontDestroyOnLoad(this.gameObject);

        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    private void Start() {
        gameData = new GameData(level1, level2, level3);
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, dataFileName);
        LoadData();
    }

    public void LoadData(){
        gameData = fileDataHandler.LoadData();

        if(gameData == null){
            gameData = new GameData(level1, level2, level3);
        }else{
            LoadGameData();
        }

    }

    public void SaveData(){
        SaveGameData();
        this.fileDataHandler.Save(gameData);    
    }

    private void OnApplicationQuit() {
        SaveData();
    }

    private void LoadGameData(){
        Debug.Log("change");
        level1.enemiesKilled = gameData.level1EnemiesKilled;
        level1.medalsCollected = gameData.level1MedalsCollected;
        level1.timeReached = gameData.level1TimeReached;

        level2.enemiesKilled = gameData.level2EnemiesKilled;
        level2.medalsCollected = gameData.level2MedalsCollected;
        level2.timeReached = gameData.level2TimeReached;

        level3.enemiesKilled = gameData.level3EnemiesKilled;
        level3.medalsCollected = gameData.level3MedalsCollected;
        level3.timeReached = gameData.level3TimeReached;
    }

    private void SaveGameData(){
        gameData.level1EnemiesKilled = level1.enemiesKilled;
        gameData.level1MedalsCollected = level1.medalsCollected;
        gameData.level1TimeReached = level1.timeReached;

        gameData.level2EnemiesKilled = level2.enemiesKilled;
        gameData.level2MedalsCollected = level2.medalsCollected;
        gameData.level2TimeReached = level2.timeReached;

        gameData.level3EnemiesKilled = level3.enemiesKilled;
        gameData.level3MedalsCollected = level3.medalsCollected;
        gameData.level3TimeReached = level3.timeReached;
    }


}
