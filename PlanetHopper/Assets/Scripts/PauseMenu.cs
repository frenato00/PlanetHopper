﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    private bool gameIsPaused = false;
    private GameObject parent;

    public GameObject pauseMenuUI;

    public DialogueManager dialogueManager;
    void Start()
    {
        parent = transform.parent.gameObject;
        pauseMenuUI.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else if(GameManager.instance.IsAcceptingPlayerInput())
            {
                Pause();
            }
        }
    }

    private void ToggleSiblings(bool active)
    {
        int selfIndex = transform.GetSiblingIndex();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (i != selfIndex)
            {
                parent.transform.GetChild(i).gameObject.SetActive(active);
                
            }
        }
    }
    
    public void Resume()
    {
        GameManager.instance.AcceptPlayerInput(true);
        ToggleSiblings(true);
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        gameIsPaused = false;
        dialogueManager.ResumeDialogue();
    }
    
    void Pause()
    {
        GameManager.instance.AcceptPlayerInput(false);
        ToggleSiblings(false);
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void MainMenu()
    {
        GameManager.instance.AcceptPlayerInput(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}