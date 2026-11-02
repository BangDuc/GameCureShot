using Assets.Script.Core.GameRoot;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePresenter : MonoBehaviour
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
        if (controller == null)
        {
            controller = RunController.Instance;
        }
    }

    private void Update()
    {
        // Hỗ trợ phím Escape để tạm dừng hoặc tiếp tục game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel != null && pausePanel.gameObject.activeSelf)
            {
                HandleResumeClicked();
            }
            else
            {
                HandlePauseClicked();
            }
        }
    }

    private void OnEnable()
    {
        if (pauseButton != null) pauseButton.Clicked += HandlePauseClicked;
        if (resumeButton != null) resumeButton.Clicked += HandleResumeClicked;
        if (restartButton != null) restartButton.Clicked += HandleRestartClicked;
        if (settingButton != null) settingButton.Clicked += HandleSettingClicked;
        if (mainMenuButton != null) mainMenuButton.Clicked += HandleMainMenuClicked;
    }

    private void OnDisable()
    {
        if (pauseButton != null) pauseButton.Clicked -= HandlePauseClicked;
        if (resumeButton != null) resumeButton.Clicked -= HandleResumeClicked;
        if (restartButton != null) restartButton.Clicked -= HandleRestartClicked;
        if (settingButton != null) settingButton.Clicked -= HandleSettingClicked;
        if (mainMenuButton != null) mainMenuButton.Clicked -= HandleMainMenuClicked;
    }

    private void HandlePauseClicked()
    {
        // 1. Dừng hoàn toàn thời gian trong game
        Time.timeScale = 0f;

        // 2. Tạm dừng bộ đếm Run
        if (controller == null) controller = RunController.Instance;
        if (controller != null) controller.Pause();

        // 3. Hiển thị UI Pause Panel
        if (pausePanel != null) pausePanel.Show();
        Debug.Log("<color=yellow>[PausePresenter] Trò chơi đã tạm dừng (Time.timeScale = 0f).</color>");
    }   
    
    private void HandleResumeClicked()
    {
        // 1. Khôi phục thời gian trong game
        Time.timeScale = 1f;

        // 2. Tiếp tục bộ đếm Run
        if (controller == null) controller = RunController.Instance;
        if (controller != null) controller.Resume();

        // 3. Ẩn UI Pause Panel
        if (pausePanel != null) pausePanel.Hide();
        Debug.Log("<color=green>[PausePresenter] Trò chơi tiếp tục (Time.timeScale = 1f).</color>");
    }

    private void HandleRestartClicked()
    {
        // 1. Khôi phục thời gian
        Time.timeScale = 1f;

        // 2. Ẩn panel pause
        if (pausePanel != null) pausePanel.Hide();

        Debug.Log("<color=cyan>[PausePresenter] Người chơi chọn Restart! Đang tải lại màn chơi...</color>");

        // 3. Tải lại màn chơi
        if (GameRoot.Instance != null)
        {
            GameRoot.Instance.RestartCurrentRun();
        }
        else
        {
            if (RunController.Instance != null)
            {
                RunController.Instance.StartRun();
            }
            if (AsyncLoader.Instance != null)
            {
                AsyncLoader.Instance.StartLoading("GamePlaySence");
            }
            else
            {
                SceneManager.LoadScene("GamePlaySence");
            }
        }
    }

    private void HandleSettingClicked()
    {
        Debug.Log("[PausePresenter] Setting clicked.");
    }   
    
    private void HandleMainMenuClicked()
    {
        // 1. Khôi phục thời gian
        Time.timeScale = 1f;

        // 2. Ẩn panel pause
        if (pausePanel != null) pausePanel.Hide();

        Debug.Log("<color=cyan>[PausePresenter] Người chơi chọn về Main Menu...</color>");

        // 3. Quay về Main Menu
        if (GameRoot.Instance != null)
        {
            GameRoot.Instance.ReturnToMainMenu();
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }    
}
