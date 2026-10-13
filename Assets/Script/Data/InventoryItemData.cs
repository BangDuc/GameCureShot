using System;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public string itemId;
    public ItemState state;
}

public enum ItemState
{
    Normal,
    Broken
}