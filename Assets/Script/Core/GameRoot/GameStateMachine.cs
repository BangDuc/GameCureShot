using Assets.Script.Core.GameRoot.GameState;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Jobs;

namespace Assets.Script.Core.GameRoot
{
    public class GameStateMachine
    {
        public GameStateMachine(IGameState state)
        {
            currentState = state;
            currentState.Enter();
        }
        private IGameState currentState;

        public void ChangeState(IGameState newstate)
        {
            if (newstate == null) return;
            if (currentState.GetType()== newstate.GetType()) return;
            currentState.Exit();
            currentState = newstate;
            currentState.Enter();
        }
       


    }
}
