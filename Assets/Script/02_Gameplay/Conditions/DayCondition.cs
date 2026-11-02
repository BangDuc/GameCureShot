using Assets.Script.Core.GameRoot;
using System.Collections;
using UnityEngine;

namespace Assets.Script.Condition
{
    public class DayCondition : ICondition
    {
        private readonly GameData gameData;
        public DayCondition(GameData gameData)
        {
            this.gameData = gameData;
        }
        public bool Evaluate()
        {
            return gameData.currentDay == 7; 
        }
    }
}