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
        SetLevel(level);
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(level.levelName);
    }

    public void SetLevel(LevelInformation level)
    {
        this.level = level;
        levelTitle.text = "Level " + level.levelNumber;
        levelInfo.text = "Enemies killed: " + level.enemiesKilled + "\nMedals collected: " + level.medalsCollected + "\nTime reached: " + level.timeReached;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
