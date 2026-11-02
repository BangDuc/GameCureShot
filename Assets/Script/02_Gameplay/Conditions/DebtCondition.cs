using Assets.Script.Core.GameRoot;
using System.Collections;
using UnityEngine;

namespace Assets.Script.Condition
{
    public class DebtCondition : ICondition
    {
        private readonly GameData gameData;
        public DebtCondition(GameData gameData)
        {
            this.gameData = gameData;
        }
        public bool Evaluate()
        {
            return gameData.consecutiveUnpaidDays < 3;
        }
    }
}