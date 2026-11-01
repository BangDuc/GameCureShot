using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonView : MonoBehaviour
{
    public event Action Clicked;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
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
