using UnityEngine;

public abstract class UnlockByItem_Base : MonoBehaviour, IUnlockByItem
{
    [SerializeField] string idItem;
    public ItemData GetItemNeeded()
    {
        return Core.Item.ItemManager.Instance.LibraryItem.GetItem(idItem);
    }


    public void Unlocked()
    {
        TuongTac();
        RemoveItemInventory();
    }

    public void RemoveItemInventory()
    {
        var CurrentItem = Core.Item.ItemManager.Instance.InventoryItem.CurrentSelect;
        Core.Item.ItemManager.Instance.InventoryItem.UsingByInteractInGame();
    }

    public virtual void TuongTac()
    {
        
    }
}
