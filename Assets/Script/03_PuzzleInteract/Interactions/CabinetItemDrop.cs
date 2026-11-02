using Core.Item;
using DG.Tweening;
using UnityEngine;

public class CabinetItemDrop : MonoBehaviour
{
    [Header("Cấu Hình Vật Phẩm Rơi")]
    [SerializeField] private string dropItemId = "Health";
    [SerializeField] private Vector3 dropOffset = new Vector3(0f, -0.6f, 0f);
    [SerializeField] private bool hasDropped = false;

    [Header("Tham Chiếu Tủ")]
    [SerializeField] private CabinetTransition cabinet;

    public string DropItemId { get => dropItemId; set => dropItemId = value; }
    public Vector3 DropOffset { get => dropOffset; set => dropOffset = value; }
    public bool HasDropped => hasDropped;

    private void Awake()
    {
        if (cabinet == null)
        {
            cabinet = GetComponent<CabinetTransition>();
        }
    }

    private void OnEnable()
    {
        if (cabinet != null)
        {
            cabinet.OnCabinetOpened -= HandleCabinetOpened;
            cabinet.OnCabinetOpened += HandleCabinetOpened;
        }
    }

    private void OnDisable()
    {
        if (cabinet != null)
        {
            cabinet.OnCabinetOpened -= HandleCabinetOpened;
        }
    }

    public void Setup(string itemId, Vector3 offset)
    {
        dropItemId = itemId;
        dropOffset = offset;
    }

    public void ResetDrop()
    {
        hasDropped = false;
    }

    private void HandleCabinetOpened()
    {
        if (hasDropped || string.IsNullOrEmpty(dropItemId)) return;

        hasDropped = true;
        SpawnDrop();
    }

    public void SpawnDrop()
    {
        var itemData = ItemManager.GetItemData(dropItemId);
        if (itemData == null)
        {
            Debug.LogWarning($"[CabinetItemDrop] Không tìm thấy dữ liệu item cho '{dropItemId}'. Bỏ qua rơi đồ.");
            return;
        }

        Vector3 spawnStartPos = transform.position;
        Vector3 targetPos = transform.position + dropOffset;

        GameObject dropGo = new GameObject($"DroppedItem_{dropItemId}");
        dropGo.transform.position = spawnStartPos;
        dropGo.transform.localScale = Vector3.one * 0.5f;

        var pickup = dropGo.AddComponent<DroppedItemPickup>();
        pickup.Initialize(dropItemId, itemData);

        // Hiệu ứng văng ra từ tủ và nảy trên sàn
        dropGo.transform.DOScale(Vector3.one, 0.35f);
        dropGo.transform.DOJump(targetPos, jumpPower: 0.6f, numJumps: 1, duration: 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                Debug.Log($"<color=cyan>[CabinetItemDrop] Vật phẩm '{itemData.itemName}' đã rơi ra từ tủ tại vị trí {targetPos}!</color>");
            });
    }
}
