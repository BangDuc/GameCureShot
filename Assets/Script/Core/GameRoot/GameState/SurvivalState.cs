using System.Collections;
using UnityEngine;

namespace Assets.Script.Core.GameRoot.GameState
{
    public class SurvivalState : IGameState
    {
        public void Enter()
        {
            AsyncLoader.Instance.StartLoading("PlayGameScene");
        }

        public void Exit()
        {
            
        }
    }
}