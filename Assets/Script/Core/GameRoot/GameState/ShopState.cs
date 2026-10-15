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
            stateMachine.ChangeState(new SurvivalState());
        }
    }
}