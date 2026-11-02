using Assets.Script.Core.GameRoot;
using Assets.Script.Core.GameRoot.GameState;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingState : IGameState
{
    private const string SceneName = "Ending";
    private readonly GameStateMachine stateMachine;
    private readonly EndingData endingData;
    private EndingPresenter presenter;
    private readonly Action mainMenu;

    public EndingState(EndingData endingData, GameStateMachine stateMachine, Action mainMenu)
    {
        this.endingData = endingData;
        this.stateMachine = stateMachine;
        this.mainMenu = mainMenu;
    }

    public void Enter()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;

        if (SceneManager.GetActiveScene().name == SceneName)
        {
            HookPresenter(SceneManager.GetActiveScene());
        }
        else
        {
            AsyncLoader.Instance.StartLoading(SceneName);
        }
    }

    public void Exit()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;

        if (presenter != null)
        {
            presenter.ReturnToMenuRequested -= HandleChangeState;
            presenter = null;
        }
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != SceneName)
            return;

        HookPresenter(scene);
    }

    private void HookPresenter(Scene scene)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            presenter = root.GetComponentInChildren<EndingPresenter>(true);
            if (presenter != null)
                break;
        }

        if (presenter == null)
        {
            Debug.LogWarning($"[EndingState] Chưa có component EndingPresenter trên scene {SceneName}, tự động tìm hoặc gắn component...");
            
            var presenterGo = GameObject.Find("EndingPresenter");
            if (presenterGo == null)
            {
                presenterGo = GameObject.Find("Presenter");
            }
            if (presenterGo == null)
            {
                presenterGo = new GameObject("EndingPresenter");
            }

            presenter = presenterGo.GetComponent<EndingPresenter>();
            if (presenter == null)
            {
                presenter = presenterGo.AddComponent<EndingPresenter>();
            }

            var view = presenterGo.GetComponent<EndingView>();
            if (view == null)
            {
                view = presenterGo.AddComponent<EndingView>();
            }
            view.AutoFindUIComponents();
            presenter.Setup(view);
        }

        presenter.ReturnToMenuRequested -= HandleChangeState;
        presenter.ReturnToMenuRequested += HandleChangeState;
        presenter.Initialize(endingData);
        Debug.Log($"<color=green>[EndingState] Khởi tạo thành công EndingPresenter cho Ending: {endingData.endingId} ({endingData.endingType} - {endingData.endingTitle})</color>");
    }

    private void HandleChangeState()
    {
        mainMenu?.Invoke();
    }
}
