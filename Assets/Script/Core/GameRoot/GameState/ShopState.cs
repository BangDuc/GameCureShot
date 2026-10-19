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

        public ShopState(ShopManager shopManager, GameStateMachine stateMachine)
        {
            this.shopManager = shopManager;
            this.stateMachine = stateMachine;
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
            stateMachine.ChangeState(new SurvivalState());
            Debug.Log("Da an continue");
        }

        private void HandleChangeStateMenu()
        {
            stateMachine.ChangeState(new MainMenuState());
            Debug.Log("Da an menu");
        }
    }
}