using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [HideInInspector]
    public PlayerLife playerLife;

    public TMP_Text HealthText;
    public Image OxygenMask;
    public TMP_Text OxygenText;
    public TMP_Text PointsText;
    public Animator dialogueBoxAnimator;
    public TMP_Text dialogueName;
    public TMP_Text dialogueText;

    void Update(){

        HealthText.text = "Health: " + playerLife.GetCurrentHealth();
        OxygenText.text = "Oxygen: " + playerLife.GetCurrentOxygen();
        PointsText.text = "Points: " + playerLife.GetCurrentPoints();

        int alpha = (int) (255 * ((((float) playerLife.maxOxygen) - playerLife.GetCurrentOxygen()) / (float) playerLife.maxOxygen));

        OxygenMask.color = new Color32(0, 0, 0, (byte) alpha);


    }

}
