using UnityEngine;

public class ContinePresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView ContinueButton;
    [SerializeField] private UIButtonView closeButton;
    [SerializeField] private PanelView continuePanel;

    //[Header("System")]
    //[SerializeField] private RunController controller;


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
    }

    private void HandleCloseClicked()
    {
        continuePanel.Hide();
    }
}
