using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private InventorySlotView slotPrefab;

    [SerializeField] private  List<InventorySlotView> views = new();

    [SerializeField] private ItemInventory inventory;

    [SerializeField] private InventorySlotView currentSelectedSlot;

    [SerializeField] private UIButtonView useButton;
    

    private void Start()
    {
        inventory = Core.Item.ItemManager.Instance.InventoryItem;
        inventory.OnPickUpItem += InitUI;
        inventory.OnUsingItem += InitUI;
        InitUI();

    }
    private void OnEnable()
    {
        if (useButton != null)
            useButton.Clicked += HandleUseClicked;
    }

    private void OnDisable()
    {
        if (useButton != null)
            useButton.Clicked -= HandleUseClicked;
    }

    private void HandleUseClicked()
    {
        if (inventory.CurrentSelect == null)
            return;

        inventory.Using();
    }

    
    public void Initialize(ItemInventory inventory)
    {
        ClearViews();
        foreach (var item in inventory.Inventory)
        {
            InventorySlotView ItemUI = Instantiate(slotPrefab, content);
            views.Add(ItemUI);
            ItemUI.Selected += HandleSlotSelected;
            ItemUI.Init(item);
        }
    }
    private void HandleSlotSelected(InventorySlotView clickedSlot)
    {
        
        if (currentSelectedSlot == clickedSlot)
        {
            currentSelectedSlot.SetSelected(false);
            currentSelectedSlot = null;

            inventory.SetSelected(null);
            return;
        }

        
        if (currentSelectedSlot != null)
        {
            currentSelectedSlot.SetSelected(false);
        }

       
        currentSelectedSlot = clickedSlot;
        currentSelectedSlot.SetSelected(true);
        inventory.SetSelected(clickedSlot.Item);
    }
    private void ClearViews()
    {
        foreach (var view in views)
        {
            if (view != null)
            {
                view.Selected -= HandleSlotSelected;
                Destroy(view.gameObject);
            }
        }
        views.Clear();
        currentSelectedSlot = null;
        inventory.SetSelected(null);
    }

    public void InitUI()
    {
        Initialize(Core.Item.ItemManager.Instance.InventoryItem);
    }
    private void OnDestroy()
    {
        ClearViews();
    }
}