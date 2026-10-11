using UnityEngine;

public class CameraPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView cameraButton;
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
    }

    private void OnDisable()
    {
        cameraButton.Clicked -= HandleCameraClicked;
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
}
