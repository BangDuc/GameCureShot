using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Đồng bộ toàn bộ các tương tác đóng mở tủ (CabinetTransition), giải đố (ItemInteractGiaiDo)
/// và cơ chế rơi vật phẩm (CabinetItemDrop) cho căn phòng Kitchen (Nhà bếp).
/// Hỗ trợ cả Edit Mode ([ExecuteAlways]), Prefab Mode và Play Mode.
/// </summary>
[ExecuteAlways]
public class KitchenInteractionSync : MonoBehaviour
{
    public const int INTERACTABLE_LAYER = 8; // Layer 8: Interactable

    [Header("Trạng Thái Đồng Bộ (Sync Status)")]
    [SerializeField] private bool isSynchronized = false;

    [Header("Các Tủ Đóng Mở (Cabinet Transitions)")]
    [SerializeField] private CabinetTransition tuLanhTransition;
    [SerializeField] private CabinetTransition nganTu1Transition;
    [SerializeField] private CabinetTransition nganTu2Transition;
    [SerializeField] private CabinetTransition loNuongTransition;

    [Header("Các Câu Đố Giải Mã (Item Puzzles)")]
    [SerializeField] private ItemInteractGiaiDo tuLanhPuzzle;
    [SerializeField] private UnlockByItem_Cabinet tuLanhUnlock;
    [SerializeField] private ItemInteractGiaiDo voiNuocPuzzle;
    [SerializeField] private UnlockByItem_WaterPipe voiNuocUnlock;

    public bool IsSynchronized => isSynchronized;
    public CabinetTransition TuLanhTransition => tuLanhTransition;
    public CabinetTransition NganTu1Transition => nganTu1Transition;
    public CabinetTransition NganTu2Transition => nganTu2Transition;
    public CabinetTransition LoNuongTransition => loNuongTransition;

    private void Awake()
    {
        SyncInteractions();
    }

    private void OnEnable()
    {
        if (!isSynchronized || tuLanhTransition == null || loNuongTransition == null || nganTu2Transition == null)
        {
            SyncInteractions();
        }
    }

    private void Start()
    {
        if (!isSynchronized)
        {
            SyncInteractions();
        }
    }

    /// <summary>
    /// Đảm bảo KitchenInteractionSync luôn có mặt trên GameObject Kitchen và đã thực hiện đồng bộ.
    /// </summary>
    public static KitchenInteractionSync EnsureOn(GameObject kitchenGo)
    {
        if (kitchenGo == null) return null;

        var sync = kitchenGo.GetComponent<KitchenInteractionSync>();
        if (sync == null)
        {
            sync = kitchenGo.AddComponent<KitchenInteractionSync>();
        }
        sync.SyncInteractions();
        return sync;
    }

    /// <summary>
    /// Quét và cấu hình toàn bộ các tương tác: đóng mở tủ, giải đố và rơi vật phẩm.
    /// </summary>
    [ContextMenu("Synchronize All Kitchen Interactions")]
    public void SyncInteractions()
    {
        try
        {
            // 1. Đồng bộ tủ lạnh (TuLanh)
            SyncTuLanh();

            // 2. Đồng bộ 2 ngăn tủ bếp (NganTu_1 và NganTu_2 thuộc TuBep_To)
            SyncNganTu1();
            SyncNganTu2();

            // 3. Đồng bộ lò nướng (LoNuong)
            SyncLoNuong();

            // 4. Đồng bộ giải đố ống nước / vòi nước (VoiNuoc / BeNuocFull với Wrench)
            SyncWaterPipesPuzzle();

            isSynchronized = true;
            Debug.Log("<color=cyan>[KitchenInteractionSync] Đã đồng bộ thành công các tương tác đóng mở tủ và rơi vật phẩm cho Kitchen!</color>");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[KitchenInteractionSync] Lỗi trong quá trình đồng bộ: {ex.Message}\n{ex.StackTrace}");
        }
    }

