using UnityEngine;

public class ItemPickUpData : MonoBehaviour
{
    [SerializeField]string itemId;

    public ItemData GetItemData()
    {
        if (itemId == null) return null;
        return Core.Item.ItemManager.Instance.LibraryItem.GetItem(itemId);
    }
}
