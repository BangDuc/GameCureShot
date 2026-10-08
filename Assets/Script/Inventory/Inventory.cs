using UnityEngine;

public class Inventory
{
    public InventoryData data;
    public Inventory(InventoryData data)
    {
        this.data = data;
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
    //void RemoveItem(InventoryItemData item)
    //{

    //}
    //bool HasItem(string itemId)
    //{

    //}
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
