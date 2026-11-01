using System;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public string itemId;

    public Sprite _icon;

    public ItemEffectContainer itemEffect;
    public InventoryItemData()
    {

    }
    public InventoryItemData(ItemData item)
    {
        itemId = item.itemId;
        _icon = item.Icon;
    }

    public InventoryItemData(ItemData item, ItemEffectContainer effect)
    {
        itemId = item.itemId;
        _icon = item.Icon;
        itemEffect = effect;

    }
}