    #region 1. Lò Nướng (LoNuong)
    /// <summary>
    /// Đồng bộ cơ chế đóng mở và rơi vật phẩm cho lò nướng (LoNuong):
    /// - Rơi Health (bình máu) khi mở lò nướng
    /// </summary>
    [ContextMenu("Sync LoNuong (Oven)")]
    public void SyncLoNuong()
    {
        var loNuong = FindChildRecursive(transform, "LoNuong");
        if (loNuong == null) return;

        loNuong.gameObject.layer = 0;
        CleanupBlockingComponents(loNuong.gameObject);

        var dong = FindChildRecursive(loNuong, "Dong_2");
        var mo = FindChildRecursive(loNuong, "Mo_2");

        if (dong == null || mo == null) return;

        var srDong = dong.GetComponent<SpriteRenderer>();
        var srMo = mo.GetComponent<SpriteRenderer>();

        if (srDong == null || srMo == null) return;

        loNuongTransition = loNuong.GetComponent<CabinetTransition>();
        if (loNuongTransition == null)
        {
            loNuongTransition = loNuong.gameObject.AddComponent<CabinetTransition>();
        }
        loNuongTransition.Setup(srDong, srMo, 0.2f, false);

        ConfigureCabinetInteractivePart(dong.gameObject, loNuongTransition, true);
        ConfigureCabinetInteractivePart(mo.gameObject, loNuongTransition, false);

        // Gắn cơ chế rơi vật phẩm: Bình Máu (Health)
        var drop = loNuong.GetComponent<CabinetItemDrop>();
        if (drop == null)
        {
            drop = loNuong.gameObject.AddComponent<CabinetItemDrop>();
        }
        drop.Setup("Health", new Vector3(0f, -0.4f, 0f));

        var baseLoNuong = FindChildRecursive(loNuong, "BaseLoNuong");
        if (baseLoNuong != null)
        {
            baseLoNuong.gameObject.layer = 0;
            CleanupBlockingComponents(baseLoNuong.gameObject);
        }
    }

    [ContextMenu("Toggle LoNuong")]
    public void ToggleLoNuong()
    {
        if (loNuongTransition != null)
        {
            loNuongTransition.Toggle();
        }
    }
    #endregion

    #region 2. Ngăn Tủ 2 (NganTu_2)
    /// <summary>
    /// Đồng bộ cơ chế đóng mở và rơi vật phẩm cho ngăn kéo tủ bếp 2 (NganTu_2):
    /// - Rơi LightWard (Bùa miễn nhiễm quái vật tắt đèn) khi mở
    /// </summary>
    [ContextMenu("Sync NganTu_2 (Drawer 2)")]
    public void SyncNganTu2()
    {
        var tuBepTo = FindChildRecursive(transform, "TuBep_To");
        Transform rootSearch = tuBepTo != null ? tuBepTo : transform;

        var nganTu2 = FindChildRecursive(rootSearch, "NganTu_2");
        if (nganTu2 == null) return;

        nganTu2.gameObject.layer = 0;
        CleanupBlockingComponents(nganTu2.gameObject);

        var dong = FindChildRecursive(nganTu2, "Dong_1");
        var mo = FindChildRecursive(nganTu2, "Mo_1");

        if (dong == null || mo == null) return;

        var srDong = dong.GetComponent<SpriteRenderer>();
        var srMo = mo.GetComponent<SpriteRenderer>();

        if (srDong == null || srMo == null) return;

        nganTu2Transition = nganTu2.GetComponent<CabinetTransition>();
        if (nganTu2Transition == null)
        {
            nganTu2Transition = nganTu2.gameObject.AddComponent<CabinetTransition>();
        }
        nganTu2Transition.Setup(srDong, srMo, 0.2f, false);

        ConfigureCabinetInteractivePart(dong.gameObject, nganTu2Transition, true);
        ConfigureCabinetInteractivePart(mo.gameObject, nganTu2Transition, false);

        // Gắn cơ chế rơi vật phẩm: LightWard
        var drop = nganTu2.GetComponent<CabinetItemDrop>();
        if (drop == null)
        {
            drop = nganTu2.gameObject.AddComponent<CabinetItemDrop>();
        }
        drop.Setup("LightWard", new Vector3(0f, -0.3f, 0f));
    }

    [ContextMenu("Toggle NganTu_2")]
    public void ToggleNganTu2()
    {
        if (nganTu2Transition != null)
        {
            nganTu2Transition.Toggle();
        }
    }
    #endregion

