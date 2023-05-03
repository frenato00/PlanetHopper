using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]


public class Item : ScriptableObject
{
    public string objectName;
    public int quantity;
    public enum ItemType{
        Oxygen,
        Health,
        Medallion
    }

    public ItemType itemType;

    
}
