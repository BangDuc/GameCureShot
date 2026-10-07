using Assets.Script.Core.GameRoot.GameState;
using System;
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
            ShopStockData orion = new ShopStockData
            {
                itemId = "test_item",
                remainingStock = 2
            };
            GameData data = new GameData
            {
                currentDay = 1,
                cameraCooldownLevel = 1,
                cameraDamageReductionLevel = 1,
                cameraDoubleMoneyLevel = 1,
                consecutiveUnpaidDays = 0,
                currentPhase = 1,
                IsORION_Unlocked = false,
                isSealBroken = false,
                money = 0,
                outstandingDebt = 0,
                permanentHealth = 4,
                resumepoint = ResumePoint.shop,
                playtime = 5400f
            };
            data.orionShopStock.Add(orion);
            SaveSystem.Save(data, 0);
            Debug.Log(Application.persistentDataPath);
        }
    }
}

