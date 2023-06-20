using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    private bool gameIsPaused = false;
    private GameObject parent;

    public GameObject pauseMenuUI;

    void Start()
    {
        parent = transform.parent.gameObject;
        pauseMenuUI.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    
    public void Resume()
    {
        GameManager.instance.AcceptPlayerInput(true);
        if (parent.transform.childCount >= 1)
        {
            parent.transform.GetChild(1).gameObject.SetActive(true);
        }
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    
    void Pause()
    {
        GameManager.instance.AcceptPlayerInput(false);
        if (parent.transform.childCount >= 1)
        {
            parent.transform.GetChild(1).gameObject.SetActive(false);
        }
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}