using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PanelView : MonoBehaviour
{
    private CanvasGroup canvas;
    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        canvas.alpha = 1f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }
    public void Hide()
    {
        if (gameObject.activeInHierarchy)
        {
            canvas.alpha = 0f;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }

        gameObject.SetActive(false);
    }
}
