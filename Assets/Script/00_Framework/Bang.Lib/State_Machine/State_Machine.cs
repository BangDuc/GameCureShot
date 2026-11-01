using System.Collections.Generic;
using UnityEngine;
namespace Bang.Lib.StateMachine
{
    public class State_Machine
    {
        Dictionary<string, IState> dic_states = new Dictionary<string, IState>();

        IState currentState;
        string lastState_id;
        string currentState_id;

        public void ChangeState(string newStateID)
        {
            var newState = dic_states[newStateID];

            if (currentState == newState) return;
            lastState_id = currentState_id;
            currentState.Exit();

            currentState_id = newStateID;

            currentState = newState;

            currentState.Enter();
        }
        public void Execute()
        {
            if (currentState == null) return;
            currentState.Update();
        }
        public virtual void Initialize(string startedStateID)
        {
            currentState = dic_states[startedStateID];
            currentState_id = startedStateID;
            lastState_id = startedStateID;

            currentState.Enter();
        }
        public void AddState(string newstateid, IState newstate)
        {
            if (dic_states.ContainsKey(newstateid))
            {
                return;
            }
            dic_states.Add(newstateid, newstate);

        }
    }
    public interface IState
    {
        void Enter();

        void Update();

        void Exit();
    }
}

