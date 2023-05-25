using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [HideInInspector]
    public PlayerLife playerLife;

    public TMP_Text HealthText;
    public TMP_Text OxygenText;
    public TMP_Text PointsText;

    void Update(){

        HealthText.text = "Health: " + playerLife.GetCurrentHealth();
        OxygenText.text = "Oxygen: " + playerLife.GetCurrentOxygen();
        PointsText.text = "Points: " + playerLife.GetCurrentPoints();


    }
}
