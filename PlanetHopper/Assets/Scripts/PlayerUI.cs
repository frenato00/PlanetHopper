using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerUI : MonoBehaviour
{
    [HideInInspector]
    public PlayerLife playerLife;

    public TMP_Text HealthText;
    public Image OxygenMask;
    public TMP_Text OxygenText;
    public TMP_Text PointsText;
    public TMP_Text TimeText;

    void Update(){

        HealthText.text = "Health: " + playerLife.GetCurrentHealth();
        OxygenText.text = "Oxygen: " + playerLife.GetCurrentOxygen();
        PointsText.text = "Points: " + playerLife.GetCurrentPoints();

        int alpha = (int) (255 * ((((float) playerLife.maxOxygen) - playerLife.GetCurrentOxygen()) / (float) playerLife.maxOxygen));

        OxygenMask.color = new Color32(0, 0, 0, (byte) alpha);

        string time = FormatTime(playerLife.GetCurrentTime());
        TimeText.text = time;


    }

    private string FormatTime(float time){
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
