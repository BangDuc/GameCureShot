using Assets.Script.Core.GameRoot;
using System.Collections.Generic;
using UnityEngine;

public class NormalItemSlotPresenter : MonoBehaviour
{
    [SerializeField] private Transform buffContent;
    [SerializeField] private Transform foundContent;
    [SerializeField] private ItemShopView slotPrefab;
    [SerializeField] private ItemDetailView detailView;

    private readonly List<ItemShopView> views = new();
    private ShopManager shopManager;

    public void Initialize(ShopManager shopManager)
    {
        if (shopManager == null)
            throw new System.ArgumentNullException(nameof(shopManager));

        this.shopManager = shopManager;
    }

    private void OnEnable()
    {
        detailView.Selected += HandleBuyButtonClicked;
    }

    private void OnDisable()
    {
        detailView.Selected -= HandleBuyButtonClicked;
    }

    public void BuildProfile()
    {
        if (shopManager == null)
        {
            Debug.LogError(
                "NormalItemSlotPresenter chưa được Initialize.", this);
            return;
        }

        ClearSlots();

        List<ShopOfferData> offers = shopManager.GetNormalOffers();

        foreach (ShopOfferData offer in offers)
        {
            ItemData item = ItemManager.GetItemData(offer.itemId);
            if (item == null)
            {
                Debug.LogError(
                    $"Không tìm thấy ItemData của {offer.itemId}", this);
                continue;
            }

            Transform parent = offer.group == ShopItemGroup.Buff
                ? buffContent
                : foundContent;

            ItemShopView view = Instantiate(slotPrefab, parent, false);
            view.Render(item.itemId, item.icon);
            view.Selected += HandleItemSlotClicked;
            view.gameObject.SetActive(true);

            views.Add(view);
        }
    }

    private void HandleItemSlotClicked(string itemId)
    {
        if (!isActiveAndEnabled || shopManager == null)
            return;

        ShopOfferData offer = shopManager.GetNormalOffer(itemId);
        ItemData item = ItemManager.GetItemData(itemId);
        ShopStockData stock = shopManager.GetNormalStock(itemId);

        if (offer == null || item == null || stock == null)
        {
            Debug.LogError(
                $"Không đủ dữ liệu để hiển thị item {itemId}", this);
            return;
        }

        int owned = shopManager.GetOwnedCount(itemId);
        detailView.Render(itemId, item, offer, stock, owned);
    }

    public void HandleBuyButtonClicked(string itemId, int quantity)
    {
        if (!isActiveAndEnabled || shopManager == null)
            return;

        bool purchased = shopManager.BuyItem(itemId, quantity);
        if(purchased)
        {
            Debug.Log($"Đã mua {quantity} x {itemId}", this);
        }    
        if (!purchased)
        {
            Debug.Log($"Không mua được {quantity} x {itemId}", this);
            return;
        }

        HandleItemSlotClicked(itemId);
    }

    private void ClearSlots()
    {
        foreach (ItemShopView view in views)
        {
            if (view == null)
                continue;

            view.Selected -= HandleItemSlotClicked;
            view.gameObject.SetActive(false);
            Destroy(view.gameObject);
        }

        views.Clear();
    }

    private void OnDestroy()
    {
        ClearSlots();
    }
}