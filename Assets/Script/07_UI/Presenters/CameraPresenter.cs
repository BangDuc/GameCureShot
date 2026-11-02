using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView cameraButton;
    [SerializeField] private UIButtonView exitCameraButton;
    [SerializeField] private UIButtonView CaptureCameraButton;
    [SerializeField] private PanelView cameraPanel;

    [Header("System")]
    [SerializeField] private RunController controller;
    [SerializeField] private CameraMoving cameraMoving;

    private void Start()
    {
        controller = RunController.Instance;
        cameraMoving = CameraMoving.Instance;
    }

    private void OnEnable()
    {
        if (cameraButton != null) cameraButton.Clicked += HandleCameraClicked;
        if (exitCameraButton != null) exitCameraButton.Clicked += HandleExitZoomClicked;
        if (CaptureCameraButton != null) CaptureCameraButton.Clicked += HandleCaptureCameraClicked;
    }

    private void OnDisable()
    {
        if (cameraButton != null) cameraButton.Clicked -= HandleCameraClicked;
        if (exitCameraButton != null) exitCameraButton.Clicked -= HandleExitZoomClicked;
        if (CaptureCameraButton != null) CaptureCameraButton.Clicked -= HandleCaptureCameraClicked;
    }

    private void Update()
    {
        // Phím tắt Space để chụp ảnh khi đang ở chế độ Camera Panel
        if (cameraPanel != null && cameraPanel.isActiveAndEnabled)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                HandleCaptureCameraClicked();
            }
        }
    }

    private void HandleCameraClicked()
    {
        if (cameraPanel == null) return;
        if (cameraMoving == null) cameraMoving = CameraMoving.Instance;

        if (cameraPanel.isActiveAndEnabled)
        {
            cameraPanel.Hide();
            if (cameraMoving != null) cameraMoving.ChangeCameraGamePlayMode();
            return;
        }

        cameraPanel.Show();
        if (cameraMoving != null) cameraMoving.ChangeCameraPhotoMode();
    }

    private void HandleExitZoomClicked()
    {
        if (CameraMoving.Instance != null && CameraMoving.Instance.isZoomMode)
        {
            CameraMoving.Instance.ChangeCameraGamePlayMode();
        }
    }

    private void HandleCaptureCameraClicked()
    {
        if (CameraManager.Instance != null)
        {
            CameraManager.Instance.Capture();
        }
    }
}
