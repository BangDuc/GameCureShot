using UnityEngine;

public class NormalShopPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UITabShopButtonView purchaseButton;
    [SerializeField] private UITabShopButtonView cameraShopButton;
    [SerializeField] private PanelView purchasePanel;
    [SerializeField] private PanelView cameraPanel;
    [SerializeField] private PanelView purchaseDetail;
    [SerializeField] private PanelView cameraDetail;

    private void OnEnable()
    {
        purchaseButton.Clicked += HandlePurchaseClicked;
        cameraShopButton.Clicked += HandleCameraClicked;
    }

    private void Start()
    {
        HandlePurchaseClicked();
    }

    private void OnDisable()
    {
        purchaseButton.Clicked -= HandlePurchaseClicked;
        cameraShopButton.Clicked -= HandleCameraClicked;
    }

    private void HandlePurchaseClicked()
    {
        cameraPanel.Hide();
        cameraDetail.Hide();
        purchasePanel.Show();
        purchaseDetail.Show();
    }

    private void HandleCameraClicked()
    {
        purchasePanel.Hide();
        purchaseDetail.Hide();
        cameraPanel.Show();
        cameraDetail.Show();
    }
}
