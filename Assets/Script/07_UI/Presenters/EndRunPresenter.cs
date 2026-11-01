using Assets.Script.Core.GamePlay;
using System;
using UnityEngine;

public class EndRunPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView endRunButton;
    [SerializeField] private UIButtonView noButton;
    [SerializeField] private UIButtonView yesButton;
    [SerializeField] private PanelView endRunPanel;

    public event Action EndRunRequested;

    private void OnEnable()
    {
        endRunButton.Clicked += HandleEndRunClicked;
        noButton.Clicked += HandleNoClicked;
        yesButton.Clicked += HandleYesClicked;
    }

    private void OnDisable()
    {
        endRunButton.Clicked -= HandleEndRunClicked;
        noButton.Clicked -= HandleNoClicked;
        yesButton.Clicked -= HandleYesClicked;
    }

    private void HandleEndRunClicked()
    {
        Time.timeScale = 0f;
        endRunPanel.Show();
    }

    private void HandleNoClicked()
    {
        Time.timeScale = 1f;
        endRunPanel.Hide();
    }

    private void HandleYesClicked()
    {
        EndRunRequested?.Invoke();
    }
}


