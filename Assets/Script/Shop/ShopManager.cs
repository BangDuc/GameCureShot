using Assets.Script.Core.GameRoot;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager
{
    //public List<ShopStockData> orionShopStock;
    //public List<ShopOfferData> orionItems;
    //public List<CameraUpgradeData> cameraUpgrades;

    private readonly List<ShopOfferData> normalItems;
    private readonly List<ShopStockData> normalShopStock;
    private readonly Inventory inventory;
    private readonly GameData gameData;
    private readonly DebtManager debtManager;
    private const string starterPotionId = "StarterPotion";

    public int CurrentDay => gameData.currentDay;
    public int Money => gameData.money;
    public int OutstandingDebt => gameData.outstandingDebt;
    public int PermanentHealth => gameData.permanentHealth;

    public ShopManager(
        Inventory inventory,
        GameData gameData,
        DebtManager debtManager)
    {
        if (inventory == null)
            throw new ArgumentNullException(nameof(inventory));
        if (gameData == null)
            throw new ArgumentNullException(nameof(gameData));
        if (debtManager == null)
            throw new ArgumentNullException(nameof(debtManager));

        this.normalItems = ItemManager.GetNormalShopItemDatas();
        this.normalShopStock = gameData.normalShopStock;
        this.inventory = inventory;
        this.gameData = gameData;
        this.debtManager = debtManager;

        HashSet<string> ids = new();
        foreach (ShopOfferData offer in normalItems)
        {
            if (offer == null || string.IsNullOrWhiteSpace(offer.itemId) ||
                offer.price < 0 || offer.initialStock < 0)
            {
                throw new ArgumentException(
                    "Offer cần ID hợp lệ, giá và stock ban đầu từ 0 trở lên.",
                    nameof(normalItems));
            }

            if (!ids.Add(offer.itemId))
                throw new ArgumentException(
                    $"Trùng offer của item {offer.itemId}.", nameof(normalItems));
        }

        EnsureNormalStock();
    }

    public List<ShopOfferData> GetNormalOffers()
    {
        List<ShopOfferData> offers = new();
        foreach (ShopOfferData offer in normalItems)
        {
            if(offer.group == ShopItemGroup.Found)
            {
                if (gameData.worldStateData != null &&
                    gameData.worldStateData.collectedItemIds != null &&
                    gameData.worldStateData.collectedItemIds.Contains(offer.itemId))
                {
                    offers.Add(offer);
                }
            }
            else
            {
                offers.Add(offer);
            }
        }
        return offers;
    }

    public ShopOfferData GetNormalOffer(string itemId)
    {
        foreach (ShopOfferData offer in GetNormalOffers())
        {
            if (offer.itemId == itemId)
                return offer;
        }

        return null;
    }

    public ShopStockData GetNormalStock(string itemId)
    {
        if (itemId == starterPotionId)
        {
            return new ShopStockData
            {
                itemId = starterPotionId,
                remainingStock = HasBoughtStarterPotion() ? 0 : 1
            };
        }

        foreach (ShopStockData stock in normalShopStock)
        {
            if (stock != null && stock.itemId == itemId)
                return stock;
        }

        return null;
    }

    public int GetOwnedCount(string itemId)
    {
        return inventory.GetCount(itemId);
    }

    public bool CanAfford(int amount)
    {
        return amount >= 0 && gameData.money >= amount;
    }

    public bool HasBoughtStarterPotion()
    {
        return gameData.lastStarterPotionPurchaseDay == gameData.currentDay;
    }

    public bool BuyItem(string itemId, int quantity)
    {
        if (quantity <= 0)
            return false;

        if (itemId == starterPotionId)
            return quantity == 1 && BuyStarterPotion();

        ShopOfferData offer = GetNormalOffer(itemId);
        ShopStockData stock = GetNormalStock(itemId);

        if (offer == null || stock == null ||
            stock.remainingStock < quantity)
            return false;

        if (offer.price < 0 || ItemManager.GetItemData(itemId) == null)
            return false;

        long total = (long)offer.price * quantity;

        if (total > int.MaxValue || !CanAfford((int)total))
            return false;

        for (int i = 0; i < quantity; i++)
        {
            inventory.AddItem(new InventoryItemData
            {
                itemId = itemId,
                state = ItemState.Normal
            });
        }

        gameData.money -= (int)total;
        stock.remainingStock -= quantity;
        return true;
    }
    public bool BuyStarterPotion()
    {
        if (HasBoughtStarterPotion())
            return false;

        ShopOfferData offer = GetNormalOffer(starterPotionId);

        if (offer == null || offer.price < 0 || gameData.money < 0 ||
            ItemManager.GetItemData(starterPotionId) == null)
            return false;

        int paidAmount = Math.Min(gameData.money, offer.price);
        int missingAmount = offer.price - paidAmount;

        if (missingAmount > 0)
            debtManager.AddDebt(missingAmount);

        inventory.AddItem(new InventoryItemData
        {
            itemId = starterPotionId,
            state = ItemState.Normal
        });

        gameData.money -= paidAmount;
        gameData.lastStarterPotionPurchaseDay = gameData.currentDay;
        return true;
    }
    private void EnsureNormalStock()
    {
        foreach (ShopOfferData offer in normalItems)
        {
            if (offer.itemId == starterPotionId)
                continue;

            if (GetNormalStock(offer.itemId) != null)
                continue;

            normalShopStock.Add(new ShopStockData
            {
                itemId = offer.itemId,
                remainingStock = offer.initialStock
            });
        }
    }
    public void RestockNormalShop()
    {
        EnsureNormalStock();

        foreach (ShopOfferData offer in normalItems)
        {
            if (offer.itemId == starterPotionId)
                continue;

            ShopStockData stock = GetNormalStock(offer.itemId);
            stock.remainingStock = offer.initialStock;
        }
    }
}
