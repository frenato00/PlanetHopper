using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    private bool acceptPlayerInput = true;

    public MenuLevel[] levels;

    [SerializeField]
    private GameObject dog;


    // Start is called before the first frame update

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
        for (int i = 0; i < levels.Length; i++)
        {
            if (i == 0) levels[i].EnableLevel();
            else
            {
                if (levels[i - 1].level.timeReached < 99999)
                {
                    levels[i].EnableLevel();
                }
                else
                {
                    levels[i].DisableLevel();
                }
            }
        }
        if (levels[2].level.timeReached < 99999)
        {
            dog.SetActive(true);
        }
        else
        {
            dog.SetActive(false);
        }
    }

    void Start()
    {
    }

    public void AcceptPlayerInput(bool accept)
    {
        acceptPlayerInput = accept;
    }

    public bool IsAcceptingPlayerInput()
    {
        return acceptPlayerInput;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (i == 0) levels[i].EnableLevel();
            else
            {
                if (levels[i - 1].level.timeReached < 99999)
                {
                    levels[i].EnableLevel();
                }
                else
                {
                    levels[i].DisableLevel();
                }
            }
        }

        if (levels[2].level.timeReached < 99999)
        {
            dog.SetActive(true);
        }
        else
        {
            dog.SetActive(false);
        }
    }
}
