using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIButtonView))]
public class InventorySlotView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject selectionMark;

    private UIButtonView buttonView;

    public InventoryItemData Item { get; private set; }
    public event Action<InventoryItemData> Selected;

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

    public void Render(InventoryItemData item, Sprite sprite)
    {
        Item = item;

        if (sprite != null)
            icon.sprite = sprite;

    }

    public void SetSelected(bool selected)
    {
        if (selectionMark != null)
            selectionMark.SetActive(selected);
    }

    private void NotifySelected()
    {
        Selected?.Invoke(Item);
    }
}