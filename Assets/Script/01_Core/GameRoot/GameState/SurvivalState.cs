using Assets.Script.Core.GamePlay;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Core.GameRoot.GameState
{
    public class SurvivalState : IGameState
    {
        private const string SceneName = "GamePlaySence";
        private readonly Inventory inventory;
        private readonly GameData gameData;
        private readonly GameStateMachine stateMachine;
        private readonly ItemSpawnManager itemSpawnManager;

        private RunController run;
        private EndRunPresenter endRunPresenter;
        private RunResult runResult;

        private readonly Action<RunResult> showSummary;
        private readonly bool forceReload;

        public SurvivalState(
                GameStateMachine gameState,
                ItemSpawnManager itemSpawnManager,
                Inventory inventory,
                GameData gameData,
                Action<RunResult> showSummary,
                bool forceReload = false)
        {
            this.stateMachine = gameState;
            this.itemSpawnManager = itemSpawnManager;
            this.inventory = inventory;
            this.gameData = gameData;
            this.showSummary = showSummary;
            this.forceReload = forceReload;
        }

        public void Enter()
        {
            Time.timeScale = 1f;
            runResult = null;
            SceneManager.sceneLoaded += HandleSceneLoaded;

            if (!forceReload && SceneManager.GetActiveScene().name == SceneName)
            {
                InitializeRun(SceneManager.GetActiveScene());
            }
            else
            {
                if (AsyncLoader.Instance != null)
                {
                    AsyncLoader.Instance.StartLoading(SceneName);
                }
                else
                {
                    SceneManager.LoadScene(SceneName);
                }
            }
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;

            if (run != null)
            {
                run.OnRunFinished -= HandleRunFinished;
                run = null;
            }

            if (endRunPresenter != null)
            {
                endRunPresenter.EndRunRequested -= HandleEndRunRequested;
                endRunPresenter = null;
            }

            Time.timeScale = 1f;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SceneName)
                return;

            InitializeRun(scene);
        }

        private void InitializeRun(Scene scene)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                if (run == null)
                {
                    run = root.GetComponentInChildren<RunController>(true);
                }

                if (endRunPresenter == null)
                {
                    endRunPresenter =
                        root.GetComponentInChildren<EndRunPresenter>(true);
                }
            }

            if (run == null)
            {
                run = RunController.Instance;
            }

            if (run == null)
            {
                Debug.LogError($"Không tìm thấy RunController trong scene {SceneName}.");
                return;
            }

            run.Initialize(gameData);
            run.OnRunFinished -= HandleRunFinished;
            run.OnRunFinished += HandleRunFinished;

            if (endRunPresenter != null)
            {
                endRunPresenter.EndRunRequested -= HandleEndRunRequested;
                endRunPresenter.EndRunRequested += HandleEndRunRequested;
            }

            SyncInventoryWithRun();
            InitializeWorldItems(scene);

            if (MultiRoomMonsterSpawner.Instance != null)
            {
                MultiRoomMonsterSpawner.Instance.InitializeSpawner(scene);
            }

            run.StartRun();
        }

        private void HandleEndRunRequested()
        {
            if (run != null && !run.IsRunFinished)
            {
                run.FinishRun();
            }
        }

        private void HandleRunFinished(RunResult result)
        {
            if (runResult != null)
                return;

            runResult = result;

            if (runResult == null)
            {
                Debug.LogError("FinishRun() chưa trả về RunResult.");
                return;
            }

            // 1. Cập nhật GameData trước khi rời Survival Run (Requirement 9)
            gameData.money += runResult.money;
            gameData.playtime += runResult.survivalTime;

            if (runResult.failed)
            {
                gameData.permanentHealth = Mathf.Max(0, gameData.permanentHealth - 1);
                Debug.Log($"<color=red>[SurvivalState] Run thất bại! Máu vĩnh viễn còn: {gameData.permanentHealth}/4</color>");
            }

            // 2. Save sau Survival Run (Requirement 10)
            SaveSystem.Save(gameData, 0);
            Debug.Log($"<color=green>[SurvivalState] Đã lưu GameData sau Run. Tiền: ${gameData.money}, Máu vĩnh viễn: {gameData.permanentHealth}/4</color>");

            // 3. Chuyển sang Summary (Requirement 8)
            showSummary(runResult);
        }

        private void SyncInventoryWithRun()
        {
            if (gameData == null || gameData.inventoryData == null) return;

            var itemMgr = global::Core.Item.ItemManager.Instance;
            if (itemMgr != null && itemMgr.InventoryItem != null)
            {
                var runtimeInv = itemMgr.InventoryItem;
                runtimeInv.InitInventory();

                if (gameData.inventoryData.items != null)
                {
                    foreach (var invItem in gameData.inventoryData.items)
                    {
                        if (invItem == null || string.IsNullOrEmpty(invItem.itemId)) continue;
                        var itemData = ItemManager.GetItemData(invItem.itemId);
                        if (itemData != null)
                        {
                            // Nạp vào runtime inventory mà không ghi trùng thêm vào GameData
                            runtimeInv.AddItemSilently(itemData);
                        }
                    }
                }
            }
        }

        private void InitializeWorldItems(Scene scene)
        {
            if (gameData.worldStateData == null)
                gameData.worldStateData = new WorldStateData();

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                WorldItemPickup[] items =
                    root.GetComponentsInChildren<WorldItemPickup>(true);

                foreach (WorldItemPickup item in items)
                {
                    item.Initialize(
                        item.ItemId,
                        inventory,
                        gameData.worldStateData);
                }
            }
        }
    }
}
