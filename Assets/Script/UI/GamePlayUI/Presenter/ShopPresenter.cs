using Assets.Script.Core.GameRoot;
using System;
using UnityEngine;

public class ShopPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UITabShopButtonView normalShopButton;
    [SerializeField] private UITabShopButtonView orionShopButton;
    [SerializeField] private PanelView normalShopPanel;
    [SerializeField] private PanelView orionShopPanel;
    [SerializeField] private NormalItemSlotPresenter normalItemSlotPresenter;
    private ShopManager shopManager;

    public void Initialize(ShopManager shopManager)
    {
        if (shopManager == null)
            throw new ArgumentNullException(nameof(shopManager));

        this.shopManager = shopManager;
        normalItemSlotPresenter.Initialize(shopManager);

        HandleNormalShopClicked();
    }

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
        normalItemSlotPresenter.BuildProfile();
    }

    private void HandleOrionShopClicked()
    {
        normalShopPanel.Hide();
        orionShopPanel.Show();
    }
}
