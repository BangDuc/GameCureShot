using UnityEngine;

public class CameraPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView cameraButton;
    [SerializeField] private PanelView cameraPanel;

    [Header("System")]
    [SerializeField] private RunController controller;

    private void Start()
    {
        controller = RunController.Instance;
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
            return;
        }
        cameraPanel.Show();
    }
}
