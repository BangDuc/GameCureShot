using UnityEngine;

public class ContinePresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView ContinueButton;
    [SerializeField] private UIButtonView closeButton;
    [SerializeField] private PanelView continuePanel;

    [Header("Save List")]
    [SerializeField] private SavePresenter savePresenter;

    private void OnEnable()
    {
        ContinueButton.Clicked += HandleContinueClicked;
        closeButton.Clicked += HandleCloseClicked;
    }

    private void OnDisable()
    {
        ContinueButton.Clicked -= HandleContinueClicked;
        closeButton.Clicked -= HandleCloseClicked;
    }

    private void HandleContinueClicked()
    {
        continuePanel.Show();
        savePresenter.BuildProfile();
    }

    private void HandleCloseClicked()
    {
        continuePanel.Hide();
    }
}