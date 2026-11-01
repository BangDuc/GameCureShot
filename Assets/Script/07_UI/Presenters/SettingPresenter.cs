using UnityEngine;

public class SettingPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView settingButton;
    [SerializeField] private UIButtonView closeButton;
    [SerializeField] private UIButtonView audioButton;
    [SerializeField] private UIButtonView visualButton;
    [SerializeField] private PanelView settingPanel;
    [SerializeField] private PanelView audioPanel;
    [SerializeField] private PanelView visualPanel;

    //[Header("System")]
    //[SerializeField] private RunController controller;


    private void OnEnable()
    {
        settingButton.Clicked += HandleSettingClicked;
        closeButton.Clicked += HandleCloseClicked;
        audioButton.Clicked += HandleAudioClicked;
        visualButton.Clicked += HandleVisualClicked;
    }

    private void OnDisable()
    {
        settingButton.Clicked -= HandleSettingClicked;
        closeButton.Clicked -= HandleCloseClicked;
        audioButton.Clicked -= HandleAudioClicked;
        visualButton.Clicked -= HandleVisualClicked;
    }

    private void HandleSettingClicked()
    {
        settingPanel.Show();
    }

    private void HandleCloseClicked()
    {
        settingPanel.Hide();
    }
    private void HandleAudioClicked()
    {
        visualPanel.Hide();
        audioPanel.Show();
    }
    private void HandleVisualClicked()
    {
        audioPanel.Hide();
        visualPanel.Show();
    }
}