    #region 3. Ngăn Tủ 1 (NganTu_1)
    /// <summary>
    /// Đồng bộ cơ chế đóng mở và rơi vật phẩm cho ngăn kéo tủ bếp 1 (NganTu_1):
    /// - Rơi Health (Bình máu) khi mở
    /// </summary>
    [ContextMenu("Sync NganTu_1 (Drawer 1)")]
    public void SyncNganTu1()
    {
        var tuBepTo = FindChildRecursive(transform, "TuBep_To");
        Transform rootSearch = tuBepTo != null ? tuBepTo : transform;

        var nganTu1 = FindChildRecursive(rootSearch, "NganTu_1");
        if (nganTu1 == null) return;

        nganTu1.gameObject.layer = 0;
        CleanupBlockingComponents(nganTu1.gameObject);

        var dong = FindChildRecursive(nganTu1, "Dong");
        var mo = FindChildRecursive(nganTu1, "Mo");

        if (dong == null || mo == null) return;

        var srDong = dong.GetComponent<SpriteRenderer>();
        var srMo = mo.GetComponent<SpriteRenderer>();

        if (srDong == null || srMo == null) return;

        nganTu1Transition = nganTu1.GetComponent<CabinetTransition>();
        if (nganTu1Transition == null)
        {
            nganTu1Transition = nganTu1.gameObject.AddComponent<CabinetTransition>();
        }
        nganTu1Transition.Setup(srDong, srMo, 0.2f, false);

        ConfigureCabinetInteractivePart(dong.gameObject, nganTu1Transition, true);
        ConfigureCabinetInteractivePart(mo.gameObject, nganTu1Transition, false);

        // Gắn cơ chế rơi vật phẩm: Health
        var drop = nganTu1.GetComponent<CabinetItemDrop>();
        if (drop == null)
        {
            drop = nganTu1.gameObject.AddComponent<CabinetItemDrop>();
        }
        drop.Setup("Health", new Vector3(0f, -0.3f, 0f));

        var baseTuBep = FindChildRecursive(rootSearch, "BaseTuBepTo");
        if (baseTuBep != null)
        {
            baseTuBep.gameObject.layer = 0;
            CleanupBlockingComponents(baseTuBep.gameObject);
        }
    }

    [ContextMenu("Toggle NganTu_1")]
    public void ToggleNganTu1()
    {
        if (nganTu1Transition != null)
        {
            nganTu1Transition.Toggle();
        }
    }
    #endregion

    #region 4. Tủ Lạnh (TuLanh)
    /// <summary>
    /// Đồng bộ cơ chế đóng mở và rơi vật phẩm cho tủ lạnh (TuLanh):
    /// - Rơi ColdCoffee (Cà phê lạnh hồi 1 máu) khi mở
    /// </summary>
    [ContextMenu("Sync TuLanh (Fridge)")]
    public void SyncTuLanh()
    {
        var tuLanh = FindChildRecursive(transform, "TuLanh");
        if (tuLanh == null) return;

        var dong = FindChildRecursive(tuLanh, "TuLanhDong");
        var mo = FindChildRecursive(tuLanh, "TuLanhMo");

        if (dong == null || mo == null) return;

        var srDong = dong.GetComponent<SpriteRenderer>();
        var srMo = mo.GetComponent<SpriteRenderer>();

        if (srDong == null || srMo == null) return;

        tuLanhTransition = tuLanh.GetComponent<CabinetTransition>();
        if (tuLanhTransition == null)
        {
            tuLanhTransition = tuLanh.gameObject.AddComponent<CabinetTransition>();
        }
        tuLanhTransition.Setup(srDong, srMo, 0.25f, false);

        ConfigureCabinetInteractivePart(dong.gameObject, tuLanhTransition, true);
        ConfigureCabinetInteractivePart(mo.gameObject, tuLanhTransition, false);

        // Gắn cơ chế rơi vật phẩm: ColdCoffee
        var drop = tuLanh.GetComponent<CabinetItemDrop>();
        if (drop == null)
        {
            drop = tuLanh.gameObject.AddComponent<CabinetItemDrop>();
        }
        drop.Setup("ColdCoffee", new Vector3(0.3f, -0.6f, 0f));

        // Cấu hình giải đố tùy chọn bằng FaceMask
        tuLanhUnlock = tuLanh.GetComponent<UnlockByItem_Cabinet>();
        if (tuLanhUnlock == null)
        {
            tuLanhUnlock = tuLanh.gameObject.AddComponent<UnlockByItem_Cabinet>();
        }
        tuLanhUnlock.SetItemId("FaceMask");
        tuLanhUnlock.SetCabinet(tuLanhTransition);

        tuLanhPuzzle = tuLanh.GetComponent<ItemInteractGiaiDo>();
        if (tuLanhPuzzle == null)
        {
            tuLanhPuzzle = tuLanh.gameObject.AddComponent<ItemInteractGiaiDo>();
        }

        var baseTuLanh = FindChildRecursive(tuLanh, "BaseTuLanh");
        if (baseTuLanh != null)
        {
            baseTuLanh.gameObject.layer = 0;
            CleanupBlockingComponents(baseTuLanh.gameObject);
        }
    }

    [ContextMenu("Toggle TuLanh")]
    public void ToggleTuLanh()
    {
        if (tuLanhTransition != null)
        {
            tuLanhTransition.Toggle();
        }
    }
    #endregion

