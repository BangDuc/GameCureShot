using System;
using System.Collections;
using Bang.Lib.Singleton;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : Singleton<RoomManager>
{
    [Header("Cấu Hình Các Phòng (Room Setup)")]
    [SerializeField] private GameObject phongKhachRoot;
    [SerializeField] private GameObject kitchenRoot;
    [SerializeField] private BackgroundBounds phongKhachBounds;
    [SerializeField] private BackgroundBounds kitchenBounds;

    [Header("Vật Thể Chuyển Cửa (Door Triggers)")]
    [SerializeField] private GameObject doorToKitchenTrigger;
    [SerializeField] private GameObject doorToPhongKhachTrigger;

    [Header("Trạng Thái Hiện Tại (State)")]
    [SerializeField] private RoomId currentRoom = RoomId.PhongKhach;
    [SerializeField] private bool toggleActiveState = true;

    [Header("Hiệu Ứng Chuyển Cảnh (Transition FX)")]
    [SerializeField] private bool useFadeTransition = true;
    [SerializeField] private float fadeDuration = 0.35f;

    [Header("Phím Tắt Debug (Testing)")]
    [SerializeField] private bool enableDebugHotkey = true;

    private bool isTransitioning = false;
    private CanvasGroup fadeCanvasGroup;

    public RoomId CurrentRoom => currentRoom;
    public bool IsTransitioning => isTransitioning;
    public GameObject PhongKhachRoot => phongKhachRoot;
    public GameObject KitchenRoot => kitchenRoot;
    public BackgroundBounds PhongKhachBounds => phongKhachBounds;
    public BackgroundBounds KitchenBounds => kitchenBounds;

    public BackgroundBounds GetBoundsForRoom(RoomId room)
    {
        if (room == RoomId.Kitchen)
        {
            if (kitchenBounds != null)
            {
                kitchenBounds.CalculateBounds();
                return kitchenBounds;
            }
            if (kitchenRoot != null)
            {
                var b = kitchenRoot.GetComponentInChildren<BackgroundBounds>(true);
                if (b != null) { b.CalculateBounds(); return b; }
            }
        }
        else
        {
            if (phongKhachBounds != null)
            {
                phongKhachBounds.CalculateBounds();
                return phongKhachBounds;
            }
            if (phongKhachRoot != null)
            {
                var b = phongKhachRoot.GetComponentInChildren<BackgroundBounds>(true);
                if (b != null) { b.CalculateBounds(); return b; }
            }
            if (CameraMoving.Instance != null && CameraMoving.Instance.BackgroundBounds != null)
            {
                CameraMoving.Instance.BackgroundBounds.CalculateBounds();
                return CameraMoving.Instance.BackgroundBounds;
            }
        }
        return null;
    }

    public event Action<RoomId, RoomId> OnRoomTransitionStart;
    public event Action<RoomId> OnRoomChanged;

    private void Start()
    {
        AutoFindRooms();
        EnsureFadeOverlay();
        InitializeRoomState();
    }

    private void Update()
    {
        if (enableDebugHotkey && Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleRoom();
        }
    }

    /// <summary>
    /// Tự động tìm kiếm các GameObject phòng con nếu chưa được kéo thả trong Inspector.
    /// </summary>
    public void AutoFindRooms()
    {
        var allChildren = GetComponentsInChildren<Transform>(true);

        if (phongKhachRoot == null)
        {
            foreach (var t in allChildren)
            {
                if (t.name == "PhongKhach" || t.name.Contains("PhongKhach"))
                {
                    phongKhachRoot = t.gameObject;
                    break;
                }
            }
            if (phongKhachRoot == null)
            {
                var found = GameObject.Find("PhongKhach");
                if (found != null) phongKhachRoot = found;
            }
        }

        if (kitchenRoot == null)
        {
            foreach (var t in allChildren)
            {
                if (t.name == "Kitchen" || t.name.Contains("Kitchen"))
                {
                    kitchenRoot = t.gameObject;
                    break;
                }
            }
            if (kitchenRoot == null)
            {
                var found = GameObject.Find("Kitchen");
                if (found != null) kitchenRoot = found;
            }
        }

        // Tự động tìm Door triggers
        if (doorToKitchenTrigger == null)
        {
            var t = transform.Find("Door_To_Kitchen");
            if (t != null) doorToKitchenTrigger = t.gameObject;
        }

        if (doorToPhongKhachTrigger == null)
        {
            var t = transform.Find("Door_To_PhongKhach");
            if (t != null) doorToPhongKhachTrigger = t.gameObject;
        }

        // Tự động tìm hoặc gắn BackgroundBounds cho PhongKhach
        if (phongKhachBounds == null && phongKhachRoot != null)
        {
            phongKhachBounds = phongKhachRoot.GetComponentInChildren<BackgroundBounds>(true);
            if (phongKhachBounds == null && CameraMoving.Instance != null && CameraMoving.Instance.BackgroundBounds != null)
            {
                phongKhachBounds = CameraMoving.Instance.BackgroundBounds;
            }
        }

        // Tự động tìm hoặc tạo BackgroundBounds cho Kitchen
        if (kitchenBounds == null && kitchenRoot != null)
        {
            kitchenBounds = kitchenRoot.GetComponentInChildren<BackgroundBounds>(true);
            if (kitchenBounds == null)
            {
                var renderers = kitchenRoot.GetComponentsInChildren<SpriteRenderer>(true);
                SpriteRenderer bgRenderer = null;
                foreach (var r in renderers)
                {
                    if (r.name == "BackGround" || r.name == "Layer 0" || r.name == "Background")
                    {
                        bgRenderer = r;
                        break;
                    }
                }
                if (bgRenderer == null && renderers.Length > 0)
                {
                    float maxArea = 0f;
                    foreach (var r in renderers)
                    {
                        if (r != null && r.sprite != null)
                        {
                            float area = r.bounds.size.x * r.bounds.size.y;
                            if (area > maxArea)
                            {
                                maxArea = area;
                                bgRenderer = r;
                            }
                        }
                    }
                    if (bgRenderer == null) bgRenderer = renderers[0];
                }

                if (bgRenderer != null)
                {
                    kitchenBounds = kitchenRoot.GetComponent<BackgroundBounds>();
                    if (kitchenBounds == null)
                    {
                        kitchenBounds = kitchenRoot.AddComponent<BackgroundBounds>();
                    }
                    kitchenBounds.backgroundRenderer = bgRenderer;
                    kitchenBounds.CalculateBounds();
                }
            }
        }

        // Tự động đồng bộ các tương tác đóng mở tủ & giải đố cho Kitchen
        if (kitchenRoot != null)
        {
            KitchenInteractionSync.EnsureOn(kitchenRoot);
        }

        // Tự động gắn CabinetItemDrop cho các tủ trong PhongKhach
        if (phongKhachRoot != null)
        {
            SetupLivingRoomCabinetDrops();
        }
    }

    private void SetupLivingRoomCabinetDrops()
    {
        if (phongKhachRoot == null) return;

        var transitions = phongKhachRoot.GetComponentsInChildren<CabinetTransition>(true);
        foreach (var cab in transitions)
        {
            if (cab == null || cab.gameObject == null) continue;
            if (cab.gameObject.name == "DongHo") continue;

            var drop = cab.GetComponent<CabinetItemDrop>();
            if (drop == null)
            {
                drop = cab.gameObject.AddComponent<CabinetItemDrop>();
                drop.Setup("Health", new Vector3(0f, -0.5f, 0f));
            }
        }
    }

    /// <summary>
    /// Đảm bảo Canvas làm mờ màn hình (Screen Fade) luôn tồn tại để chuyển phòng mượt mà.
    /// </summary>
    private void EnsureFadeOverlay()
    {
        if (fadeCanvasGroup != null) return;

        var existing = GameObject.Find("[RoomTransition_Fade]");
        if (existing != null)
        {
            fadeCanvasGroup = existing.GetComponent<CanvasGroup>();
            return;
        }

        GameObject fadeGo = new GameObject("[RoomTransition_Fade]");
        DontDestroyOnLoad(fadeGo);
        var canvas = fadeGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;
        fadeGo.AddComponent<CanvasScaler>();

        fadeCanvasGroup = fadeGo.AddComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
        fadeCanvasGroup.interactable = false;

        GameObject imgGo = new GameObject("BlackOverlay");
        imgGo.transform.SetParent(fadeGo.transform, false);
        var img = imgGo.AddComponent<Image>();
        img.color = Color.black;
        var rect = imgGo.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    /// <summary>
    /// Khởi tạo trạng thái ban đầu: Kích hoạt phòng hiện tại và tắt phòng còn lại.
    /// </summary>
    public void InitializeRoomState()
    {
        if (toggleActiveState)
        {
            if (phongKhachRoot != null)
                phongKhachRoot.SetActive(currentRoom == RoomId.PhongKhach);

            if (kitchenRoot != null)
                kitchenRoot.SetActive(currentRoom == RoomId.Kitchen);

            if (doorToKitchenTrigger != null)
                doorToKitchenTrigger.SetActive(currentRoom == RoomId.PhongKhach);

            if (doorToPhongKhachTrigger != null)
                doorToPhongKhachTrigger.SetActive(currentRoom == RoomId.Kitchen);
        }

        UpdateCameraForCurrentRoom();
    }

    /// <summary>
    /// Chuyển đổi giữa 2 phòng: PhongKhach và Kitchen.
    /// </summary>
    public void SwitchRoom(RoomId targetRoom, bool immediate = false)
    {
        if (targetRoom == currentRoom)
        {
            Debug.Log($"[RoomManager] Đang ở phòng {targetRoom} rồi.");
            return;
        }

        if (isTransitioning)
        {
            Debug.LogWarning("[RoomManager] Đang trong quá trình chuyển phòng, xin chờ.");
            return;
        }

        StartCoroutine(TransitionRoutine(targetRoom, immediate));
    }

    private IEnumerator TransitionRoutine(RoomId targetRoom, bool immediate)
    {
        isTransitioning = true;
        RoomId fromRoom = currentRoom;
        OnRoomTransitionStart?.Invoke(fromRoom, targetRoom);

        if (!immediate && useFadeTransition && fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = true;
            float halfDuration = fadeDuration * 0.5f;

            // 1. Làm mờ sang đen (Fade Out)
            float elapsed = 0f;
            while (elapsed < halfDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / halfDuration);
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;

            // 2. Chuyển đổi phòng
            ApplyRoomSwitch(targetRoom);
            yield return new WaitForSecondsRealtime(0.05f);

            // 3. Sáng trở lại (Fade In)
            elapsed = 0f;
            while (elapsed < halfDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                fadeCanvasGroup.alpha = Mathf.Clamp01(1f - (elapsed / halfDuration));
                yield return null;
            }
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            ApplyRoomSwitch(targetRoom);
        }

        currentRoom = targetRoom;
        isTransitioning = false;

        OnRoomChanged?.Invoke(targetRoom);
        EventBus.Publish("OnRoomChanged");
        EventBus.Publish($"RoomChanged_{targetRoom}");
        Debug.Log($"<color=green>[RoomManager] Chuyển phòng thành công từ {fromRoom} sang {targetRoom}.</color>");
    }

    private void ApplyRoomSwitch(RoomId targetRoom)
    {
        if (toggleActiveState)
        {
            if (phongKhachRoot != null)
                phongKhachRoot.SetActive(targetRoom == RoomId.PhongKhach);

            if (kitchenRoot != null)
                kitchenRoot.SetActive(targetRoom == RoomId.Kitchen);

            if (doorToKitchenTrigger != null)
                doorToKitchenTrigger.SetActive(targetRoom == RoomId.PhongKhach);

            if (doorToPhongKhachTrigger != null)
                doorToPhongKhachTrigger.SetActive(targetRoom == RoomId.Kitchen);
        }

        currentRoom = targetRoom;
        UpdateCameraForCurrentRoom();
    }

    private void UpdateCameraForCurrentRoom()
    {
        if (CameraMoving.Instance == null) return;

        BackgroundBounds targetBounds = (currentRoom == RoomId.PhongKhach) ? phongKhachBounds : kitchenBounds;
        if (targetBounds != null)
        {
            CameraMoving.Instance.SetBackgroundBounds(targetBounds);
        }
    }

    [ContextMenu("Chuyển Sang Kitchen")]
    public void SwitchToKitchen() => SwitchRoom(RoomId.Kitchen);

    [ContextMenu("Chuyển Sang PhongKhach")]
    public void SwitchToPhongKhach() => SwitchRoom(RoomId.PhongKhach);

    [ContextMenu("Đổi Phòng (Toggle)")]
    public void ToggleRoom()
    {
        SwitchRoom(currentRoom == RoomId.PhongKhach ? RoomId.Kitchen : RoomId.PhongKhach);
    }
}
