using System;
using TMPro;
using Unity.VisualScripting;
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
    private string itemId;
    private int unitPrice;
    private int total;
    private int numberPurchase = 1;
    [SerializeField] private UIButtonView buttonView;
    public event Action<string, int> Selected;

    private void OnEnable()
    {
        buttonView.Clicked += NotifySelected;
        minusButton.Clicked += HanldeMinusClicked;
        plusButton.Clicked += HanldePlusClicked;

    }
    private void OnDisable()
    {
        buttonView.Clicked -= NotifySelected;
        minusButton.Clicked -= HanldeMinusClicked;
        plusButton.Clicked -= HanldePlusClicked;
    }

    public void Render(string itemId, ItemData item, ShopOfferData offer, ShopStockData stock, int owned)
    {
        if (itemId != null)
        {
            this.itemId = itemId;
            icon.sprite = item.Icon;
            nameText.text = item.itemName;
            descriptionText.text = item.description;
            ownedText.text = "OWNED: "+owned;
            stockText.text = "INSTOCK: " + stock.remainingStock;
            unitPrice = offer.price;
            priceText.text = "PRICE: $" + unitPrice;
            RefreshQuantityAndTotal();
            totalText.text = "$" + total;
        }
    }
    private void RefreshQuantityAndTotal()
    {
        total = unitPrice * numberPurchase;

        quantityText.text = numberPurchase.ToString();
        totalText.text = "$" + total;
    }

    private void HanldeMinusClicked()
    {
        numberPurchase = Mathf.Max(1, numberPurchase - 1);
        RefreshQuantityAndTotal();
    }

    private void HanldePlusClicked()
    {
        numberPurchase += 1;
        RefreshQuantityAndTotal();
    }

    private void NotifySelected()
    {
        Selected?.Invoke(itemId, numberPurchase);    
    }
}
