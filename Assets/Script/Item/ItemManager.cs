using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ItemManager
{
    public static List<ItemData> itemDatabase;
    public static ShopConfigData config;

    [RuntimeInitializeOnLoadMethod(
    RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetCache()
    {
        itemDatabase = null;
        config = null;
    }

    public static ItemData GetItemData(string itemId)
    {
        if(itemDatabase == null)
        {
            itemDatabase = GetItemDatas();
        }    
        foreach(ItemData item in itemDatabase)
        {
            if (item.itemId == itemId) return item;
            continue;
        }
        return null;
    }

    public static List<ItemData> GetItemDatas()
    {
        if (itemDatabase != null)
            return itemDatabase;

        string json = ReadConfigJson("items");
        ItemConfigData fileData =
            JsonUtility.FromJson<ItemConfigData>(json);

        if (fileData == null || fileData.items == null)
            throw new System.InvalidOperationException(
                "Cấu hình items không có danh sách items.");

        List<ItemData> loadedItems = new();

        foreach (ItemConfig itemConfig in fileData.items)
        {
            ItemData item = new ItemData
            {
                itemId = itemConfig.itemId,
                itemName = itemConfig.itemName,
                description = itemConfig.description,
                //icon = Resources.Load<Sprite>(itemConfig.iconPath),

                
            };

            // Nối bước tạo effect từ itemConfig.effects tại đây.
            // Nối bước tạo passive từ itemConfig.passiveEffects tại đây.

            loadedItems.Add(item);
        }

        itemDatabase = loadedItems;
        return itemDatabase;
    }

    public static ShopConfigData GetShopOfferItemDatas()
    {
        if (config != null)
            return config;

        string json = ReadConfigJson("shops");
        config = JsonUtility.FromJson<ShopConfigData>(json);

        if (config == null)
            throw new System.InvalidOperationException(
                "Không đọc được cấu hình shop.");

        return config;
    }
    public static List<ShopOfferData> GetNormalShopItemDatas()
    {
        if (config == null) config = GetShopOfferItemDatas();
        return config.normalItems;
    }

    public static ShopOfferData GetNormalShopItemData(string itemId)
    {
        foreach (ShopOfferData data in GetNormalShopItemDatas())
        {
            if (data.itemId == itemId) return data;
        }
        return null;
    }
    public static ShopOfferData GetOrionsShopItemData(string itemId)
    {
        foreach (ShopOfferData data in GetOrionsShopItemDatas())
        {
            if (data.itemId == itemId) return data;
        }
        return null;
    }
    public static List<ShopOfferData> GetOrionsShopItemDatas()
    {
        if (config == null) config = GetShopOfferItemDatas();
        return config.orionItems;
    }
    private static string ReadConfigJson(string name)
    {
        TextAsset file = Resources.Load<TextAsset>($"Data/{name}");

        if (file == null)
            throw new System.InvalidOperationException(
                $"Không tìm thấy cấu hình Data/{name}.");

        return file.text;
    }

    //private static List<IItemEffect> CreateEffects(
    //List<ItemEffectConfig> configs)
    //{
    //    List<IItemEffect> effects = new();

    //    if (configs == null)
    //        return effects;

    //    foreach (ItemEffectConfig config in configs)
    //    {
    //        switch (config.type)
    //        {
    //            case "Heal":
    //                effects.Add(new HealEffect(config.amount));
    //                break;

    //            default:
    //                throw new System.InvalidOperationException(
    //                    $"Chưa hỗ trợ effect: {config.type}");
    //        }
    //    }

    //    return effects;
    //}

    //private static List<IPassiveItemEffect> CreatePassiveEffects(
    //List<ItemEffectConfig> configs)
    //{
    //    List<IPassiveItemEffect> effects = new();

    //    if (configs == null)
    //        return effects;

    //    foreach (ItemEffectConfig config in configs)
    //    {
    //        switch (config.type)
    //        {
    //            case "Heal":
    //                effects.Add(new HealEffect(config.amount));
    //                break;

    //            default:
    //                throw new System.InvalidOperationException(
    //                    $"Chưa hỗ trợ effect: {config.type}");
    //        }
    //    }

    //    return effects;
    //}
}
