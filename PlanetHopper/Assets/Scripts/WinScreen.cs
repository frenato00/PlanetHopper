using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{

    [Header("Text Fields")]
    public TMPro.TextMeshProUGUI medalsText;
    public TMPro.TextMeshProUGUI timeText;
    public TMPro.TextMeshProUGUI enemiesText;

    public TMPro.TextMeshProUGUI levelNameText;

    [Header("New Records")]
    public GameObject newRecordMedals;
    public GameObject newRecordTime;
    public GameObject newRecordEnemies;

    public GameObject nextLevelButton;

    private LevelInformation levelInformation;

    // Start is called before the first frame update

    private void Awake() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        levelInformation = GameManager.instance.levelInformation;
        int enemiesKilled = GameManager.instance._enemiesKilled;
        int medalsCollected = GameManager.instance._medalsCollected;
        float timeReached = GameManager.instance._timeReached;

        medalsText.text = medalsCollected.ToString();
        timeText.text = FormatTime(timeReached);
        enemiesText.text = enemiesKilled.ToString();
        levelNameText.text = levelInformation.levelName;
        
        newRecordMedals.SetActive(levelInformation.medalsCollected < medalsCollected);
        newRecordTime.SetActive(levelInformation.timeReached > timeReached || levelInformation.timeReached == 0);
        newRecordEnemies.SetActive(levelInformation.enemiesKilled < enemiesKilled);

        levelInformation.enemiesKilled = levelInformation.enemiesKilled > enemiesKilled ? levelInformation.enemiesKilled : enemiesKilled;
        levelInformation.medalsCollected = levelInformation.medalsCollected > medalsCollected ? levelInformation.medalsCollected : medalsCollected;
        levelInformation.timeReached = levelInformation.timeReached < timeReached ? levelInformation.timeReached : timeReached;

        nextLevelButton.SetActive(levelInformation.levelNumber < 3);

    }


    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void MainMenu(){
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("Menu");

    }

    public void NextLevel(){
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("Level " + (levelInformation.levelNumber + 1));
    }
}
