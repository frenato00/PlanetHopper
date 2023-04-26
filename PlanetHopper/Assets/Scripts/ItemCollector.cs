using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    int points = 0;
    PlayerLife playerLife;

    void Start(){
        playerLife = GetComponent<PlayerLife>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Medallion"))
        {
            Destroy(other.gameObject);
            points +=1 ;
            Debug.Log("Points: " + points); 
        }
        else if (other.gameObject.CompareTag("Heart"))
        {
            Destroy(other.gameObject);
            playerLife.GainHealth();
            Debug.Log("Health: " + playerLife.GetCurrentHealth());
        }
    }
}
