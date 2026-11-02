using Assets.Script.Core.GameRoot;
using System;
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

    public event Action bought;

    public void Initialize(ShopManager shopManager)
    {
        if (shopManager == null)
            throw new ArgumentNullException(nameof(shopManager));

        this.shopManager = shopManager;
    }

    private void OnEnable()
    {
        if (detailView != null)
        {
            detailView.Selected -= HandleBuyButtonClicked;
            detailView.Selected += HandleBuyButtonClicked;
        }
    }

    private void OnDisable()
    {
        if (detailView != null)
        {
            detailView.Selected -= HandleBuyButtonClicked;
        }
    }

    public void BuildProfile()
    {
        if (shopManager == null)
        {
            Debug.LogError("NormalItemSlotPresenter chưa được Initialize.", this);
            return;
        }

        ClearSlots();

        string firstItemId = null;

        // 1. Hiển thị Buff Items trong buffContent (luôn có sẵn trong Shop)
        List<ShopOfferData> buffOffers = shopManager.GetBuffOffers();
        foreach (ShopOfferData offer in buffOffers)
        {
            if (offer == null) continue;
            ItemData item = ItemManager.GetItemData(offer.itemId);
            if (item == null) continue;

            Transform parent = buffContent != null ? buffContent : transform;
            ItemShopView view = Instantiate(slotPrefab, parent, false);
            view.Render(item.itemId, item.Icon);
            view.Selected += HandleItemSlotClicked;
            view.gameObject.SetActive(true);
            views.Add(view);

            if (firstItemId == null)
            {
                firstItemId = offer.itemId;
            }
        }

        // 2. Hiển thị Found Items trong foundContent (chỉ hiện ra khi đã tìm thấy trong Survival Run)
        List<ShopOfferData> unlockedFoundOffers = shopManager.GetUnlockedFoundOffers();
        Debug.Log($"<color=cyan>[NormalItemSlotPresenter] Hiển thị {unlockedFoundOffers.Count} Found item(s) đã mở khóa trong Shop.</color>");
        foreach (ShopOfferData offer in unlockedFoundOffers)
        {
            if (offer == null) continue;
            ItemData item = ItemManager.GetItemData(offer.itemId);
            if (item == null) continue;

            Transform parent = foundContent != null ? foundContent : transform;
            ItemShopView view = Instantiate(slotPrefab, parent, false);
            view.Render(item.itemId, item.Icon);
            view.Selected += HandleItemSlotClicked;
            view.gameObject.SetActive(true);
            views.Add(view);

            if (firstItemId == null)
            {
                firstItemId = offer.itemId;
            }
        }

        // Tự động hiển thị item đầu tiên trên chi tiết
        if (!string.IsNullOrEmpty(firstItemId))
        {
            HandleItemSlotClicked(firstItemId);
        }
    }

    private void HandleItemSlotClicked(string itemId)
    {
        if (!isActiveAndEnabled || shopManager == null || detailView == null)
            return;

        ShopOfferData offer = shopManager.GetNormalOffer(itemId);
        ItemData item = ItemManager.GetItemData(itemId);
        ShopStockData stock = shopManager.GetNormalStock(itemId);

        if (offer == null || item == null || stock == null)
        {
            Debug.LogError($"Không đủ dữ liệu để hiển thị item {itemId}", this);
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
        if (purchased)
        {
            Debug.Log($"<color=green>[NormalItemSlotPresenter] Đã mua thành công {quantity}x {itemId}!</color>", this);
            bought?.Invoke(); // ShopPresenter.RefreshHeader() cập nhật lại tiền mặt trên UI TopBar
            HandleItemSlotClicked(itemId); // Cập nhật lại số lượng và stock trên detailView
        }
        else
        {
            Debug.LogWarning($"[NormalItemSlotPresenter] Không mua được {quantity}x {itemId} (hết hàng hoặc không đủ tiền)!", this);
        }
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
