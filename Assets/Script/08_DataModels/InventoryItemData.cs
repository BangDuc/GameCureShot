using System;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public string itemId;
    public Sprite _icon;
    public ItemEffectContainer itemEffect;

    public Sprite Icon
    {
        get
        {
            if (_icon != null) return _icon;
            if (!string.IsNullOrEmpty(itemId))
            {
                var data = ItemManager.GetItemData(itemId);
                if (data != null) _icon = data.Icon;
            }
            return _icon;
        }
        set => _icon = value;
    }

    public InventoryItemData()
    {
    }

    public InventoryItemData(ItemData item)
    {
        if (item != null)
        {
            itemId = item.itemId;
            _icon = item.Icon;
        }
    }

    public InventoryItemData(ItemData item, ItemEffectContainer effect)
    {
        if (item != null)
        {
            itemId = item.itemId;
            _icon = item.Icon;
            itemEffect = effect;
        }
    }
}
