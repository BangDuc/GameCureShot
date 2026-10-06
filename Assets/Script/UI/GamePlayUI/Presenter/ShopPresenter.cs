using UnityEngine;

public class ShopPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UITabShopButtonView normalShopButton;
    [SerializeField] private UITabShopButtonView orionShopButton;
    [SerializeField] private PanelView normalShopPanel;
    [SerializeField] private PanelView orionShopPanel;

    //[Header("System")]
    //[SerializeField] private RunController controller;


    private void OnEnable()
    {
        normalShopButton.Clicked += HandleNormalShopClicked;
        orionShopButton.Clicked += HandleOrionShopClicked;
    }

    private void OnDisable()
    {
        normalShopButton.Clicked -= HandleNormalShopClicked;
        orionShopButton.Clicked -= HandleOrionShopClicked;
    }

    private void HandleNormalShopClicked()
    {
        orionShopPanel.Hide();
        normalShopPanel.Show();
    }

    private void HandleOrionShopClicked()
    {
        normalShopPanel.Hide();
        orionShopPanel.Show();
    }
}
