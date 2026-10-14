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
    [SerializeField] private UIButtonView continueButton;
    public event Action Clicked;
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
        continueButton.Clicked += HandleContinueClicked;
        normalShopButton.Clicked += HandleNormalShopClicked;
        orionShopButton.Clicked += HandleOrionShopClicked;
    }

    private void Start()
    {
        if (shopManager != null)
            return;
        GameData data = new GameData
        {
            currentDay = 1,
            money = 500,
            inventoryData = new InventoryData()
        };
        Inventory inventory = new Inventory(data.inventoryData);
        DebtManager debtManager = new DebtManager(data);
        ShopManager testShop = new ShopManager(inventory, data, debtManager);
        Initialize(testShop);
    }

    private void OnDisable()
    {
        continueButton.Clicked -= HandleContinueClicked;
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

    private void HandleContinueClicked()
    {
        Clicked?.Invoke();
    }    
}
