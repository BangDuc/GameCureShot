using Assets.Script.Core.GameRoot.GameState;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Core.GameRoot
{
    public class GameRoot : MonoBehaviour
    {
        private GameSession gameSession;
        private GameStateMachine gameState;
        private ShopManager shopManager;
        private Inventory inventory;
        private DebtManager debtManager;
        private EndingSystem endingSystem;
        private void Awake()
        {
            Initialize();
            DontDestroyOnLoad(this);
        }

        private void Initialize()
        {
            gameSession = new GameSession();
            gameState = new GameStateMachine(new MainMenuState());
        }
        [ContextMenu("Chạy Hàm Test")]
        public void StartGame()
        {
            AsyncLoader.Instance.StartLoading("GamePlaySence");
            ShopStockData orion = new ShopStockData
            {
                itemId = "test_item",
                remainingStock = 2
            };
            GameData data = new GameData
            {
                currentDay = 1,
                cameraCooldownLevel = 1,
                cameraDamageReductionLevel = 1,
                cameraDoubleMoneyLevel = 1,
                consecutiveUnpaidDays = 0,
                currentPhase = 1,
                IsORION_Unlocked = false,
                isSealBroken = false,
                money = 0,
                outstandingDebt = 0,
                permanentHealth = 4,
                playtime = 5400f
            };
            data.orionShopStock.Add(orion);
            SaveSystem.Save(data, 0);
            Debug.Log(Application.persistentDataPath);

            //    ShopOfferData starter =
            //ItemManager.GetNormalShopItemData("StarterPotion");

            //    if (starter == null)
            //    {
            //        Debug.LogError("Không tìm thấy offer của StarterPotion.");
            //        return;
            //    }

            //    gameSession.StartNewGame(starter.price);
            //    InitializeManagers();
            //    AutoSave();

            //gameState.ChangeState(ShopState);
        }
        private void PrepareNextDay()
        {
            gameSession.AdvanceDay();
            shopManager.RestockNormalShop();
            AutoSave();
        }
        private void AutoSave()
        {
            if (gameSession.Data == null)
                return;

            SaveSystem.Save(gameSession.Data, 0);
        }
        private void InitializeManagers()
        {   
            GameData data = gameSession.Data;
            this.inventory = new Inventory(data.inventoryData);
            this.debtManager = new DebtManager(data);
            this.shopManager = new ShopManager(inventory, data, debtManager);
            this.endingSystem = new EndingSystem(data);
        }    
        public void LoadGame(int slotId)
        {
            GameData data = SaveSystem.Load(slotId);
            if (data == null)
            {
                Debug.LogWarning($"Không có save ở ô {slotId}");
                return;
            }
            gameSession.SetData(data);
            InitializeManagers();
            gameState.ChangeState(new ShopState(this.shopManager, gameState));
        }    
    }
}

