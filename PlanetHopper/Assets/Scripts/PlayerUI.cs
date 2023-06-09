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
    public RectTransform HealthBar;
    public Image OxygenMask;
    // public TMP_Text OxygenText;
    public TMP_Text PointsText;
    public Animator dialogueBoxAnimator;
    public TMP_Text dialogueName;
    public TMP_Text dialogueText;
    public TMP_Text TimeText;

    void Update()
    {

        HealthText.text = "Health: " + playerLife.GetCurrentHealth();
        HealthBar.sizeDelta = new Vector2(85*playerLife.GetCurrentHealth(),85);
        // OxygenText.text = "Oxygen: " + playerLife.GetCurrentOxygen();

        int alpha = (int)(255 * ((((float)playerLife.maxOxygen) - playerLife.GetCurrentOxygen()) / (float)playerLife.maxOxygen));

        OxygenMask.color = new Color32(0, 0, 0, (byte)alpha);

        PointsText.text = "" + playerLife.GetCurrentPoints();
        string time = FormatTime(playerLife.GetCurrentTime());
        TimeText.text = time;


    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
