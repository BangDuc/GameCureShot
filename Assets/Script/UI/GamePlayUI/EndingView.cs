using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingView : MonoBehaviour
{
    [SerializeField] private TMP_Text endingTypeText;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image endingImage;
    [SerializeField] private UIButtonView continueButton;
    public event Action ContinueClicked;

    private void OnEnable()
    {
        continueButton.Clicked += HandleButtonClicked;
    }

    private void OnDisable()
    {
        continueButton.Clicked -= HandleButtonClicked;
    }

    public void Render(EndingData ending)
    {
        if (ending == null)
            throw new ArgumentNullException(nameof(ending));
        endingTypeText.text = ending.endingType;
        titleText.text = ending.endingTitle;
        descriptionText.text = ending.description;
        endingImage.sprite = ending.endingImage;
    }    
    private void HandleButtonClicked()
    {
        ContinueClicked?.Invoke();
    }
}
