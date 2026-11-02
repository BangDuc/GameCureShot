using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Core.GamePlay;
using Bang.Lib.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiRoomMonsterSpawner : Singleton<MultiRoomMonsterSpawner>
{
    [Header("Cấu Hình Spawn (Timing & Rules)")]
    [Tooltip("Thời gian tối thiểu giữa các đợt spawn (giây)")]
    [SerializeField] private float minSpawnInterval = 10f;

    [Tooltip("Thời gian tối đa giữa các đợt spawn (giây)")]
    [SerializeField] private float maxSpawnInterval = 20f;

    [Tooltip("Thời gian quái vật tồn tại trong phòng trước khi tự biến mất (giây)")]
    [SerializeField] private float monsterStayDuration = 22f;

    [Tooltip("Số lượng quái vật tối đa xuất hiện đồng thời")]
    [SerializeField] private int maxConcurrentMonsters = 1;

    [Header("Monster Template")]
    [SerializeField] private TargetController monsterTemplate;

    [Header("Điểm Xuất Hiện (Spawn Points)")]
    [SerializeField] private List<Transform> phongKhachSpawnPoints = new();
    [SerializeField] private List<Transform> kitchenSpawnPoints = new();

    [Header("Trạng Thái Runtime")]
    [SerializeField] private List<TargetController> activeMonsters = new();
    [SerializeField] private bool isSpawningActive = false;

    private Coroutine spawnLoopCoroutine;
    private readonly List<TargetController> monsterPool = new();

    public IReadOnlyList<TargetController> ActiveMonsters => activeMonsters;

    public event Action<TargetController, RoomId> OnMonsterSpawned;
    public event Action<TargetController> OnMonsterCaptured;
    public event Action<TargetController> OnMonsterDespawned;

    private void Start()
    {
        InitializeSpawner();
    }

    public void InitializeSpawner(Scene? scene = null)
    {
        FindOrSetupTemplate();
        FindOrSetupSpawnPoints();

        if (RunController.Instance != null)
        {
            RunController.Instance.OnRunFinished -= HandleRunFinished;
            RunController.Instance.OnRunFinished += HandleRunFinished;

            if (RunController.Instance.IsRunning)
            {
                StartSpawning();
            }
        }
    }

    private void OnDisable()
    {
        StopSpawning();
        if (RunController.Instance != null)
        {
            RunController.Instance.OnRunFinished -= HandleRunFinished;
        }
    }

    private void HandleRunFinished(RunResult result)
    {
        StopSpawning();
        DespawnAllMonsters();
    }

    /// <summary>
    /// Bắt đầu vòng lặp sinh quái ngẫu nhiên trong suốt Survival Run.
    /// </summary>
    public void StartSpawning()
    {
        if (isSpawningActive) return;
        isSpawningActive = true;

        if (spawnLoopCoroutine != null)
        {
            StopCoroutine(spawnLoopCoroutine);
        }
        spawnLoopCoroutine = StartCoroutine(SpawnLoopRoutine());
        Debug.Log("<color=magenta>[MultiRoomMonsterSpawner] Bắt đầu vòng lặp Spawn quái vật ngẫu nhiên.</color>");
    }

    /// <summary>
    /// Dừng vòng lặp sinh quái.
    /// </summary>
    public void StopSpawning()
    {
        isSpawningActive = false;
        if (spawnLoopCoroutine != null)
        {
            StopCoroutine(spawnLoopCoroutine);
            spawnLoopCoroutine = null;
        }
    }

    private IEnumerator SpawnLoopRoutine()
    {
        // Chờ 5 giây ban đầu sau khi vào Run để người chơi quan sát
        yield return new WaitForSeconds(5f);

        while (isSpawningActive)
        {
            if (RunController.Instance != null && RunController.Instance.IsRunFinished)
            {
                yield break;
            }

            // Nếu đang Pause hoặc Run chưa chạy -> Chờ cho đến khi tiếp tục
            while (Time.timeScale <= 0f || (RunController.Instance != null && !RunController.Instance.IsRunning))
            {
                if (RunController.Instance != null && RunController.Instance.IsRunFinished)
                {
                    yield break;
                }
                yield return null;
            }

            // Nếu số lượng quái hiện tại chưa đạt mức tối đa
            if (activeMonsters.Count < maxConcurrentMonsters)
            {
                SpawnRandomMonster();
            }

            // Hẹn giờ cho đợt spawn kế tiếp
            float delay = UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(delay);
        }
    }

    /// <summary>
    /// Sinh ngẫu nhiên 1 trong 2 loại quái đặc thù (Quỷ Máu hoặc The Shy) tại phòng ngẫu nhiên.
    /// </summary>
    public TargetController SpawnRandomMonster(RoomId? forceRoom = null)
    {
        // 50% Quỷ Máu (tấn công) - 50% The Shy (tắt đèn)
        string[] monsterPool = new string[] { "QuyMau", "TheShy" };
        string chosenId = monsterPool[UnityEngine.Random.Range(0, monsterPool.Length)];

        return SpawnSpecificMonster(chosenId, forceRoom);
    }

    /// <summary>
    /// Sinh cụ thể Quỷ Máu (Hành vi: Tấn công người chơi).
    /// </summary>
    public TargetController SpawnQuyMau(RoomId? room = null)
    {
        return SpawnSpecificMonster("QuyMau", room);
    }

    /// <summary>
    /// Sinh cụ thể The Shy (Hành vi: Làm tắt toàn bộ ánh sáng).
    /// </summary>
    public TargetController SpawnTheShy(RoomId? room = null)
    {
        return SpawnSpecificMonster("TheShy", room);
    }

    /// <summary>
    /// Sinh quái vật theo ID chỉ định tại phòng chỉ định.
    /// </summary>
    public TargetController SpawnSpecificMonster(string monsterId, RoomId? forceRoom = null)
    {
        FindOrSetupTemplate();
        if (monsterTemplate == null)
        {
            Debug.LogWarning("[MultiRoomMonsterSpawner] Không tìm thấy monsterTemplate để spawn quái vật!");
            return null;
        }

        // Chọn phòng: ưu tiên phòng người chơi đang đứng hoặc phòng bên cạnh
        RoomId chosenRoom;
        if (forceRoom.HasValue)
        {
            chosenRoom = forceRoom.Value;
        }
        else
        {
            RoomId currentRoom = (RoomManager.Instance != null) ? RoomManager.Instance.CurrentRoom : RoomId.PhongKhach;
            chosenRoom = (UnityEngine.Random.value < 0.6f)
                ? currentRoom
                : ((currentRoom == RoomId.PhongKhach) ? RoomId.Kitchen : RoomId.PhongKhach);
        }

        Transform spawnPoint = GetRandomSpawnPoint(chosenRoom);
        Vector3 rawPos = (spawnPoint != null) ? spawnPoint.position : GetFallbackPosition(chosenRoom);
        Vector3 spawnPos = ClampPositionToSafeBounds(rawPos, chosenRoom);

        // Lấy hoặc tạo quái vật từ Pool
        TargetController monster = GetMonsterFromPool();
        if (monster == null) return null;

        // Gắn quái vật làm con của GameObject phòng tương ứng
        GameObject roomRoot = GetRoomRoot(chosenRoom);
        if (roomRoot != null)
        {
            monster.transform.SetParent(roomRoot.transform, true);
        }

        monster.transform.position = spawnPos;

        // Lấy TargetData từ TargetLibrary
        TargetData targetData = null;
        if (TargetManager.Instance != null && TargetManager.Instance.library != null)
        {
            targetData = TargetManager.Instance.library.GetTarget(monsterId);
        }

        // Thiết lập ID, Sprite và Hành vi đặc thù
        monster.Setup(monsterId, targetData);

        // Đăng ký sự kiện
        monster.OnCapturedEvent -= HandleMonsterCaptured;
        monster.OnCapturedEvent += HandleMonsterCaptured;
        monster.OnDespawnedEvent -= HandleMonsterDespawned;
        monster.OnDespawnedEvent += HandleMonsterDespawned;

        // Kích hoạt GameObject quái vật
        monster.gameObject.SetActive(true);
        activeMonsters.Add(monster);

        // Bắt đầu đếm thời gian tồn tại (Stay Duration)
        StartCoroutine(MonsterStayTimerRoutine(monster, monsterStayDuration));

        OnMonsterSpawned?.Invoke(monster, chosenRoom);
        Debug.Log($"<color=magenta>[MultiRoomMonsterSpawner] Đã spawn quái '{monster.Data?.TargetName ?? monsterId}' tại phòng {chosenRoom} (Tọa độ: {spawnPos}). Thưởng: ${monster.Data?.MoneyReward ?? 0}</color>");

        return monster;
    }

    private IEnumerator MonsterStayTimerRoutine(TargetController monster, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (monster == null || monster.IsCaptured || !monster.gameObject.activeInHierarchy)
            {
                yield break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Hết thời gian mà người chơi chưa kịp chụp -> Quái tự biến mất
        if (monster != null && !monster.IsCaptured && monster.gameObject.activeInHierarchy)
        {
            Debug.Log($"<color=yellow>[MultiRoomMonsterSpawner] Quái '{monster.Data?.TargetName ?? monster.Id}' hết thời gian tồn tại, tự biến mất.</color>");
            monster.Despawn();
        }
    }

    private void HandleMonsterCaptured(TargetController monster)
    {
        if (monster == null) return;
        activeMonsters.Remove(monster);
        OnMonsterCaptured?.Invoke(monster);
        Debug.Log($"<color=green>[MultiRoomMonsterSpawner] Quái '{monster.Data?.TargetName ?? monster.Id}' đã bị tiêu diệt và biến mất.</color>");
    }

    private void HandleMonsterDespawned(TargetController monster)
    {
        if (monster == null) return;
        activeMonsters.Remove(monster);
        OnMonsterDespawned?.Invoke(monster);
    }

    /// <summary>
    /// Dọn dẹp tất cả quái vật đang xuất hiện khi kết thúc Run.
    /// </summary>
    public void DespawnAllMonsters()
    {
        var copy = new List<TargetController>(activeMonsters);
        foreach (var monster in copy)
        {
            if (monster != null)
            {
                monster.Despawn();
            }
        }
        activeMonsters.Clear();
    }

    private TargetController GetMonsterFromPool()
    {
        foreach (var m in monsterPool)
        {
            if (m != null && !m.gameObject.activeInHierarchy && !activeMonsters.Contains(m))
            {
                return m;
            }
        }

        // Tạo mới từ template
        if (monsterTemplate == null) return null;

        GameObject newGo = Instantiate(monsterTemplate.gameObject);
        newGo.name = $"Monster_Spawned_{monsterPool.Count + 1}";
        TargetController newMonster = newGo.GetComponent<TargetController>();

        monsterPool.Add(newMonster);
        return newMonster;
    }

    public BackgroundBounds GetRoomBounds(RoomId room)
    {
        if (RoomManager.Instance != null)
        {
            var b = RoomManager.Instance.GetBoundsForRoom(room);
            if (b != null)
            {
                b.CalculateBounds();
                return b;
            }
        }

        if (CameraMoving.Instance != null && CameraMoving.Instance.BackgroundBounds != null)
        {
            CameraMoving.Instance.BackgroundBounds.CalculateBounds();
            return CameraMoving.Instance.BackgroundBounds;
        }

        var allBounds = FindObjectsByType<BackgroundBounds>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var b in allBounds)
        {
            if (b == null) continue;
            string bName = b.gameObject.name.ToLower();
            string rName = room.ToString().ToLower();
            if (bName.Contains(rName) || (b.transform.parent != null && b.transform.parent.name.ToLower().Contains(rName)))
            {
                b.CalculateBounds();
                return b;
            }
        }

        if (allBounds.Length > 0 && allBounds[0] != null)
        {
            allBounds[0].CalculateBounds();
            return allBounds[0];
        }

        return null;
    }

    /// <summary>
    /// Tính toán phạm vi tầm nhìn mà camera có thể lia tới trong phòng được chỉ định.
    /// </summary>
    public bool GetCameraPanBounds(RoomId room, out float viewMinX, out float viewMaxX, out float viewMinY, out float viewMaxY)
    {
        BackgroundBounds bounds = GetRoomBounds(room);
        if (bounds == null)
        {
            viewMinX = -5f; viewMaxX = 5f; viewMinY = -2f; viewMaxY = 2f;
            return false;
        }

        bounds.CalculateBounds();

        float camHalfHeight = 4f;
        float aspect = 16f / 9f;
        Camera cam = Camera.main;
        if (cam != null)
        {
            if (cam.orthographic) camHalfHeight = cam.orthographicSize;
            if (cam.pixelHeight > 0) aspect = cam.aspect;
        }
        if (CameraMoving.Instance != null && CameraMoving.Instance.ActiveCam != null)
        {
            camHalfHeight = CameraMoving.Instance.ActiveCam.Lens.OrthographicSize;
        }
        float camHalfWidth = camHalfHeight * aspect;

        float bgWidth = bounds.maxX - bounds.minX;
        float bgHeight = bounds.maxY - bounds.minY;

        if (bgWidth >= camHalfWidth * 2f)
        {
            viewMinX = bounds.minX;
            viewMaxX = bounds.maxX;
        }
        else
        {
            float midX = (bounds.minX + bounds.maxX) * 0.5f;
            viewMinX = midX - camHalfWidth;
            viewMaxX = midX + camHalfWidth;
        }

        if (bgHeight >= camHalfHeight * 2f)
        {
            viewMinY = bounds.minY;
            viewMaxY = bounds.maxY;
        }
        else
        {
            float midY = (bounds.minY + bounds.maxY) * 0.5f;
            viewMinY = midY - camHalfHeight;
            viewMaxY = midY + camHalfHeight;
        }

        return true;
    }

    /// <summary>
    /// Vùng hình chữ nhật an toàn để quái vật xuất hiện, đảm bảo 100% nằm gọn trong Background Bounds và Camera view.
    /// </summary>
    public Rect GetSafeSpawnRect(RoomId room, float paddingX = 1.8f, float paddingBottom = 1.2f, float paddingTop = 1.8f)
    {
        GetCameraPanBounds(room, out float minX, out float maxX, out float minY, out float maxY);

        float safeMinX = minX + paddingX;
        float safeMaxX = maxX - paddingX;
        if (safeMinX > safeMaxX)
        {
            float midX = (minX + maxX) * 0.5f;
            safeMinX = midX;
            safeMaxX = midX;
        }

        float totalH = maxY - minY;
        // Cho chiều cao: Quái vật nên xuất hiện từ sàn nhà (minY + paddingBottom) lên đến ngang tầm mắt (khoảng 35% - 45% chiều cao phòng)
        float safeMinY = minY + paddingBottom;
        float safeMaxY = Mathf.Min(minY + Mathf.Max(3.0f, totalH * 0.45f), maxY - paddingTop);

        if (safeMinY > safeMaxY)
        {
            float midY = (minY + maxY) * 0.5f;
            safeMinY = midY;
            safeMaxY = midY;
        }

        return new Rect(safeMinX, safeMinY, safeMaxX - safeMinX, safeMaxY - safeMinY);
    }

    /// <summary>
    /// Giới hạn chặt chẽ bất kỳ vị trí spawn nào để bắt buộc phải nằm bên trong Safe Spawn Rect của BackgroundBounds.
    /// </summary>
    public Vector3 ClampPositionToSafeBounds(Vector3 worldPos, RoomId room)
    {
        Rect safeRect = GetSafeSpawnRect(room);
        worldPos.x = Mathf.Clamp(worldPos.x, safeRect.xMin, safeRect.xMax);
        worldPos.y = Mathf.Clamp(worldPos.y, safeRect.yMin, safeRect.yMax);
        worldPos.z = 0f;
        return worldPos;
    }

    private Transform GetRandomSpawnPoint(RoomId room)
    {
        List<Transform> list = (room == RoomId.PhongKhach) ? phongKhachSpawnPoints : kitchenSpawnPoints;
        if (list != null && list.Count > 0)
        {
            int idx = UnityEngine.Random.Range(0, list.Count);
            if (list[idx] != null) return list[idx];
        }
        return null;
    }

    private Vector3 GetFallbackPosition(RoomId room)
    {
        Rect safeRect = GetSafeSpawnRect(room);
        float randX = UnityEngine.Random.Range(safeRect.xMin, safeRect.xMax);
        float randY = UnityEngine.Random.Range(safeRect.yMin, Mathf.Min(safeRect.yMin + 1.2f, safeRect.yMax));
        return new Vector3(randX, randY, 0f);
    }

    private GameObject GetRoomRoot(RoomId room)
    {
        if (RoomManager.Instance != null)
        {
            return (room == RoomId.PhongKhach) ? RoomManager.Instance.PhongKhachRoot : RoomManager.Instance.KitchenRoot;
        }

        string rootName = (room == RoomId.PhongKhach) ? "PhongKhach" : "Kitchen";
        return GameObject.Find(rootName);
    }

    private void FindOrSetupTemplate()
    {
        if (monsterTemplate != null) return;

        // Tìm FakeTarget trong Scene
        var targets = FindObjectsByType<TargetController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var t in targets)
        {
            if (t != null)
            {
                monsterTemplate = t;
                // Đảm bảo template gốc không tự hiển thị ngoài ý muốn
                monsterTemplate.gameObject.SetActive(false);
                break;
            }
        }
    }

    private void FindOrSetupSpawnPoints()
    {
        // Đảm bảo RoomManager đã quét xong các phòng và bounds
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.AutoFindRooms();
        }

        // 1. Tìm hoặc tạo Spawn Points an toàn cho PhongKhach
        Rect pkRect = GetSafeSpawnRect(RoomId.PhongKhach);
        float pkFloorY = pkRect.yMin + Mathf.Min(0.3f, pkRect.height * 0.2f);
        float pkMidX = pkRect.center.x;

        if (phongKhachSpawnPoints.Count == 0)
        {
            GameObject pkRoot = GetRoomRoot(RoomId.PhongKhach);
            if (pkRoot != null)
            {
                Transform container = pkRoot.transform.Find("MonsterSpawnPoints");
                if (container == null)
                {
                    GameObject cGo = new GameObject("MonsterSpawnPoints");
                    cGo.transform.SetParent(pkRoot.transform, true);
                    container = cGo.transform;

                    CreateSpawnPointWorld(container, "Spawn_Left", new Vector3(Mathf.Lerp(pkRect.xMin, pkMidX, 0.25f), pkFloorY, 0f), phongKhachSpawnPoints);
                    CreateSpawnPointWorld(container, "Spawn_Center", new Vector3(pkMidX, pkFloorY, 0f), phongKhachSpawnPoints);
                    CreateSpawnPointWorld(container, "Spawn_Right", new Vector3(Mathf.Lerp(pkMidX, pkRect.xMax, 0.75f), pkFloorY, 0f), phongKhachSpawnPoints);
                }
                else
                {
                    foreach (Transform child in container)
                    {
                        phongKhachSpawnPoints.Add(child);
                    }
                }
            }
        }

        // Đảm bảo tất cả điểm spawn của Phong Khach đều nằm trong Safe Bounds
        for (int i = 0; i < phongKhachSpawnPoints.Count; i++)
        {
            if (phongKhachSpawnPoints[i] != null)
            {
                phongKhachSpawnPoints[i].position = ClampPositionToSafeBounds(phongKhachSpawnPoints[i].position, RoomId.PhongKhach);
            }
        }

        // 2. Tìm hoặc tạo Spawn Points an toàn cho Kitchen
        Rect kRect = GetSafeSpawnRect(RoomId.Kitchen);
        float kFloorY = kRect.yMin + Mathf.Min(0.3f, kRect.height * 0.2f);
        float kMidX = kRect.center.x;

        if (kitchenSpawnPoints.Count == 0)
        {
            GameObject kRoot = GetRoomRoot(RoomId.Kitchen);
            if (kRoot != null)
            {
                Transform container = kRoot.transform.Find("MonsterSpawnPoints");
                if (container == null)
                {
                    GameObject cGo = new GameObject("MonsterSpawnPoints");
                    cGo.transform.SetParent(kRoot.transform, true);
                    container = cGo.transform;

                    CreateSpawnPointWorld(container, "Spawn_Kitchen_Stove", new Vector3(Mathf.Lerp(kRect.xMin, kMidX, 0.25f), kFloorY, 0f), kitchenSpawnPoints);
                    CreateSpawnPointWorld(container, "Spawn_Kitchen_Center", new Vector3(kMidX, kFloorY, 0f), kitchenSpawnPoints);
                    CreateSpawnPointWorld(container, "Spawn_Kitchen_Cabinet", new Vector3(Mathf.Lerp(kMidX, kRect.xMax, 0.75f), kFloorY, 0f), kitchenSpawnPoints);
                }
                else
                {
                    foreach (Transform child in container)
                    {
                        kitchenSpawnPoints.Add(child);
                    }
                }
            }
        }

        // Đảm bảo tất cả điểm spawn của Kitchen đều nằm trong Safe Bounds
        for (int i = 0; i < kitchenSpawnPoints.Count; i++)
        {
            if (kitchenSpawnPoints[i] != null)
            {
                kitchenSpawnPoints[i].position = ClampPositionToSafeBounds(kitchenSpawnPoints[i].position, RoomId.Kitchen);
            }
        }
    }

    private void CreateSpawnPointWorld(Transform parent, string name, Vector3 worldPos, List<Transform> list)
    {
        GameObject go = new GameObject(name);
        go.transform.position = worldPos;
        go.transform.SetParent(parent, true); // true = giữ nguyên world position
        list.Add(go.transform);
    }

    private void OnDrawGizmosSelected()
    {
        DrawRoomGizmos(RoomId.PhongKhach, Color.red, Color.green);
        DrawRoomGizmos(RoomId.Kitchen, Color.yellow, Color.cyan);
    }

    private void DrawRoomGizmos(RoomId room, Color boundsColor, Color safeRectColor)
    {
        BackgroundBounds b = GetRoomBounds(room);
        if (b != null)
        {
            Gizmos.color = boundsColor;
            Vector3 center = new Vector3((b.minX + b.maxX) * 0.5f, (b.minY + b.maxY) * 0.5f, 0f);
            Vector3 size = new Vector3(Mathf.Abs(b.maxX - b.minX), Mathf.Abs(b.maxY - b.minY), 0.1f);
            Gizmos.DrawWireCube(center, size);
        }

        Rect safeRect = GetSafeSpawnRect(room);
        Gizmos.color = safeRectColor;
        Vector3 safeCenter = new Vector3(safeRect.center.x, safeRect.center.y, 0f);
        Vector3 safeSize = new Vector3(safeRect.width, safeRect.height, 0.1f);
        Gizmos.DrawWireCube(safeCenter, safeSize);
    }

    [ContextMenu("Spawn Quỷ Máu (Attack - PhongKhach)")]
    public void DebugSpawnQuyMauPK() => SpawnQuyMau(RoomId.PhongKhach);

    [ContextMenu("Spawn The Shy (Turn Off Lights - PhongKhach)")]
    public void DebugSpawnTheShyPK() => SpawnTheShy(RoomId.PhongKhach);

    [ContextMenu("Spawn Quỷ Máu (Attack - Kitchen)")]
    public void DebugSpawnQuyMauKitchen() => SpawnQuyMau(RoomId.Kitchen);

    [ContextMenu("Spawn The Shy (Turn Off Lights - Kitchen)")]
    public void DebugSpawnTheShyKitchen() => SpawnTheShy(RoomId.Kitchen);

    [ContextMenu("Xóa Hết Quái Hiện Có")]
    public void DebugDespawnAll() => DespawnAllMonsters();
}
