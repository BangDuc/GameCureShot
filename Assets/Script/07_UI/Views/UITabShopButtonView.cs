using System;
using UnityEngine;
using UnityEngine.UI;

public class UITabShopButtonView : MonoBehaviour
{
    public event Action Clicked;
    private Button button;
    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }
    private void OnEnable()
    {
        button.onClick.AddListener(NotifyClicked);
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(NotifyClicked);
    }
    private void NotifyClicked()
    {
        Clicked?.Invoke();
    }
    public void SetInteractable(bool value)
    {
        button.interactable = value;
    }
}
