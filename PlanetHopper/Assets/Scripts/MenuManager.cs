using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    private bool acceptPlayerInput = true;


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

    public void StartGame() {
        Debug.Log("Starting Game");
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        Debug.Log("Quitting Game");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
