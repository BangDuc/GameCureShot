using Assets.Script.Core.GameRoot.GameState;
using UnityEngine;

namespace Assets.Script.Core.GameRoot
{
    public class GameStateMachine
    {
        private IGameState currentState;
        public IGameState CurrentState => currentState;

        public GameStateMachine(IGameState state)
        {
            if (state == null)
            {
                Debug.LogError("[GameStateMachine] State khởi đầu bị null!");
                return;
            }

            currentState = state;
            Debug.Log($"<color=yellow>[GameStateMachine] Khởi tạo State ban đầu: {currentState.GetType().Name}</color>");
            currentState.Enter();
        }

        public void ChangeState(IGameState newstate, bool force = false)
        {
            if (newstate == null)
            {
                Debug.LogError("[GameStateMachine] Không thể chuyển sang state null!");
                return;
            }

            if (!force && currentState != null && currentState.GetType() == newstate.GetType())
            {
                Debug.LogWarning($"[GameStateMachine] Đã ở trong state {currentState.GetType().Name}, bỏ qua ChangeState.");
                return;
            }

            string oldName = currentState != null ? currentState.GetType().Name : "None";
            string newName = newstate.GetType().Name;
            Debug.Log($"<color=yellow>[GameStateMachine] Chuyển State: {oldName} -> {newName}</color>");

            try
            {
                currentState?.Exit();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[GameStateMachine] Lỗi khi Exit state {oldName}: {ex}");
            }

            currentState = newstate;

            try
            {
                currentState.Enter();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[GameStateMachine] Lỗi khi Enter state {newName}: {ex}");
            }
        }
    }
}
