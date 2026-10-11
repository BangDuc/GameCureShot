using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Core.GameRoot
{
    public class GameSession
    {
        private GameData gamedata;
        public GameData Data => gamedata;

        public void SetData(GameData data)
        {
            if (data == null)
                throw new System.ArgumentNullException(nameof(data));

            gamedata = data;
        }

        public void OnPermanentHealthLoss()
        {
            gamedata.permanentHealth -= 1;
        }

        public void StartNewGame(int startingMoney)
        {
            SetData(new GameData
            {
                currentDay = 1,
                currentPhase = 1,
                permanentHealth = 4,
                money = startingMoney,
                cameraCooldownLevel = 1,
                cameraDamageReductionLevel = 1,
                cameraDoubleMoneyLevel = 1,
            });
            SaveSystem.Save(gamedata, 0);
        }

        public void AdvanceDay()
        {
            gamedata.currentDay++;
        }
    }
}
