using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Core.GameRoot.GameState
{
    public class MainMenuState : IGameState
    {
        private const string SceneName = "MainMenu";
        private MainMenuPresenter mainMenuPresenter;
        private readonly Action startNewGame;
        private readonly Action<int> loadGame;

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

            if (mainMenuPresenter != null)
            {
                mainMenuPresenter.NewGame -= HandleChangeStateNewGame;
                mainMenuPresenter.Continue -= HandleChangeStateContinue;
                mainMenuPresenter = null;
            }
        }

        private void HandleSceneLoaded(
            Scene scene, LoadSceneMode mode)
        {
            if (scene.name != SceneName)
                return;

            HookPresenter(scene);
        }

        private void HookPresenter(Scene scene)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                mainMenuPresenter =
                    root.GetComponentInChildren<MainMenuPresenter>(true);

                if (mainMenuPresenter == null)
                    continue;

                mainMenuPresenter.NewGame -= HandleChangeStateNewGame;
                mainMenuPresenter.NewGame += HandleChangeStateNewGame;
                mainMenuPresenter.Continue -= HandleChangeStateContinue;
                mainMenuPresenter.Continue += HandleChangeStateContinue;
                return;
            }

            Debug.LogError(
                $"Không tìm thấy MainMenuPresenter trong {SceneName}.");
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
