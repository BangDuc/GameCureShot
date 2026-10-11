using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Core.GameRoot.GameState
{
    public class ShopState : IGameState
    {
        private readonly ShopManager shopManager;

        public ShopState(ShopManager shopManager)
        {
            this.shopManager = shopManager;
        }

        public void Enter()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
            AsyncLoader.Instance.StartLoading("ShopScene");
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void HandleSceneLoaded(
            Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "ShopScene")
                return;

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                ShopPresenter presenter =
                    root.GetComponentInChildren<ShopPresenter>(true);

                if (presenter == null)
                    continue;

                presenter.Initialize(shopManager);
                return;
            }

            Debug.LogError(
                "Không tìm thấy ShopPresenter trong ShopScene.");
        }
    }
}