using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text ownedText;
    [SerializeField] private TMP_Text stockText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private UIButtonView minusButton;
    [SerializeField] private UIButtonView plusButton;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private TMP_Text totalText;
    [SerializeField] private UIButtonView buttonView;

    private string itemId;
    private int unitPrice;
    private int total;
    private int numberPurchase = 1;
    private bool isSellMode = false;
    private int maxAllowedQuantity = 99;
    private TMP_Text buttonLabelText;

    public bool IsSellMode => isSellMode;
    public string CurrentItemId => itemId;
    public int CurrentQuantity => numberPurchase;

    public event Action<string, int> Selected;
    public event Action<string, int, bool> ActionTriggered;

    private void Awake()
    {
        EnsureButtonLabel();
    }

    private void EnsureButtonLabel()
    {
        if (buttonLabelText == null && buttonView != null)
        {
            buttonLabelText = buttonView.GetComponentInChildren<TMP_Text>(true);
        }
    }

    private void OnEnable()
    {
        EnsureButtonLabel();
        if (buttonView != null) buttonView.Clicked += NotifySelected;
        if (minusButton != null) minusButton.Clicked += HandleMinusClicked;
        if (plusButton != null) plusButton.Clicked += HandlePlusClicked;
    }

    private void OnDisable()
    {
        if (buttonView != null) buttonView.Clicked -= NotifySelected;
        if (minusButton != null) minusButton.Clicked -= HandleMinusClicked;
        if (plusButton != null) plusButton.Clicked -= HandlePlusClicked;
    }

    public void Render(string itemId, ItemData item, ShopOfferData offer, ShopStockData stock, int owned)
    {
        RenderBuy(itemId, item, offer, stock, owned);
    }

    public void RenderBuy(string itemId, ItemData item, ShopOfferData offer, ShopStockData stock, int owned)
    {
        if (string.IsNullOrEmpty(itemId) || item == null) return;

        EnsureButtonLabel();
        this.itemId = itemId;
        this.isSellMode = false;

        if (icon != null) icon.sprite = item.Icon;
        if (nameText != null) nameText.text = item.itemName;
        if (descriptionText != null) descriptionText.text = item.description;
        if (ownedText != null) ownedText.text = "OWNED: " + owned;

        int remStock = stock != null ? stock.remainingStock : 0;
        if (stockText != null)
        {
            stockText.gameObject.SetActive(true);
            stockText.text = "INSTOCK: " + remStock;
        }

        unitPrice = offer != null ? offer.price : 0;
        if (priceText != null) priceText.text = "PRICE: $" + unitPrice;

        maxAllowedQuantity = remStock;
        numberPurchase = remStock > 0 ? 1 : 0;

        if (buttonLabelText != null)
        {
            buttonLabelText.text = "BUY";
        }

        RefreshQuantityAndTotal();

        if (buttonView != null)
        {
            buttonView.SetInteractable(remStock > 0);
        }
    }

    public void RenderSell(string itemId, ItemData item, int sellPrice, int owned)
    {
        if (string.IsNullOrEmpty(itemId) || item == null) return;

        EnsureButtonLabel();
        this.itemId = itemId;
        this.isSellMode = true;

        if (icon != null) icon.sprite = item.Icon;
        if (nameText != null) nameText.text = item.itemName;
        if (descriptionText != null) descriptionText.text = item.description;
        if (ownedText != null) ownedText.text = "OWNED: " + owned;

        if (stockText != null)
        {
            stockText.gameObject.SetActive(true);
            stockText.text = "FOUND ITEM";
        }

        unitPrice = sellPrice;
        if (priceText != null) priceText.text = "SELL PRICE: $" + unitPrice;

        maxAllowedQuantity = owned;
        numberPurchase = owned > 0 ? 1 : 0;

        if (buttonLabelText != null)
        {
            buttonLabelText.text = "SELL";
        }

        RefreshQuantityAndTotal();

        if (buttonView != null)
        {
            buttonView.SetInteractable(owned > 0 && sellPrice > 0);
        }
    }

    private void RefreshQuantityAndTotal()
    {
        total = unitPrice * numberPurchase;

        if (quantityText != null)
        {
            quantityText.text = numberPurchase.ToString();
        }

        if (totalText != null)
        {
            totalText.text = "$" + total;
        }
    }

    private void HandleMinusClicked()
    {
        numberPurchase = Mathf.Max(1, numberPurchase - 1);
        RefreshQuantityAndTotal();
    }

    private void HandlePlusClicked()
    {
        int limit = isSellMode ? maxAllowedQuantity : 99;
        if (limit > 0)
        {
            numberPurchase = Mathf.Min(limit, numberPurchase + 1);
        }
        else
        {
            numberPurchase += 1;
        }
        RefreshQuantityAndTotal();
    }

    private void NotifySelected()
    {
        Selected?.Invoke(itemId, numberPurchase);
        ActionTriggered?.Invoke(itemId, numberPurchase, isSellMode);
    }
}
