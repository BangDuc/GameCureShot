using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopView : MonoBehaviour
{
    [SerializeField] private Image icon;
    private UIButtonView buttonView;
    public event Action<string> Selected;
    private string itemId;
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

    public void Render(string itemId, Sprite img)
    {
        if (itemId != null)
        {
            this.itemId = itemId;
            icon.sprite = img;
        }
    }

    private void NotifySelected()
    {
        Selected?.Invoke(itemId);
    }
}
