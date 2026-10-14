using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private readonly InventoryData data;

    public IReadOnlyList<InventoryItemData> Items => data.items;
    public event Action Changed;

    public Inventory(InventoryData data)
    {
        this.data = data;
        if (data.items == null)
            data.items = new List<InventoryItemData>();
    }
    public void AddItem(InventoryItemData item)
    {
        data.items.Add(item);
    }
    public int GetCount(string itemId)
    {
        int count = 0;

        foreach (InventoryItemData item in data.items)
        {
            if (item.itemId == itemId)
                count++;
        }

        return count;
    }
    public bool RemoveItem(InventoryItemData item)
    {
        if (item == null || !data.items.Remove(item))
            return false;

        Changed?.Invoke();
        return true;
    }

    public bool HasItem(string itemId)
    {
        return GetCount(itemId) > 0;
    }
    //void UseItem(ItemUseContext item)
    //{
            
    //}
    //void ActivatePassiveEffects(InventoryItemData item)
    //{
            
    //}
    //void DeactivatePassiveEffects(InventoryItemData item)
    //{

    //}
}
