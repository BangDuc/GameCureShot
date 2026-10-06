using UnityEngine;

public class GalleryPresenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private UIButtonView galleryButton;
    [SerializeField] private UIButtonView closeButton;
    [SerializeField] private PanelView galleryPanel;

    //[Header("System")]
    //[SerializeField] private RunController controller;


    private void OnEnable()
    {
        galleryButton.Clicked += HandleGalleryClicked;
        closeButton.Clicked += HandleCloseClicked;
    }

    private void OnDisable()
    {
        galleryButton.Clicked -= HandleGalleryClicked;
        closeButton.Clicked -= HandleCloseClicked;
    }

    private void HandleGalleryClicked()
    {
        galleryPanel.Show();
    }

    private void HandleCloseClicked()
    {
        galleryPanel.Hide();
    }
}
