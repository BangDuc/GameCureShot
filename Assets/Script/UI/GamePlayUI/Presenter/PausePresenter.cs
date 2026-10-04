using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PausePresenter: MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView pauseButton;
    [SerializeField] private UIButtonView resumeButton;
    [SerializeField] private UIButtonView restartButton;
    [SerializeField] private UIButtonView settingButton;
    [SerializeField] private UIButtonView mainMenuButton;
    [SerializeField] private PanelView pausePanel;

    [Header("System")]
    [SerializeField] private RunController controller;

    private void Start()
    {
        controller = RunController.Instance;
    }

    private void OnEnable()
    {

        pauseButton.Clicked += HandlePauseClicked;
        resumeButton.Clicked += HandleResumeClicked;
        restartButton.Clicked += HandleRestartClicked;
        settingButton.Clicked += HandleSettingClicked;
        mainMenuButton.Clicked += HandleMainMenuClicked;
    }

    private void OnDisable()
    {
        pauseButton.Clicked -= HandlePauseClicked;
        resumeButton.Clicked -= HandleResumeClicked;
        restartButton.Clicked -= HandleRestartClicked;
        settingButton.Clicked -= HandleSettingClicked;
        mainMenuButton.Clicked -= HandleMainMenuClicked;
    }

    private void HandlePauseClicked()
    {
        //LogicGame
        controller.Pause();

        //Logic UI
        Time.timeScale = 0f;
        pausePanel.Show();
    }   
    
    private void HandleResumeClicked()
    {
        controller.Resume();
        // Logic UI
        Time.timeScale = 1f;
        pausePanel.Hide();
    }

    private void HandleRestartClicked()
    {

    }

    private void HandleSettingClicked()
    {

    }   
    
    private void HandleMainMenuClicked()
    {

    }    
}
