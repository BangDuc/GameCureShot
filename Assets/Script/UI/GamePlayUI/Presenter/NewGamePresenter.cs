using System;
using UnityEngine;

public class NewGamePresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView NewGameButton;
    [SerializeField] private UIButtonView noButton;
    [SerializeField] private UIButtonView yesButton;
    [SerializeField] private PanelView newGamePanel;

    public event Action Clicked;
    //[Header("System")]
    //[SerializeField] private RunController controller;


    private void OnEnable()
    {
        NewGameButton.Clicked += HandleNewGameClicked;
        noButton.Clicked += HandleNoClicked;
        yesButton.Clicked += HandleYesClicked;
    }

    private void OnDisable()
    {
        NewGameButton.Clicked -= HandleNewGameClicked;
        noButton.Clicked -= HandleNoClicked;
        yesButton.Clicked -= HandleYesClicked;
    }

    private void HandleNewGameClicked()
    {
        newGamePanel.Show();
    }

    private void HandleNoClicked()
    {
        newGamePanel.Hide();
    }

    private void HandleYesClicked()
    {
        Clicked?.Invoke();
    }

}
