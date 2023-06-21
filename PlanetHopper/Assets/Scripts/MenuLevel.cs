using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLevel : MonoBehaviour
{
    public LevelInformation level;

    public LevelInformation previousLevel;

    void Awake()
    {
        if (level.levelNumber != 1 && level.timeReached == 99999) {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
        }

        if(previousLevel != null){
            if (previousLevel.timeReached == 99999) {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
            }
            else {
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }


    }

    public void SetInfo() {
        MenuLevelInfo.instance.SetLevel(level);
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
