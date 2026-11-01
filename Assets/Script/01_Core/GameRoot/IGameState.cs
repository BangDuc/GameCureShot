using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Script.Core.GameRoot
{
    public interface IGameState
    {
        public void Enter();
        public void Exit();
    }
}
