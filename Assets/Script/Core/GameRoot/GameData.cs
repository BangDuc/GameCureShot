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
        public ResumePoint resumepoint;
        public InventoryData inventory = new();
        public WorldStateData worldState = new();
        public List<ShopStockData> orionShopStock = new();
    }
}

public enum ResumePoint
{
    run,
    summary,
    shop
}