using Assets.Script.Core.GameRoot;
using System;
using TMPro;
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
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private TMP_Text debtText;
    [SerializeField] private TMP_Text healthText;
    public event Action Clicked;
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
    private void RefreshHeader()
    {
        dayText.text = $"DAY {shopManager.CurrentDay}";
        cashText.text = $"CASH ${shopManager.Money}";
        debtText.text = $"OUTSTANDING DEBT ${shopManager.OutstandingDebt}";
        healthText.text = $"PERMANENT HEALTH {shopManager.PermanentHealth}/4";
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
