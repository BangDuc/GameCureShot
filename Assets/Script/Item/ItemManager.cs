using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ItemManager
{
    public static List<ItemData> itemDatabase;
    public static ShopConfigData config;

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
        string folder = Path.Combine(
            Application.dataPath, "Resources", "Data");

        Directory.CreateDirectory(folder);

        string path = Path.Combine(folder, "items.json");
        string json = File.ReadAllText(path);
        itemDatabase = JsonUtility.FromJson<List<ItemData>>(json);
        return itemDatabase;
    }

    public static ShopConfigData GetShopOfferItemDatas()
    {
        string folder = Path.Combine(
            Application.dataPath, "Resources", "Data");

        Directory.CreateDirectory(folder);

        string path = Path.Combine(folder, "shops.json");
        string json = File.ReadAllText(path);
        config = JsonUtility.FromJson<ShopConfigData>(json);
        return config;
    }
    public static List<ShopOfferData> GetNormalShopItemDatas()
    {
        if (config == null) config = GetShopOfferItemDatas();
        return config.normalItems;
    }
    public static ShopOfferData GetNormalShopItemData(string itemId)
    {
        foreach(ShopOfferData data in config.normalItems)
        {
            if (data.itemId == itemId) return data;
        }
        return null;
    }
    public static ShopOfferData GetOrionsShopItemData(string itemId)
    {
        foreach (ShopOfferData data in config.orionItems)
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
}
