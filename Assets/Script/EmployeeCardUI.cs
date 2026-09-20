using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class EmployeeCardUI : MonoBehaviour
{
    [SerializeField] private float slideDuration = 0.25f;
    [SerializeField] private float extraHiddenDistance = 40f;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Coroutine slideCoroutine;

    private Vector2 openPosition;
    private Vector2 closedPosition;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        // Vị trí đang đặt trong Scene chính là vị trí khi mở.
        openPosition = rectTransform.anchoredPosition;

        // Đẩy toàn bộ thẻ ra ngoài mép trái.
        closedPosition = openPosition
            + Vector2.left * (rectTransform.rect.width + extraHiddenDistance);

        SetImmediate(false);
    }

    public void Toggle()
    {
        SetOpen(!IsOpen);
    }

    public void Open()
    {
        SetOpen(true);
    }

    public void Close()
    {
        SetOpen(false);
    }

    private void SetOpen(bool open)
    {
        IsOpen = open;

        if (slideCoroutine != null)
            StopCoroutine(slideCoroutine);

        canvasGroup.interactable = open;
        canvasGroup.blocksRaycasts = open;

        slideCoroutine = StartCoroutine(Slide(open));
    }

    private IEnumerator Slide(bool opening)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 targetPosition = opening ? openPosition : closedPosition;

        float startAlpha = canvasGroup.alpha;
        float targetAlpha = opening ? 1f : 0f;

        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(elapsed / slideDuration);
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            rectTransform.anchoredPosition =
                Vector2.Lerp(startPosition, targetPosition, smoothProgress);

            canvasGroup.alpha =
                Mathf.Lerp(startAlpha, targetAlpha, smoothProgress);

            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        canvasGroup.alpha = targetAlpha;
        slideCoroutine = null;
    }

    private void SetImmediate(bool open)
    {
        IsOpen = open;

        rectTransform.anchoredPosition =
            open ? openPosition : closedPosition;

        canvasGroup.alpha = open ? 1f : 0f;
        canvasGroup.interactable = open;
        canvasGroup.blocksRaycasts = open;
    }
}