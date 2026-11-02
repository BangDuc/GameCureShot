using System;
using UnityEngine;

public class MainMenuPresenter : MonoBehaviour
{
    [SerializeField] private NewGamePresenter newGamePresenter;
    [SerializeField] private ContinePresenter continePresenter;
    public event Action NewGame;
    public event Action<int> Continue;
    private void OnEnable()
    {
        newGamePresenter.Clicked += HandleNewGameClicked;
        continePresenter.ClickedSave += HandleContinueClicked;
    }
    private void OnDisable()
    {
        newGamePresenter.Clicked -= HandleNewGameClicked;
        continePresenter.ClickedSave -= HandleContinueClicked;
    }
    private void HandleNewGameClicked()
    {
        NewGame?.Invoke();
    }

    private void HandleContinueClicked(int slotId)
    {
        Continue?.Invoke(slotId);
    }
}
