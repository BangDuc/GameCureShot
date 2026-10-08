using System;

public enum ShopItemGroup
{
    Buff,
    Found
}

[Serializable]
public class ShopOfferData
{
    public string itemId;
    public int price;
    public int initialStock;
    public ShopItemGroup group;
}