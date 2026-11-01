using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIButtonView))]
public class InventorySlotView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject selectionMark;
    [SerializeField] private InventoryItemData item;
    private UIButtonView buttonView;

    public InventoryItemData Item { get => item; set => item = value; }

    public event Action<InventorySlotView> Selected;
    private void Awake()
    {
        buttonView = GetComponent<UIButtonView>();
    }

    private void OnEnable()
    {
        buttonView.Clicked += NotifySelected;
    }

    private void OnDisable()
    {
        buttonView.Clicked -= NotifySelected;
    }

    public void Init(InventoryItemData item)
    {
        this.Item = item;
        if (icon != null && item != null)
            icon.sprite = item._icon;

        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        if (selectionMark != null)
            selectionMark.SetActive(selected);
    }

    private void NotifySelected()
    {
        Selected?.Invoke(this);

    }
}