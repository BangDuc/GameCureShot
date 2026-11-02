using Assets.Script.Core.GameRoot;
using UnityEngine;

namespace Assets.Script.Condition
{
    public class HealthCondition : ICondition
    {
        private readonly GameData gameData;
        public HealthCondition(GameData gameData)
        {
            this.gameData = gameData;
        }

        public bool Evaluate()
        {
            return gameData.permanentHealth >= 1;
        }
    }
}