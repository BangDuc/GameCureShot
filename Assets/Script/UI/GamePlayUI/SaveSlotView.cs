using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(UIButtonView))]
public class SaveSlotView : MonoBehaviour
{
    [Header("CONTENT")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    private UIButtonView buttonView;
    private string saveId;

    public event Action<string> Selected;

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

    public void Render(
        string id,
        string title,
        string description,
        bool canLoad)
    {
        saveId = canLoad ? id : null;

        titleText.text = title;
        descriptionText.text = description;

        buttonView.SetInteractable(canLoad);
    }

    private void NotifySelected()
    {
        if (string.IsNullOrEmpty(saveId))
            return;

        Selected?.Invoke(saveId);
    }
}