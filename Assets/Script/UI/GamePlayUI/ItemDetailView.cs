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
    private int numberPurchase = 1;
    private UIButtonView buttonView;
    public event Action<string, int> Selected;
    private void Awake()
    {
        buttonView = GetComponentInChildren<UIButtonView>();
    }


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
            icon.sprite = item.icon;
            nameText.text = item.itemName;
            descriptionText.text = item.description;
            ownedText.text = ""+owned;
            stockText.text = "" + stock.remainingStock;
            priceText.text = "" + offer.price;
            quantityText.text = "" + numberPurchase;
            totalText.text = $"{offer.price*numberPurchase}";
        }
    }
    private void HanldeMinusClicked()
    {
        numberPurchase -= 1;
    }
    private void HanldePlusClicked()
    {
        numberPurchase += 1;
    }
    private void NotifySelected()
    {
        Selected?.Invoke(itemId, numberPurchase);
    }
}
