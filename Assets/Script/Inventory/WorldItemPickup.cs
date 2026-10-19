using Core.Interact;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class WorldItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemId;

    private Inventory inventory;
    private WorldStateData worldData;
    private ItemData itemData;
    private bool collected;

    public string ItemId => itemId;

    public void Initialize(
        string itemId,
        Inventory inventory,
        WorldStateData worldData)
    {
        this.itemId = itemId;
        this.inventory = inventory;
        this.worldData = worldData;

        if (inventory == null || worldData == null)
        {
            Debug.LogError(
                "Chưa truyền Inventory hoặc WorldStateData.", this);
            return;
        }

        if (worldData.collectedItemIds == null)
            worldData.collectedItemIds = new List<string>();

        itemData = ItemManager.GetItemData(itemId);

        if (itemData == null)
        {
            Debug.LogError(
                $"Không tìm thấy ItemData: {itemId}", this);
            return;
        }

        GetComponent<SpriteRenderer>().sprite = itemData.icon;

        collected = worldData.collectedItemIds.Contains(itemId);
        gameObject.SetActive(!collected);
    }

    public bool CanInteract()
    {
        return isActiveAndEnabled &&
               !collected &&
               inventory != null &&
               worldData != null &&
               itemData != null;
    }

    public bool Interact()
    {
        if (!CanInteract())
            return false;

        collected = true;

        inventory.AddItem(new InventoryItemData
        {
            itemId = itemData.itemId,
            remainingUses = itemData.initialUses
        });

        if (!worldData.collectedItemIds.Contains(itemId))
            worldData.collectedItemIds.Add(itemId);

        Debug.Log($"Đã nhặt: {itemData.itemName}", this);

        gameObject.SetActive(false);
        Destroy(gameObject);

        return true;
    }

    public void OnHoverEnter()
    {
    }

    public void OnHoverExit()
    {
    }
}