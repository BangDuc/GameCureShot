using Core.Interact;
using Core.Item;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DroppedItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemId;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private ItemData itemData;
    private bool isPickedUp = false;

    public string ItemId => itemId;
    public ItemData ItemData => itemData;

    private void Awake()
    {
        gameObject.layer = 8; // Layer 8: Interactable

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }
        }

        var col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    public void Initialize(string id, ItemData data = null)
    {
        itemId = id;
        itemData = data ?? ItemManager.GetItemData(id);

        if (itemData != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = itemData.Icon;
            spriteRenderer.sortingOrder = 15; // Nằm trên nền và tủ
        }

        // Tự động thêm hoặc chỉnh BoxCollider2D vừa vặn với kích thước sprite
        var boxCol = GetComponent<BoxCollider2D>();
        if (boxCol == null)
        {
            boxCol = gameObject.AddComponent<BoxCollider2D>();
        }
        boxCol.isTrigger = true;
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            boxCol.size = spriteRenderer.sprite.bounds.size;
        }
    }

    public bool CanInteract()
    {
        return !isPickedUp && !string.IsNullOrEmpty(itemId);
    }

    public bool Interact()
    {
        if (isPickedUp) return false;
        isPickedUp = true;

        if (itemData == null)
        {
            itemData = ItemManager.GetItemData(itemId);
        }

        if (itemData != null)
        {
            var itemMgr = Core.Item.ItemManager.Instance;
            if (itemMgr != null && itemMgr.InventoryItem != null)
            {
                itemMgr.InventoryItem.PickUpItem(itemData);
                Debug.Log($"<color=green>[DroppedItemPickup] Đã nhặt vật phẩm: {itemData.itemName} ({itemId})!</color>");
            }

            if (RunController.Instance != null && RunController.Instance.GameData != null)
            {
                var ws = RunController.Instance.GameData.worldStateData;
                if (ws != null)
                {
                    if (ws.collectedItemIds == null) ws.collectedItemIds = new System.Collections.Generic.List<string>();
                    if (!ws.collectedItemIds.Contains(itemId))
                    {
                        ws.collectedItemIds.Add(itemId);
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning($"[DroppedItemPickup] Không tìm thấy dữ liệu cho ItemId: {itemId}");
        }

        // Hiệu ứng nhặt đồ: nảy nhẹ rồi thu nhỏ biến mất
        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack);
        transform.DOMoveY(transform.position.y + 0.35f, 0.25f).OnComplete(() =>
        {
            Destroy(gameObject);
        });

        return true;
    }

    public void OnHoverEnter()
    {
        if (isPickedUp) return;
        transform.DOScale(Vector3.one * 1.15f, 0.15f);
    }

    public void OnHoverExit()
    {
        if (isPickedUp) return;
        transform.DOScale(Vector3.one, 0.15f);
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}
