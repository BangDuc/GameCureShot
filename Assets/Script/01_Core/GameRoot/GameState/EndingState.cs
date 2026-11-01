using Assets.Script.Core.GameRoot;
using Assets.Script.Core.GameRoot.GameState;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingState : IGameState
{
    private readonly GameStateMachine stateMachine;
    private readonly EndingData endingData;
    private EndingPresenter presenter;
    public event Action mainMenu;
    public EndingState(EndingData endingData, GameStateMachine stateMachine, Action mainMenu)
    {
        this.endingData = endingData;
        this.stateMachine = stateMachine;
        this.mainMenu = mainMenu;
    }
    public void Enter()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
        AsyncLoader.Instance.StartLoading("EndingScene");
    }

    public void Exit()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
        if (presenter != null)
        {
            presenter.ReturnToMenuRequested -= HandleChangeState;
        }
    }

    private void HandleSceneLoaded(
    Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "EndingScene")
            return;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            presenter = root.GetComponentInChildren<EndingPresenter>(true);

            if (presenter == null)
                continue;
            presenter.ReturnToMenuRequested += HandleChangeState;
            presenter.Initialize(endingData);
            return;
        }

        Debug.LogError(
            "Không tìm thấy EndingPresenter trong EndingScene.");
    }

    private void HandleChangeState()
    {
        mainMenu?.Invoke();
    }
}
