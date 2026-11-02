using Core.Interact;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteract_Pickup : MonoBehaviour, IInteractable
{
    private Core.Item.ItemManager itemManager;
    private ItemPickUpData data;
    private bool isCollected = false;

    private void Start()
    {
        InitItemPickUp();
        CheckIfAlreadyCollected();
    }

    public void InitItemPickUp()
    {
        itemManager = Core.Item.ItemManager.Instance;
        data = GetComponent<ItemPickUpData>();
    }

    private void CheckIfAlreadyCollected()
    {
        if (data == null) data = GetComponent<ItemPickUpData>();
        if (data == null || string.IsNullOrEmpty(data.ItemId)) return;

        if (RunController.Instance != null && RunController.Instance.GameData != null)
        {
            var ws = RunController.Instance.GameData.worldStateData;
            if (ws != null && ws.collectedItemIds != null && ws.collectedItemIds.Contains(data.ItemId))
            {
                isCollected = true;
                gameObject.SetActive(false);
            }
        }
    }

    public bool CanInteract()
    {
        return !isCollected && isActiveAndEnabled;
    }

    public bool Interact()
    {
        if (!CanInteract()) return false;
        isCollected = true;

        if (data == null) data = GetComponent<ItemPickUpData>();
        if (data != null)
        {
            var itemData = data.GetItemData();
            string id = data.ItemId;

            if (itemData != null)
            {
                if (itemManager == null) itemManager = Core.Item.ItemManager.Instance;
                if (itemManager != null && itemManager.InventoryItem != null)
                {
                    itemManager.InventoryItem.PickUpItem(itemData);
                }

                // Ghi nhận vào WorldStateData.collectedItemIds để mở khóa xuất hiện trong Shop
                if (RunController.Instance != null && RunController.Instance.GameData != null)
                {
                    var ws = RunController.Instance.GameData.worldStateData;
                    if (ws != null)
                    {
                        if (ws.collectedItemIds == null) ws.collectedItemIds = new List<string>();
                        if (!ws.collectedItemIds.Contains(id))
                        {
                            ws.collectedItemIds.Add(id);
                            Debug.Log($"<color=green>[ItemInteract_Pickup] Đã nhặt '{itemData.itemName}' ({id}) và mở khóa bán trong Shop!</color>");
                        }
                    }
                }
            }
        }

        Destroy(this.gameObject);
        return true;
    }

    public void OnHoverEnter()
    {
    }

    public void OnHoverExit()
    {
    }
}
