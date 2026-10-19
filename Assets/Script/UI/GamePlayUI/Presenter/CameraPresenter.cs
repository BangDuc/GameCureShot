using UnityEngine;

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
        cameraButton.Clicked += HandleCameraClicked;
        exitCameraButton.Clicked += HandleExitZoomClicked;
        CaptureCameraButton.Clicked += HandleCaptureCameraClicked;
    }

    private void OnDisable()
    {
        cameraButton.Clicked -= HandleCameraClicked;
        exitCameraButton.Clicked -= HandleExitZoomClicked;
        CaptureCameraButton.Clicked -= HandleCaptureCameraClicked;
    }

    private void HandleCameraClicked()
    {
        if (cameraPanel.isActiveAndEnabled)
        {
            cameraPanel.Hide();
            cameraMoving.ChangeCameraGamePlayMode();

            return;
        }
        cameraPanel.Show();
        
        cameraMoving.ChangeCameraPhotoMode();

    }

    private void HandleExitZoomClicked()
    {
        if (!CameraMoving.Instance.isZoomMode) return;

        CameraMoving.Instance.ChangeCameraGamePlayMode();

    }

    private void HandleCaptureCameraClicked()
    {
       CameraManager.Instance.Capture();

    }

}
