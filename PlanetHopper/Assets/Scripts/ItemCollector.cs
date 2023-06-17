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
        if (other.gameObject.CompareTag("Consumable"))
        {
            Item consumable = other.gameObject.GetComponent<Consumable>().item;
            if(consumable != null){
                switch(consumable.itemType){
                    case Item.ItemType.Oxygen:
                        playerLife.RefillOxygen(consumable.quantity);
                        other.gameObject.SetActive(false);
                        break;
                    case Item.ItemType.Health:
                        if(playerLife.GainHealth(consumable.quantity))
                            other.gameObject.SetActive(false);;
                        break;
                    case Item.ItemType.Medallion:
                    playerLife.GainPoints(consumable.quantity);
                        other.gameObject.SetActive(false);;
                        break;
                }
            }
        }
    }
}
