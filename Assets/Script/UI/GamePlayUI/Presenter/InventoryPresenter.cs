using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private InventorySlotView slotPrefab;

    private readonly List<InventorySlotView> views = new();
    private Inventory inventory;

    public InventoryItemData SelectedItem { get; private set; }
    public event Action<InventoryItemData> Selected;

    [SerializeField] private UIButtonView useButton;
    public event Action<InventoryItemData> UseRequested;

    private void OnEnable()
    {
        useButton.Clicked += HandleUseClicked;
    }

    private void OnDisable()
    {
        useButton.Clicked -= HandleUseClicked;
    }

    private void HandleUseClicked()
    {
        if (SelectedItem == null)
            return;

        UseRequested?.Invoke(SelectedItem);
    }
    public void Initialize(Inventory inventory)
    {
        if (this.inventory != null)
            this.inventory.Changed -= BuildProfile;

        this.inventory = inventory
            ?? throw new ArgumentNullException(nameof(inventory));

        SelectedItem = null;
        this.inventory.Changed += BuildProfile;
        BuildProfile();
    }

    public void BuildProfile()
    {
        if (inventory == null)
            return;

        ClearSlots();

        bool selectionExists = false;

        foreach (InventoryItemData ownedItem in inventory.Items)
        {
            ItemData item = ItemManager.GetItemData(ownedItem.itemId);

            if (item == null)
            {
                Debug.LogError(
                    $"Không tìm thấy ItemData: {ownedItem.itemId}", this);
                continue;
            }

            InventorySlotView view =
                Instantiate(slotPrefab, content, false);

            view.Render(ownedItem, item.icon);

            bool selected = ReferenceEquals(ownedItem, SelectedItem);

            if (selected)
                selectionExists = true;

            view.SetSelected(selected);
            view.Selected += HandleSelected;
            view.gameObject.SetActive(true);
            views.Add(view);
        }

        if (!selectionExists && SelectedItem != null)
        {
            SelectedItem = null;
            Selected?.Invoke(null);
        }
    }

    private void HandleSelected(InventoryItemData item)
    {
        SelectedItem = ReferenceEquals(SelectedItem, item)
            ? null
            : item;

        foreach (InventorySlotView view in views)
        {
            view.SetSelected(
                ReferenceEquals(view.Item, SelectedItem));
        }

        Selected?.Invoke(SelectedItem);
    }

    private void ClearSlots()
    {
        foreach (InventorySlotView view in views)
        {
            if (view == null)
                continue;

            view.Selected -= HandleSelected;
            view.gameObject.SetActive(false);
            Destroy(view.gameObject);
        }

        views.Clear();
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.Changed -= BuildProfile;

        ClearSlots();
    }
}