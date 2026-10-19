using Core.Item;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public List<InventoryItemData> Inventory = new List<InventoryItemData>();

    [SerializeField]private InventoryItemData currentSelect;

    public InventoryItemData CurrentSelect { get => currentSelect; set => currentSelect = value; }
    public event Action<InventoryItemData> Selected;

    public event Action<InventoryItemData> OnUsing;

    public event Action OnPickUpItem;
    public event Action OnUsingItem;

    private void Start()
    {

        //InitInventoryTest();

        InitInventory();
    }
    public void PickUpItem(ItemData item)
    {
        if (item == null) return;

        InventoryItemData Item_inventory = new InventoryItemData(item, Core.Item.ItemManager.Instance.ItemEffect.GetEffects(item.Id));
        Inventory.Add(Item_inventory);
        currentSelect = null;
        OnPickUpItem?.Invoke();
    }

    public void UsingByInteractInGame()
    {
        
        RemoveItemInventory(currentSelect);
        currentSelect = null;
        OnUsingItem?.Invoke();
    }

    public void Using()
    {
        if (currentSelect == null) return;

        //check null ItemEffect
        if (currentSelect.itemEffect == null)
        {
            Debug.Log("Chỉ sử dụng để tương tác hoặc giải đố");
            return;
        }

        //check Null hoac khong co effects 
        var listActiveEffects = currentSelect.itemEffect.Effects;
        if(listActiveEffects == null || listActiveEffects.Length == 0)
        {
            Debug.Log("Chỉ sử dụng để tương tác hoặc giải đố");
            return;
        }


        
        RemoveItemInventory(currentSelect);

        currentSelect = null;
        OnUsing?.Invoke(currentSelect);
        OnUsingItem?.Invoke();
    }

    

    public void SetSelected(InventoryItemData item)
    {
        currentSelect = item;
        if (currentSelect == null) 
        {
            Selected?.Invoke(null);
            return;
        } 
        Selected?.Invoke(currentSelect);
    }
    public void InitInventory()
    {
        Inventory = new List<InventoryItemData>();
    }

    

    public void AddItem(ItemData item)
    {
        if (item == null) return;

        InventoryItemData Item_inventory = new InventoryItemData(item, Core.Item.ItemManager.Instance.ItemEffect.GetEffects(item.Id));
        Inventory.Add(Item_inventory);
    }

    public void RemoveItemInventory(InventoryItemData item)
    {
        if (item == null) return;
        if (Inventory.Contains(item))
        {
            Inventory.Remove(item);
        }
    }


    public void InitInventoryTest()
    {
        Inventory = new List<InventoryItemData>();

        var ListLibrary = Core.Item.ItemManager.Instance.LibraryItem.GetAllItems();

        foreach( var item in ListLibrary)
        {

            InventoryItemData Item_inventory = new InventoryItemData(item, Core.Item.ItemManager.Instance.ItemEffect.GetEffects(item.Id));

            
            Inventory.Add(Item_inventory);
        }
    }



    
}
