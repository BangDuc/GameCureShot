using UnityEngine;
using UnityEngine.EventSystems;

public class ShopTabButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    private GameObject tabBrush;
    private GameObject selectedLine;
    private ShopTabController controller;

    private bool isHovered;
    private bool isSelected;

    private void Awake()
    {
        Transform slot = transform.parent;

        tabBrush = slot.Find("TabBrush")?.gameObject;
        selectedLine = slot.Find("SelectedLine")?.gameObject;
        controller = GetComponentInParent<ShopTabController>();

        if (tabBrush == null)
            Debug.LogError($"{name}: Không tìm thấy TabBrush.", this);

        if (selectedLine == null)
            Debug.LogError($"{name}: Không tìm thấy SelectedLine.", this);

        if (controller == null)
            Debug.LogError($"{name}: Không tìm thấy ShopTabController.", this);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        RefreshVisual();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        RefreshVisual();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        RefreshVisual();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller?.SelectTab(this);
    }

    private void RefreshVisual()
    {
        bool showBrush = isHovered || isSelected;

        if (tabBrush != null)
            tabBrush.SetActive(showBrush);

        if (selectedLine != null)
            selectedLine.SetActive(isSelected);
    }
}