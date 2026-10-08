using Assets.Script.Core.GameRoot;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager
{
    public Inventory inventory;
    public List<ShopStockData> normalShopStock;
    public List<ShopStockData> orionShopStock;
    public GameSession gameSession;
    public List<ShopOfferData> normalItems;
    public List<ShopOfferData> orionItems;
    public List<CameraUpgradeData> cameraUpgrades;
    public ItemData starterPotion;
    public DebtManager debtManager;

}
