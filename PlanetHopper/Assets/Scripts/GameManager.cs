using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject deathScreen;
    public GameObject winScreen;

    private GameObject deathUI;
    private GameObject winUI;

    public LevelInformation levelInformation;

    public ICheckpoint currentCheckpoint;

    public static Action endLevelTakeOff;

    private bool acceptPlayerInput = true;

    private GameObject canvas;


    private int _enemiesKilled;
    private int _medalsCollected;
    private float _timeReached;

    // Start is called before the first frame update
    
    void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    public void GameOver(){
        acceptPlayerInput = false;
        deathUI =  Instantiate(deathScreen);
        deathUI.transform.SetParent(canvas.transform, false);
        deathUI.SetActive(true);

        StartCoroutine(Restart());
    }

    public void Win(){
        acceptPlayerInput = false;
        SaveLevelInformation();
        endLevelTakeOff?.Invoke();
        
    }

    public void WinGameAfterSwitchCamera(){
        winUI =  Instantiate(winScreen);
        winUI.transform.SetParent(canvas.transform, false);
        winUI.SetActive(true);

        //TODO Add next level stuff

    }

    private void SaveLevelInformation(){
        PlayerLife playerLife = GameObject.FindWithTag("Player").GetComponent<PlayerLife>();

        _enemiesKilled = playerLife.GetEnemiesKilled();
        _medalsCollected = playerLife.GetCurrentPoints();
        _timeReached = playerLife.GetCurrentTime();

        levelInformation.enemiesKilled = levelInformation.enemiesKilled > _enemiesKilled ? levelInformation.enemiesKilled : _enemiesKilled;
        levelInformation.medalsCollected = levelInformation.medalsCollected > _medalsCollected ? levelInformation.medalsCollected : _medalsCollected;
        levelInformation.timeReached = levelInformation.timeReached < _timeReached ? levelInformation.timeReached : _timeReached;

    }

    private IEnumerator Restart(){
        Debug.Log("Restarting");
        yield return new WaitForSeconds(3f);
        if(deathUI != null){
            Destroy(deathUI);
        }
        if(winUI != null){
            Destroy(winUI);
        }

        currentCheckpoint.ResetGameState();

        acceptPlayerInput = true;
    }

    public void AcceptPlayerInput(bool accept){
        acceptPlayerInput = accept;
    }

    public bool IsAcceptingPlayerInput(){
        return acceptPlayerInput;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
