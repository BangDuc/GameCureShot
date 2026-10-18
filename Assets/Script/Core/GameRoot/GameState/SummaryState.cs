using Assets.Script.Core.GamePlay;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Core.GameRoot.GameState
{
    public class SummaryState : IGameState
    {
        private const string SceneName = "SummaryScene";

        private readonly RunResult runResult;
        private readonly GameData gameData;
        private readonly DebtManager debtManager;
        private readonly EndingSystem endingSystem;
        private readonly GameStateMachine stateMachine;
        private readonly Action prepareNextDay;

        private SummaryPrensenter presenter;
        private DebtResult debtResult;

        private bool debtSettled;
        private bool continueHandled;

        public SummaryState(
            RunResult runResult,
            GameData gameData,
            DebtManager debtManager,
            EndingSystem endingSystem,
            GameStateMachine stateMachine,
            Action prepareNextDay)
        {
            this.runResult = runResult;
            this.gameData = gameData;
            this.debtManager = debtManager;
            this.endingSystem = endingSystem;
            this.stateMachine = stateMachine;
            this.prepareNextDay = prepareNextDay;
        }

        public void Enter()
        {
            Time.timeScale = 1f;

            if (!debtSettled)
            {
                debtResult = debtManager.PayDailyDebt();
                debtSettled = true;
            }

            SceneManager.sceneLoaded += HandleSceneLoaded;
            AsyncLoader.Instance.StartLoading(SceneName);
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;

            if (presenter != null)
            {
                presenter.Clicked -= HandleContinueClicked;
            }
        }

        private void HandleSceneLoaded(
            Scene scene,
            LoadSceneMode mode)
        {
            if (scene.name != SceneName)
                return;

            SceneManager.sceneLoaded -= HandleSceneLoaded;

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                presenter =
                    root.GetComponentInChildren<SummaryPrensenter>(true);

                if (presenter == null)
                    continue;

                presenter.Initialize(
                    runResult,
                    debtResult,
                    gameData);

                presenter.Clicked += HandleContinueClicked;
                return;
            }

            Debug.LogError(
                $"Không tìm thấy SummaryPrensenter trong {SceneName}.");
        }

        private void HandleContinueClicked()
        {
            if (continueHandled)
                return;

            continueHandled = true;

            EndingData ending = endingSystem.CheckEnding();

            if (ending != null)
            {
                stateMachine.ChangeState(
                    new EndingState(ending, stateMachine));

                return;
            }

            prepareNextDay();
        }
    }
}