    #region 5. Đường Ống Nước / Vòi Nước (Wrench Puzzle)
    [ContextMenu("Sync Water Pipe Puzzle")]
    public void SyncWaterPipesPuzzle()
    {
        var voiNuoc = FindChildRecursive(transform, "VoiNuoc");
        if (voiNuoc == null)
        {
            voiNuoc = FindChildRecursive(transform, "BeNuoc");
        }
        if (voiNuoc == null) return;

        var leakList = new List<GameObject>();
        string[] leakNames = { "NuocTran", "ChayNuoc", "VungNuoc" };

        foreach (var leakName in leakNames)
        {
            FindAllChildrenRecursive(transform, leakName, leakList);
        }

        voiNuoc.gameObject.layer = INTERACTABLE_LAYER;
        EnsureCollider(voiNuoc.gameObject);

        voiNuocUnlock = voiNuoc.GetComponent<UnlockByItem_WaterPipe>();
        if (voiNuocUnlock == null)
        {
            voiNuocUnlock = voiNuoc.gameObject.AddComponent<UnlockByItem_WaterPipe>();
        }
        voiNuocUnlock.SetItemId("Wrench");
        voiNuocUnlock.SetWaterLeakObjects(leakList.ToArray());

        voiNuocPuzzle = voiNuoc.GetComponent<ItemInteractGiaiDo>();
        if (voiNuocPuzzle == null)
        {
            voiNuocPuzzle = voiNuoc.gameObject.AddComponent<ItemInteractGiaiDo>();
        }
    }
    #endregion

    #region Helpers
    private void ConfigureCabinetInteractivePart(GameObject targetGo, CabinetTransition transition, bool initiallyActive)
    {
        targetGo.layer = INTERACTABLE_LAYER;
        EnsureCollider(targetGo);

        var interactDotween = targetGo.GetComponent<ItemInteract_DOTween>();
        if (interactDotween == null)
        {
            interactDotween = targetGo.AddComponent<ItemInteract_DOTween>();
        }
        interactDotween.DOTween_Transition = transition;

        var sr = targetGo.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = initiallyActive ? 1f : 0f;
            sr.color = c;
        }

        targetGo.SetActive(initiallyActive);
    }

    private Collider2D EnsureCollider(GameObject go)
    {
        var col = go.GetComponent<Collider2D>();
        if (col == null)
        {
            var poly = go.AddComponent<PolygonCollider2D>();
            col = poly;
        }
        col.isTrigger = true;

        if (col is PolygonCollider2D polyCol && (polyCol.pathCount == 0 || polyCol.GetPath(0).Length == 0))
        {
            var sr = go.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                float ppu = sr.sprite.pixelsPerUnit > 0 ? sr.sprite.pixelsPerUnit : 100f;
                Vector2 pivot = sr.sprite.pivot / ppu;
                Vector2 size = sr.sprite.rect.size / ppu;

                float minX = -pivot.x;
                float maxX = size.x - pivot.x;
                float minY = -pivot.y;
                float maxY = size.y - pivot.y;

                if (size.x > 0.01f && size.y > 0.01f)
                {
                    polyCol.pathCount = 1;
                    polyCol.SetPath(0, new Vector2[]
                    {
                        new Vector2(minX, minY),
                        new Vector2(maxX, minY),
                        new Vector2(maxX, maxY),
                        new Vector2(minX, maxY)
                    });
                }
            }
        }

        return col;
    }

    private void CleanupBlockingComponents(GameObject go)
    {
        var cols = go.GetComponents<Collider2D>();
        foreach (var c in cols)
        {
            SafeDestroy(c);
        }

        var puzzles = go.GetComponents<ItemInteractGiaiDo>();
        foreach (var p in puzzles)
        {
            SafeDestroy(p);
        }
    }

    private void SafeDestroy(UnityEngine.Object obj)
    {
        if (obj == null) return;
        if (Application.isPlaying)
        {
            Destroy(obj);
        }
        else
        {
            DestroyImmediate(obj);
        }
    }

    private Transform FindChildRecursive(Transform parent, string childName)
    {
        if (parent == null) return null;

        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            if (string.Equals(child.name, childName, StringComparison.OrdinalIgnoreCase))
            {
                return child;
            }

            var found = FindChildRecursive(child, childName);
            if (found != null) return found;
        }

        return null;
    }

    private void FindAllChildrenRecursive(Transform parent, string childName, List<GameObject> results)
    {
        if (parent == null) return;

        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            if (string.Equals(child.name, childName, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(child.gameObject);
            }

            FindAllChildrenRecursive(child, childName, results);
        }
    }
    #endregion
}
