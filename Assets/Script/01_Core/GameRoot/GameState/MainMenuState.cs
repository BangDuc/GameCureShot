using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Core.GameRoot.GameState
{
    public class MainMenuState : IGameState
    {
        private MainMenuPresenter mainMenuPresenter;
        private SettingPresenter settingPresenter;
        private event Action startNewGame;
        private event Action<int> loadGame;
        public MainMenuState(
            Action startNewGame,
            Action<int> loadGame)
        {
            this.startNewGame = startNewGame;
            this.loadGame = loadGame;
        }
        public void Enter()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
            AsyncLoader.Instance.StartLoading("SurvivalRunScene");
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            mainMenuPresenter.NewGame -= HandleChangeStateNewGame;
            mainMenuPresenter.Continue -= HandleChangeStateContinue;
        }

        private void HandleSceneLoaded(
            Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "SurvivalRunScene")
                return;

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                mainMenuPresenter =
                root.GetComponentInChildren<MainMenuPresenter>(true);

                if (mainMenuPresenter == null)
                    continue;
                mainMenuPresenter.NewGame += HandleChangeStateNewGame;
                mainMenuPresenter.Continue += HandleChangeStateContinue;
                return;
            }

            Debug.LogError(
                "Không tìm thấy MainMenuPresenter trong MainMenuScene.");
        }

        private void HandleChangeStateNewGame()
        {
            startNewGame?.Invoke();
        }

        private void HandleChangeStateContinue(int slotId)
        {
            loadGame?.Invoke(slotId);
        }
    }
}
