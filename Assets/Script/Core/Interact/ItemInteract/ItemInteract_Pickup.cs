using Core.Interact;
using Unity.VisualScripting;
using UnityEngine;

public class ItemInteract_Pickup : MonoBehaviour, IInteractable
{
    Core.Item.ItemManager itemManager;

    ItemPickUpData data;
    private void Start()
    {
        InitItemPickUp();
    }
    public void InitItemPickUp()
    {
        itemManager = Core.Item.ItemManager.Instance;
        data= GetComponent<ItemPickUpData>();
    }
    public bool CanInteract()
    {

        return true;
    }

    public bool Interact()
    {
        itemManager.InventoryItem.PickUpItem(data.GetItemData());
        Destroy(this.gameObject);
        return true;
    }

    public void OnHoverEnter()
    {
        
    }

    public void OnHoverExit()
    {
        
    }
}
