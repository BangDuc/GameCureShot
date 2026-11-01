using Assets.Script.Core.GamePlay;
using Assets.Script.Core.GameRoot.GameState;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Core.GameRoot
{
    public class GameRoot : MonoBehaviour
    {
        private static GameRoot instance;
        private GameSession gameSession;
        private GameStateMachine gameState;
        private ShopManager shopManager;
        private Inventory inventory;
        private DebtManager debtManager;
        private EndingSystem endingSystem;
        private ItemSpawnManager itemSpawnManager;
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        private void Initialize()
        {
            gameSession = new GameSession();

            gameState = new GameStateMachine(
                new MainMenuState(StartGame, LoadGame));
        }

        private void LoadMainMenuScene()
        {
            gameState.ChangeState(
                new MainMenuState(StartGame, LoadGame));
        }
        [ContextMenu("Chạy Hàm Test")]
        public void StartGame()
        {
            ShopOfferData starter =
                ItemManager.GetNormalShopItemData("StarterPotion");

            if (starter == null)
            {
                Debug.LogError("Không tìm thấy offer của StarterPotion.");
                return;
            }

            gameSession.StartNewGame(starter.price);
            InitializeManagers();
            AutoSave();

            gameState.ChangeState(new ShopState(
                shopManager,
                gameState,
                itemSpawnManager,
                inventory,
                gameSession.Data,
                ShowSummary,
                LoadMainMenuScene));
        }
        private void PrepareNextDay()
        {
            gameSession.AdvanceDay();
            shopManager.RestockNormalShop();
            AutoSave();

            gameState.ChangeState(
                new ShopState(
                    shopManager,
                    gameState,
                    itemSpawnManager,
                    inventory,
                    gameSession.Data,
                    ShowSummary,
                    LoadMainMenuScene));
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
            this.itemSpawnManager = new ItemSpawnManager(data);
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
            gameState.ChangeState(new ShopState(
                shopManager,
                gameState,
                itemSpawnManager,
                inventory,
                gameSession.Data,
                ShowSummary,
                LoadMainMenuScene));
        }
        private void ShowSummary(RunResult runResult)
        {
            gameState.ChangeState(
                new SummaryState(
                    runResult,
                    gameSession.Data,
                    debtManager,
                    endingSystem,
                    gameState,
                    PrepareNextDay,
                    LoadMainMenuScene));
        }
    }
}

