using UnityEngine;

namespace Assets.Script.Core.GameRoot.GameState
{
    public class MainMenuState : IGameState
    {
        public void Enter()
        {
            Debug.Log("Enter MainMenuState");
        }

        public void Exit()
        {
            Debug.Log("Exit MainMenuState");
        }
    }
}
