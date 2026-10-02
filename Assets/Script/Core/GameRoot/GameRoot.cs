using Assets.Script.Core.GameRoot.GameState;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Core.GameRoot
{
    public class GameRoot : MonoBehaviour
    {
        private GameSession gameSession;
        private GameStateMachine gameState;
        private void Awake()
        {
            Initialzie();
            DontDestroyOnLoad(this);
        }

        private void Initialzie()
        {
            gameSession = new GameSession();
            gameState = new GameStateMachine(new MainMenuState());
        }
        [ContextMenu("Chạy Hàm Test")]
        public void StartGame()
        {
            AsyncLoader.Instance.StartLoading("GamePlaySence");
        }

    }
}

