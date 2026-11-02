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
            Debug.LogError("Chưa truyền Inventory hoặc WorldStateData.", this);
            return;
        }

        if (worldData.collectedItemIds == null)
            worldData.collectedItemIds = new List<string>();

        itemData = ItemManager.GetItemData(itemId);

        if (itemData == null)
        {
            Debug.LogError($"Không tìm thấy ItemData: {itemId}", this);
            return;
        }

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite == null && itemData.Icon != null)
        {
            sr.sprite = itemData.Icon;
        }

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

        // 1. Thêm vào GameData Inventory
        inventory.AddItem(new InventoryItemData
        {
            itemId = itemData.itemId
        });

        // 2. Ghi nhận itemId vào WorldStateData.collectedItemIds để không spawn lại khi nạp lại
        if (worldData.collectedItemIds == null)
            worldData.collectedItemIds = new List<string>();

        if (!worldData.collectedItemIds.Contains(itemId))
            worldData.collectedItemIds.Add(itemId);

        // 3. Đồng bộ với thanh UI Inventory runtime trong màn chơi
        if (Core.Item.ItemManager.Instance != null && Core.Item.ItemManager.Instance.InventoryItem != null)
        {
            Core.Item.ItemManager.Instance.InventoryItem.AddItemSilently(itemData);
        }

        Debug.Log($"<color=green>[WorldItemPickup] Đã nhặt Found Item: {itemData.itemName} ({itemId})! Đưa vào Inventory, sẵn sàng bán tại Shop.</color>", this);

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
