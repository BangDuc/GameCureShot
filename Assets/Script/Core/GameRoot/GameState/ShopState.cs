using Assets.Script.Core.GamePlay;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Core.GameRoot.GameState
{
    public class ShopState : IGameState
    {
        private readonly GameStateMachine stateMachine;
        private readonly ShopManager shopManager;
        private ShopPresenter presenter;
        private readonly ItemSpawnManager itemSpawnManager;
        private readonly Inventory inventory;
        private readonly GameData gameData;
        private readonly Action<RunResult> showSummary;
        private readonly Action mainMenu;
        public ShopState(
            ShopManager shopManager,
            GameStateMachine stateMachine,
            ItemSpawnManager itemSpawnManager,
            Inventory inventory,
            GameData gameData,
            Action<RunResult> showSummary,
            Action mainMenu)
        {
            this.shopManager = shopManager;
            this.stateMachine = stateMachine;
            this.itemSpawnManager = itemSpawnManager;
            this.inventory = inventory;
            this.gameData = gameData;
            this.showSummary = showSummary;
            this.mainMenu = mainMenu;
        }

        public void Enter()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
            AsyncLoader.Instance.StartLoading("ShopScene");
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            if (presenter != null)
            {
                presenter.Continue -= HandleChangeStateContinue;
                presenter.Menu -= HandleChangeStateMenu;
            }
        }

        private void HandleSceneLoaded(
            Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "ShopScene")
                return;

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                presenter =
                root.GetComponentInChildren<ShopPresenter>(true);

                if (presenter == null)
                    continue;
                presenter.Continue += HandleChangeStateContinue;
                presenter.Menu += HandleChangeStateMenu;
                presenter.Initialize(shopManager);
                return;
            }

            Debug.LogError(
                "Không tìm thấy ShopPresenter trong ShopScene.");
        }

        private void HandleChangeStateContinue()
        {
            if (!shopManager.HasBoughtStarterPotion())
                return;

            stateMachine.ChangeState(
                new SurvivalState(
                    stateMachine,
                    itemSpawnManager,
                    inventory,
                    gameData,
                    showSummary));
        }

        private void HandleChangeStateMenu()
        {
            mainMenu?.Invoke();
        }
    }
}