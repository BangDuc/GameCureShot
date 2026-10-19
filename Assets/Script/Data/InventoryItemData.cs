using System;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public string itemId;
    public int remainingUses = 1;
    //public ItemState state;
}

//public enum ItemState
//{
//    Normal,
//    Broken
//}