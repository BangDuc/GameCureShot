using Assets.Script.Core.GameRoot;
using System;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private UIButtonView MainMenuButton;
    [SerializeField] private TopBarView topBarView;
    public event Action Continue;

    public event Action Menu;
    private ShopManager shopManager;

    public void Initialize(ShopManager shopManager)
    {
        if (shopManager == null)
            throw new ArgumentNullException(nameof(shopManager));

        this.shopManager = shopManager;
        normalItemSlotPresenter.Initialize(shopManager);

        HandleNormalShopClicked();
        RefreshHeader();
    }

    private void OnEnable()
    {
        continueButton.Clicked += HandleContinueClicked;
        MainMenuButton.Clicked += HandleMenuCLicked;
        normalShopButton.Clicked += HandleNormalShopClicked;
        orionShopButton.Clicked += HandleOrionShopClicked;
        normalItemSlotPresenter.bought += RefreshHeader;
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
        MainMenuButton.Clicked -= HandleMenuCLicked;
        normalShopButton.Clicked -= HandleNormalShopClicked;
        orionShopButton.Clicked -= HandleOrionShopClicked;
        normalItemSlotPresenter.bought -= RefreshHeader;
    }
    public void RefreshHeader()
    {
        topBarView.Render(
            $"DAY {shopManager.CurrentDay}",
            $"CASH ${shopManager.Money}",
            $"OUTSTANDING DEBT ${shopManager.OutstandingDebt}",
            $"PERMANENT HEALTH {shopManager.PermanentHealth}");
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
        Continue?.Invoke();
    }

    private void HandleMenuCLicked()
    {
        Menu?.Invoke();
    }    
}
