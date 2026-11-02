using UnityEngine;

public class ItemPickUpData : MonoBehaviour
{
    [SerializeField] private string itemId;

    public string ItemId => itemId;

    public ItemData GetItemData()
    {
        if (string.IsNullOrEmpty(itemId)) return null;

        if (Core.Item.ItemManager.Instance != null && Core.Item.ItemManager.Instance.LibraryItem != null)
        {
            var item = Core.Item.ItemManager.Instance.LibraryItem.GetItem(itemId);
            if (item != null) return item;
        }

        return global::ItemManager.GetItemData(itemId);
    }
}
