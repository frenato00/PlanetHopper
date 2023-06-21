using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLevelInfo : MonoBehaviour
{
    public static MenuLevelInfo instance;

    public LevelInformation level;

    [SerializeField]
    private TMPro.TextMeshProUGUI levelTitle;

    [SerializeField]
    private TMPro.TextMeshProUGUI levelInfo;

    private int TIME_REACHED_DEFAULT = 99999;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(level.levelName);
    }

    public void SetLevel(LevelInformation level)
    {
        this.level = level;
        levelTitle.text = "Level " + level.levelNumber;
        levelInfo.text = "Enemies killed: " + level.enemiesKilled + "\nMedals collected: " + level.medalsCollected;

        if( level.timeReached == TIME_REACHED_DEFAULT)
        {
            levelInfo.text += "\nTime reached: --:--";
        }
        else
        {
            levelInfo.text += "\nTime reached: " + FormatTime(level.timeReached);
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetLevel(level);
    }
}
