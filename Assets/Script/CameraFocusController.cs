using System.Collections;
using System.Linq;
using UnityEngine;

public class CameraFocusController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera roomCamera;
    [SerializeField] private CanvasGroup cameraModePanel;

    [Header("Animation")]
    [SerializeField, Min(0.01f)]
    private float cameraTransitionDuration = 0.35f;

    [SerializeField, Min(0.01f)]
    private float panelFadeDuration = 0.15f;

    [SerializeField, Range(0.8f, 1f)]
    private float panelStartScale = 0.96f;

    private CameraFocusTarget[] targets;
    private RectTransform panelRect;

    private int currentIndex;
    private bool cameraModeActive;
    private bool isTransitioning;

    private Vector3 originalCameraPosition;
    private float originalOrthographicSize;

    private Vector3 panelVisibleScale;
    private Vector3 panelHiddenScale;

    private Coroutine transitionCoroutine;

    public bool IsCameraModeActive => cameraModeActive;

    private void Awake()
    {
        if (cameraModePanel == null)
        {
            Debug.LogError(
                "CameraFocusController: Chưa gán Camera Mode Panel."
            );

            enabled = false;
            return;
        }

        panelRect = cameraModePanel.GetComponent<RectTransform>();

        panelVisibleScale = panelRect.localScale;
        panelHiddenScale = panelVisibleScale * panelStartScale;

        HidePanelImmediately();
    }

    // Camera Button bên BottomHUD gọi hàm này.
    public void OpenCameraMode()
    {
        if (cameraModeActive || isTransitioning)
            return;

        if (roomCamera == null)
            roomCamera = Camera.main;

        if (roomCamera == null)
        {
            Debug.LogError(
                "CameraFocusController: Không tìm thấy Main Camera."
            );
            return;
        }

        targets = FindObjectsByType<CameraFocusTarget>(
                FindObjectsSortMode.None)
            .OrderBy(target => target.FocusOrder)
            .ToArray();

        if (targets.Length == 0)
        {
            Debug.LogWarning(
                "CameraFocusController: Room không có CameraFocusTarget."
            );
            return;
        }

        originalCameraPosition = roomCamera.transform.position;
        originalOrthographicSize = roomCamera.orthographicSize;

        currentIndex = 0;
        cameraModeActive = true;
        isTransitioning = true;

        PreparePanelForOpening();

        transitionCoroutine = StartCoroutine(
            OpenCameraRoutine()
        );
    }

    // TurnOffCameraButton gọi hàm này.
    public void CloseCameraMode()
    {
        if (!cameraModeActive || isTransitioning)
            return;

        isTransitioning = true;

        cameraModePanel.interactable = false;
        cameraModePanel.blocksRaycasts = true;

        transitionCoroutine = StartCoroutine(
            CloseCameraRoutine()
        );
    }

    // NextFocusButton gọi hàm này.
    public void NextTarget()
    {
        if (!CanChangeTarget())
            return;

        currentIndex = (currentIndex + 1) % targets.Length;
        StartTargetTransition();
    }

    // PreviousFocusButton gọi hàm này.
    public void PreviousTarget()
    {
        if (!CanChangeTarget())
            return;

        currentIndex--;

        if (currentIndex < 0)
            currentIndex = targets.Length - 1;

        StartTargetTransition();
    }

    private bool CanChangeTarget()
    {
        return cameraModeActive
            && !isTransitioning
            && targets != null
            && targets.Length > 0;
    }

    private void StartTargetTransition()
    {
        isTransitioning = true;

        cameraModePanel.interactable = false;
        cameraModePanel.blocksRaycasts = true;

        transitionCoroutine = StartCoroutine(
            ChangeTargetRoutine()
        );
    }

    private IEnumerator OpenCameraRoutine()
    {
        CameraFocusTarget target = targets[currentIndex];

        Vector3 startPosition = roomCamera.transform.position;
        float startSize = roomCamera.orthographicSize;

        Vector3 destination = GetTargetPosition(target);
        float destinationSize = target.OrthographicSize;

        float elapsed = 0f;

        while (elapsed < cameraTransitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float cameraProgress = Mathf.Clamp01(
                elapsed / cameraTransitionDuration
            );

            cameraProgress = Mathf.SmoothStep(
                0f,
                1f,
                cameraProgress
            );

            roomCamera.transform.position = Vector3.Lerp(
                startPosition,
                destination,
                cameraProgress
            );

            roomCamera.orthographicSize = Mathf.Lerp(
                startSize,
                destinationSize,
                cameraProgress
            );

            float panelProgress = Mathf.Clamp01(
                elapsed / panelFadeDuration
            );

            panelProgress = Mathf.SmoothStep(
                0f,
                1f,
                panelProgress
            );

            cameraModePanel.alpha = panelProgress;

            panelRect.localScale = Vector3.Lerp(
                panelHiddenScale,
                panelVisibleScale,
                panelProgress
            );

            yield return null;
        }

        roomCamera.transform.position = destination;
        roomCamera.orthographicSize = destinationSize;

        cameraModePanel.alpha = 1f;
        panelRect.localScale = panelVisibleScale;

        cameraModePanel.interactable = true;
        cameraModePanel.blocksRaycasts = true;

        isTransitioning = false;
        transitionCoroutine = null;
    }

    private IEnumerator ChangeTargetRoutine()
    {
        CameraFocusTarget target = targets[currentIndex];

        yield return MoveCameraRoutine(
            GetTargetPosition(target),
            target.OrthographicSize
        );

        cameraModePanel.interactable = true;
        cameraModePanel.blocksRaycasts = true;

        isTransitioning = false;
        transitionCoroutine = null;
    }

    private IEnumerator CloseCameraRoutine()
    {
        // Camera trở về trước, khung vẫn còn hiển thị.
        yield return MoveCameraRoutine(
            originalCameraPosition,
            originalOrthographicSize
        );

        // Sau đó khung camera mới fade-out.
        float startAlpha = cameraModePanel.alpha;
        Vector3 startScale = panelRect.localScale;
        float elapsed = 0f;

        while (elapsed < panelFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(
                elapsed / panelFadeDuration
            );

            progress = Mathf.SmoothStep(0f, 1f, progress);

            cameraModePanel.alpha = Mathf.Lerp(
                startAlpha,
                0f,
                progress
            );

            panelRect.localScale = Vector3.Lerp(
                startScale,
                panelHiddenScale,
                progress
            );

            yield return null;
        }

        cameraModePanel.alpha = 0f;
        panelRect.localScale = panelHiddenScale;
        cameraModePanel.interactable = false;
        cameraModePanel.blocksRaycasts = false;

        cameraModeActive = false;
        isTransitioning = false;
        transitionCoroutine = null;
    }

    private IEnumerator MoveCameraRoutine(
        Vector3 destination,
        float destinationSize)
    {
        Vector3 startPosition = roomCamera.transform.position;
        float startSize = roomCamera.orthographicSize;
        float elapsed = 0f;

        while (elapsed < cameraTransitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(
                elapsed / cameraTransitionDuration
            );

            progress = Mathf.SmoothStep(0f, 1f, progress);

            roomCamera.transform.position = Vector3.Lerp(
                startPosition,
                destination,
                progress
            );

            roomCamera.orthographicSize = Mathf.Lerp(
                startSize,
                destinationSize,
                progress
            );

            yield return null;
        }

        roomCamera.transform.position = destination;
        roomCamera.orthographicSize = destinationSize;
    }

    private Vector3 GetTargetPosition(CameraFocusTarget target)
    {
        Vector3 destination = target.FocusPosition;

        // Camera 2D giữ nguyên trục Z.
        destination.z = roomCamera.transform.position.z;

        return destination;
    }

    private void PreparePanelForOpening()
    {
        cameraModePanel.alpha = 0f;
        cameraModePanel.interactable = false;
        cameraModePanel.blocksRaycasts = true;

        panelRect.localScale = panelHiddenScale;
    }

    private void HidePanelImmediately()
    {
        cameraModePanel.alpha = 0f;
        cameraModePanel.interactable = false;
        cameraModePanel.blocksRaycasts = false;

        panelRect.localScale = panelHiddenScale;
    }
}