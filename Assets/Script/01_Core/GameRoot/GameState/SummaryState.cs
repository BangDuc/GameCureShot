using Assets.Script.Core.GamePlay;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Core.GameRoot.GameState
{
    public class SummaryState : IGameState
    {
        private const string SceneName = "DaySummary";

        private readonly RunResult runResult;
        private readonly GameData gameData;
        private readonly DebtManager debtManager;
        private readonly EndingSystem endingSystem;
        private readonly GameStateMachine stateMachine;
        private readonly Action prepareNextDay;
        private readonly Action mainMenu;
        private SummaryPresenter presenter;
        private DebtResult debtResult;

        private bool debtSettled;
        private bool continueHandled;

        public SummaryState(
            RunResult runResult,
            GameData gameData,
            DebtManager debtManager,
            EndingSystem endingSystem,
            GameStateMachine stateMachine,
            Action prepareNextDay,
            Action mainMenu)
        {
            this.runResult = runResult;
            this.gameData = gameData;
            this.debtManager = debtManager;
            this.endingSystem = endingSystem;
            this.stateMachine = stateMachine;
            this.prepareNextDay = prepareNextDay;
            this.mainMenu = mainMenu;
        }

        public void Enter()
        {
            Time.timeScale = 1f;

            if (!debtSettled)
            {
                debtResult = debtManager.PayDailyDebt();
                debtSettled = true;

                // Requirement 11: Debt và Save sau Summary
                SaveSystem.Save(gameData, 0);
                Debug.Log($"<color=cyan>[SummaryState] Đã xử lý nợ ngày {gameData.currentDay} và lưu GameData. Tiền: ${gameData.money}, Nợ còn: ${gameData.outstandingDebt}, Chưa trả: {gameData.consecutiveUnpaidDays} ngày.</color>");
            }

            SceneManager.sceneLoaded += HandleSceneLoaded;

            if (SceneManager.GetActiveScene().name == SceneName)
            {
                HookPresenter(SceneManager.GetActiveScene());
            }
            else
            {
                AsyncLoader.Instance.StartLoading(SceneName);
            }
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;

            if (presenter != null)
            {
                presenter.Clicked -= HandleContinueClicked;
                presenter = null;
            }
        }

        private void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            if (scene.name != SceneName)
                return;

            HookPresenter(scene);
        }

        private void HookPresenter(Scene scene)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                presenter =
                    root.GetComponentInChildren<SummaryPresenter>(true);

                if (presenter == null)
                    continue;

                presenter.Initialize(
                    runResult,
                    debtResult,
                    gameData);

                presenter.Clicked -= HandleContinueClicked;
                presenter.Clicked += HandleContinueClicked;
                return;
            }

            Debug.LogError(
                $"Không tìm thấy SummaryPresenter trong {SceneName}.");
        }

        private void HandleContinueClicked()
        {
            if (continueHandled)
                return;

            continueHandled = true;

            EndingData ending = endingSystem.CheckEnding();

            if (ending != null)
            {
                Debug.Log($"<color=magenta>[SummaryState] Đạt điều kiện Ending: {ending.endingId}! Chuyển sang EndingState.</color>");
                stateMachine.ChangeState(
                    new EndingState(ending, stateMachine, mainMenu));

                return;
            }

            Debug.Log($"<color=green>[SummaryState] Tiếp tục sang ngày mới!</color>");
            prepareNextDay?.Invoke();
        }
    }
}
