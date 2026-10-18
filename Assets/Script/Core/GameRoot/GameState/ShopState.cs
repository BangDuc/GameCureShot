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
        public ShopState(
            ShopManager shopManager,
            GameStateMachine stateMachine,
            ItemSpawnManager itemSpawnManager,
            Inventory inventory,
            GameData gameData,
            Action<RunResult> showSummary)
        {
            this.shopManager = shopManager;
            this.stateMachine = stateMachine;
            this.itemSpawnManager = itemSpawnManager;
            this.inventory = inventory;
            this.gameData = gameData;
            this.showSummary = showSummary;
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
                presenter.Clicked -= HandleChangeState;
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
                presenter.Clicked += HandleChangeState;
                presenter.Initialize(shopManager);
                return;
            }

            Debug.LogError(
                "Không tìm thấy ShopPresenter trong ShopScene.");
        }

        private void HandleChangeState()
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
    }
}