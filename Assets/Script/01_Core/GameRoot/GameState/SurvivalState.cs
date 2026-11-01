using Assets.Script.Core.GamePlay;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Core.GameRoot.GameState
{
    public class SurvivalState : IGameState
    {
        private readonly Inventory inventory;
        private readonly GameData gameData;
        private readonly GameStateMachine stateMachine;
        private readonly ItemSpawnManager itemSpawnManager;

        private RunController run;
        private EndRunPresenter endRunPresenter;
        private RunResult runResult;

        private readonly Action<RunResult> showSummary;

        public SurvivalState(
                GameStateMachine gameState,
                ItemSpawnManager itemSpawnManager,
                Inventory inventory,
                GameData gameData,
                Action<RunResult> showSummary)
        {
            this.stateMachine = gameState;
            this.itemSpawnManager = itemSpawnManager;
            this.inventory = inventory;
            this.gameData = gameData;
            this.showSummary = showSummary;
        }

        public void Enter()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;

            AsyncLoader.Instance.StartLoading("GamePlaySence");
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;

            if (endRunPresenter != null)
            {
                endRunPresenter.EndRunRequested -= HandleEndRunRequested;
            }

            Time.timeScale = 1f;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "GamePlaySence")
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
                Debug.LogError("Không tìm thấy RunController trong scene run.");
                return;
            }

            if (endRunPresenter == null)
            {
                Debug.LogError("Không tìm thấy EndRunPresenter trong scene run.");
                return;
            }



            run.Initialize(gameData);

            endRunPresenter.EndRunRequested += HandleEndRunRequested;
            InitializeWorldItems(scene);
            run.StartRun();
        }

        private void HandleEndRunRequested()
        {
            if (runResult != null)
                return;

            runResult = run.FinishRun();

            if (runResult == null)
            {
                Debug.LogError("FinishRun() chưa trả về RunResult.");
                return;
            }

            showSummary(runResult);
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