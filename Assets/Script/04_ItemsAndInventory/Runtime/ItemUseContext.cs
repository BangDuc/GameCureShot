using System;
using UnityEngine;

[Serializable]
public class ItemUseContext
{
    public InventoryItemData item;
    public PlayerManager player;
    public GameObject targetObject;
    public InventoryItemData targetItem;
    public RunController runController;

    public ItemUseContext() { }
    public ItemUseContext(InventoryItemData item, PlayerManager player, GameObject targetObject, InventoryItemData targetItem, RunController runController)
    {
        this.item = item;
        this.player = player;
        this.targetObject = targetObject;
        this.targetItem = targetItem;
        this.runController = runController;
    }

    public ItemUseContext (InventoryItemData item, PlayerManager player, RunController runController)
    {
        this.item = item;
        this.player = player;
        this.runController = runController;
    }
}
