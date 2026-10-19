using Core.Interact;
using UnityEngine;

public class ItemInteractGiaiDo : MonoBehaviour, IInteractable
{
    IUnlockByItem UnlockbyItem;
    private void Start()
    {
        InitItem();
    }

    public void InitItem()
    {
        UnlockbyItem = GetComponent<IUnlockByItem>();
    }
    public bool CanInteract()
    {
        var inventory = Core.Item.ItemManager.Instance?.InventoryItem;
        var currentItem = inventory?.CurrentSelect;

        
        if (currentItem == null || string.IsNullOrEmpty(currentItem.itemId))
        {
            return false;
        }

        
        var itemNeeded = UnlockbyItem != null ? UnlockbyItem.GetItemNeeded() : null;

        if (itemNeeded == null || string.IsNullOrEmpty(itemNeeded.Id))
        {
            Debug.LogWarning("[ItemInteractGiaiDo] Chưa thiết lập UnlockbyItem hoặc ItemNeeded!");
            return false;
        }

        
        return string.Equals(currentItem.itemId, itemNeeded.Id, System.StringComparison.Ordinal);
    }

    public bool Interact()
    {
        if (!CanInteract()) return false;
        UnlockbyItem.Unlocked();
        return true;
    }

    public void OnHoverEnter()
    {
       
    }

    public void OnHoverExit()
    {

    }
    
}
