using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Rendering;

namespace Assets.Script.Core.GameRoot
{
    [Serializable]
    public class GameData
    {
        public int currentDay = 1;
        public int currentPhase = 1;
        public int money = 100;
        public int permanentHealth = 4;
        public bool IsORION_Unlocked = false;
        public int cameraCooldownLevel = 1;
        public int cameraDamageReductionLevel = 1;
        public int cameraDoubleMoneyLevel = 1;
        public int outstandingDebt = 0;
        public int consecutiveUnpaidDays = 0;
        public bool isSealBroken = false;
        public float playtime = 0;
        public InventoryData inventoryData = new();
        public WorldStateData worldStateData = new();
        public List<ShopStockData> orionShopStock = new();
        public List<ShopStockData> normalShopStock = new();
        public int lastStarterPotionPurchaseDay = 0;
    }
}