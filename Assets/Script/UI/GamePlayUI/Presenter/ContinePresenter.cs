using System;
using UnityEngine;

public class ContinePresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView ContinueButton;
    [SerializeField] private UIButtonView closeButton;
    [SerializeField] private PanelView continuePanel;

    public event Action<int> ClickedSave;

    [Header("Save List")]
    [SerializeField] private SavePresenter savePresenter;

    private void OnEnable()
    {
        ContinueButton.Clicked += HandleContinueClicked;
        closeButton.Clicked += HandleCloseClicked;
        savePresenter.SaveSlotClicked += HandleSaveClicked;
    }

    private void OnDisable()
    {
        ContinueButton.Clicked -= HandleContinueClicked;
        closeButton.Clicked -= HandleCloseClicked;
        savePresenter.SaveSlotClicked -= HandleSaveClicked;
    }

    private void HandleContinueClicked()
    {
        continuePanel.Show();
        savePresenter.BuildProfile();
    }

    private void HandleCloseClicked()
    {
        continuePanel.Hide();
    }
    private void HandleSaveClicked(int slotId)
    {
        ClickedSave?.Invoke(slotId);
    }    
}