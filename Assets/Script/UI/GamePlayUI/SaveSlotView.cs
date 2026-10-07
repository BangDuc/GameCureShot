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
    private int saveId;

    public event Action<int> Selected;

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

    public void Render(int slotId, string slotName, SaveInfo info)
    {
        if (info != null)
        {
            saveId = slotId;
            titleText.text = slotName;
            descriptionText.text = "Time Stamp: " + info.timestamp + " playtime: " + info.playTime + " permanentHealth: " + info.permanentHealth + " money : " + info.money;
        }
    }

    private void NotifySelected()
    {
        Selected?.Invoke(saveId);
    }
}