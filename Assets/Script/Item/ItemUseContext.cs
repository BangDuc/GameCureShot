using System;
using UnityEngine;

[Serializable]
public class ItemUseContext
{
    public InventoryItemData item;
    public Player player;
    public GameObject targetObject;
    public InventoryItemData targetItem;
    public RunController runController;
}
