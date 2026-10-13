using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Rendering;

namespace Assets.Script.Core.GameRoot
{
    [Serializable]
    public class GameData
    {
        public int currentDay;
        public int currentPhase;
        public int money;
        public int permanentHealth;
        public bool IsORION_Unlocked;
        public int cameraCooldownLevel;
        public int cameraDamageReductionLevel;
        public int cameraDoubleMoneyLevel;
        public int outstandingDebt;
        public int consecutiveUnpaidDays;
        public bool isSealBroken;
        public float playtime;
        public InventoryData inventoryData = new();
        public WorldStateData worldStateData = new();
        public List<ShopStockData> orionShopStock = new();
        public List<ShopStockData> normalShopStock = new();
        public int lastStarterPotionPurchaseDay;
    }
}