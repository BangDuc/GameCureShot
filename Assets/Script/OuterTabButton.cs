using UnityEngine;
using UnityEngine.EventSystems;

public class OuterTabButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    private GameObject tabBrush;
    private GameObject selectedLine;
    private OuterPageController controller;

    private bool isHovered;
    private bool isSelected;

    private void Awake()
    {
        Transform slot = transform.parent;

        tabBrush = slot.Find("TabBrush")?.gameObject;
        selectedLine = slot.Find("SelectedLine")?.gameObject;
        controller = GetComponentInParent<OuterPageController>();
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
        if (tabBrush != null)
            tabBrush.SetActive(isHovered || isSelected);

        if (selectedLine != null)
            selectedLine.SetActive(isSelected);
    }
}