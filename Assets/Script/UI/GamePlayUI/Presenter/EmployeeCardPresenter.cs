using UnityEngine;

public class EmployeeCardPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView infoButton;
    [SerializeField] private PanelView infoPanel;

    [Header("System")]
    [SerializeField] private RunController controller;

    private void Start()
    {
        controller = RunController.Instance;
    }

    private void OnEnable()
    {
        infoButton.Clicked += HandleInfoClicked;
    }

    private void OnDisable()
    {
        infoButton.Clicked -= HandleInfoClicked;
    }

    private void HandleInfoClicked()
    {
        if(infoPanel.isActiveAndEnabled)
        {
            infoPanel.Hide();
            return;
        }
        infoPanel.Show();
    }
}
