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
    private List<ShopStockData> stocks = new();
    private Inventory inventory;
    private Transform parent;

    public void BuildProfile()
    {
        List<ItemData> items = GetListNormalShopItem();
        ClearSlots();
        foreach (ItemData item in items)
        {
            ShopOfferData offer = ItemManager.GetNormalShopItemData(item.itemId);
            parent = offer.group == ShopItemGroup.Buff ? buffContent : foundContent;
            ItemShopView view = Instantiate(slotPrefab, parent, false);
            view.gameObject.SetActive(true);
            view.Render(item.itemId, item.icon);
            view.Selected += HandleItemSlotClicked;
            views.Add(view);
        }
    }

    public List<ItemData> GetListNormalShopItem()
    {
        List<ShopOfferData> normal = ItemManager.GetNormalShopItemDatas();
        List<ItemData> items = new();
        foreach (ShopOfferData offer in normal)
        {
            ItemData item = ItemManager.GetItemData(offer.itemId);
            items.Add(item);
            if (FindStock(offer.itemId) == null)
            {
                stocks.Add(new ShopStockData
                {
                    itemId = offer.itemId,
                    remainingStock = offer.initialStock
                });
            }
        }
        return items;
    }

    private void HandleItemSlotClicked(string itemId)
    {

        if (!isActiveAndEnabled)
            return;
        ShopOfferData offer = ItemManager.GetNormalShopItemData(itemId);
        ItemData item = ItemManager.GetItemData(itemId);
        ShopStockData stock = FindStock(itemId);

        if (stock == null)
        {
            Debug.LogError($"Không tìm thấy stock của {itemId}", this);
            return;
        }

        detailView.Render(itemId, item, offer, stock, 1); // tạm
        Debug.Log($"vừa nhấn save {itemId}", this);
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
    private ShopStockData FindStock(string itemId)
    {
        foreach (ShopStockData stock in stocks)
        {
            if (stock.itemId == itemId)
                return stock;
        }

        return null;
    }
}
