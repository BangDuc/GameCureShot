using UnityEngine;

public class ExitPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView exitButton;
    [SerializeField] private UIButtonView noButton;
    [SerializeField] private UIButtonView yesButton;
    [SerializeField] private PanelView exitPanel;

    //[Header("System")]
    //[SerializeField] private RunController controller;


    private void OnEnable()
    {
        exitButton.Clicked += HandleExitClicked;
        noButton.Clicked += HandleNoClicked;
        yesButton.Clicked += HandleYesClicked;
    }

    private void OnDisable()
    {
        exitButton.Clicked -= HandleExitClicked;
        noButton.Clicked -= HandleNoClicked;
        yesButton.Clicked -= HandleYesClicked;
    }

    private void HandleExitClicked()
    {
        exitPanel.Show();
    }

    private void HandleNoClicked()
    {
        exitPanel.Hide();
    }

    private void HandleYesClicked()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
