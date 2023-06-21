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
    public GameObject pauseMenu;

    public bool isBossLevel = false;

    private GameObject deathUI;
    private GameObject winUI;
    private GameObject pauseMenuUI;

    public LevelInformation levelInformation;

    public ICheckpoint currentCheckpoint;

    public static Action endLevelTakeOff;

    private bool acceptPlayerInput = true;

    private GameObject canvas;
    private BossTarget bossTarget;


    public int _enemiesKilled;
    public int _medalsCollected;
    public float _timeReached;

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
        bossTarget = GetComponent<BossTarget>();
        pauseMenuUI =  Instantiate(pauseMenu);
        pauseMenuUI.transform.SetParent(canvas.transform, false);
        
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
        if(isBossLevel){
            WinGameAfterSwitchCamera();
        }else{
            endLevelTakeOff?.Invoke();
        }
        
    }

    public void WinGameAfterSwitchCamera(){
        
        winUI = Instantiate(winScreen);
        winUI.transform.SetParent(canvas.transform, false);
        winUI.SetActive(true);

        //TODO Add next level stuff

    }

    private void SaveLevelInformation(){
        Debug.Log("Saving Level Information");
        PlayerLife playerLife = GameObject.FindWithTag("Player").GetComponent<PlayerLife>();

        _enemiesKilled = playerLife.GetEnemiesKilled();
        _medalsCollected = playerLife.GetCurrentPoints();
        _timeReached = playerLife.GetCurrentTime();

    }

    private IEnumerator Restart(){

        if(isBossLevel){
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            yield break;
        }

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
