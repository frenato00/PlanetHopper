using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int level1EnemiesKilled;
    public int level1MedalsCollected;
    public float level1TimeReached;

    public int level2EnemiesKilled;
    public int level2MedalsCollected;
    public float level2TimeReached;

    public int level3EnemiesKilled;
    public int level3MedalsCollected;
    public float level3TimeReached;

    public GameData(LevelInformation level1, LevelInformation level2, LevelInformation level3){
        level1EnemiesKilled = level1.enemiesKilled;
        level1MedalsCollected = level1.medalsCollected;
        level1TimeReached = level1.timeReached;

        level2EnemiesKilled = level2.enemiesKilled;
        level2MedalsCollected = level2.medalsCollected;
        level2TimeReached = level2.timeReached;

        level3EnemiesKilled = level3.enemiesKilled;
        level3MedalsCollected = level3.medalsCollected;
        level3TimeReached = level3.timeReached;
    }

    public GameData(){
        level1EnemiesKilled = 0;
        level1MedalsCollected = 0;
        level1TimeReached = 99999;

        level2EnemiesKilled = 0;
        level2MedalsCollected = 0;
        level2TimeReached = 99999;

        level3EnemiesKilled = 0;
        level3MedalsCollected = 0;
        level3TimeReached = 99999;
    }


}
