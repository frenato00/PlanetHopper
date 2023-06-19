using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/Level Information")]
public class LevelInformation : ScriptableObject
{
    [Header("Level information")]
    public string levelName;
    public int levelNumber;

    [Header("Level milestones")]
    public int enemiesToKill;
    public int medalsToCollect;
    public int timeToBeBellow;

    [Header("Player achievements")]
    public int enemiesKilled=0;
    public int medalsCollected=0;
    public float timeReached=0;

}
