using Core.Item;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public List<InventoryItemData> Inventory = new List<InventoryItemData>();

    [SerializeField] private InventoryItemData currentSelect;

    public InventoryItemData CurrentSelect { get => currentSelect; set => currentSelect = value; }
    public event Action<InventoryItemData> Selected;
    public event Action<InventoryItemData> OnUsing;
    public event Action OnPickUpItem;
    public event Action OnUsingItem;

    private static readonly HashSet<string> PuzzleItemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "BrokenAntenna",
        "BlackTapeRoll",
        "CigaretteButt",
        "PhotoShard",
        "RottenMeat",
        "WaterBottle",
        "PhoneNumberShard",
        "CursedGear",
        "CrayonBox",
        "FaceMask",
        "Wrench",
        "Tape",
        "Antenna"
    };

    private void Start()
    {
        // Giữ lại các item đã có nếu được load từ GameData
        if (Inventory == null)
        {
            InitInventory();
        }
    }

    public void PickUpItem(ItemData item)
    {
        if (item == null) return;

        var effect = (Core.Item.ItemManager.Instance != null && Core.Item.ItemManager.Instance.ItemEffect != null)
            ? Core.Item.ItemManager.Instance.ItemEffect.GetEffects(item.Id)
            : null;

        InventoryItemData itemInventory = new InventoryItemData(item, effect);
        Inventory.Add(itemInventory);
        currentSelect = null;

        // Đồng bộ GameData
        SyncAddGameData(item.Id);

        OnPickUpItem?.Invoke();
    }

    public void UsingByInteractInGame()
    {
        if (currentSelect == null) return;

        RemoveItemInventory(currentSelect);
        currentSelect = null;
        OnUsingItem?.Invoke();
    }

    public void Using()
    {
        if (currentSelect == null) return;

        string id = currentSelect.itemId;

        // 1. Kiểm tra nếu là vật phẩm giải đố (Puzzle Item) -> không dùng trực tiếp qua nút Use
        if (IsPuzzleItem(id))
        {
            Debug.Log($"<color=orange>[ItemInventory] '{id}' là vật phẩm giải đố tương tác với môi trường. Hãy click vào đối tượng trong phòng để sử dụng thay vì dùng trực tiếp!</color>");
            return;
        }

        // 2. Kích hoạt hiệu ứng tiêu hao (Consumable Effects)
        ApplyConsumableEffect(id);

        // 3. Tiêu hao item (chỉ dùng 1 lần)
        var usedItem = currentSelect;
        RemoveItemInventory(usedItem);

        currentSelect = null;
        OnUsing?.Invoke(usedItem);
        OnUsingItem?.Invoke();
    }

    private void ApplyConsumableEffect(string itemId)
    {
        switch (itemId)
        {
            case "Health":
                if (RunController.Instance != null)
                {
                    RunController.Instance.Heal(2);
                }
                Debug.Log("<color=green>[ItemInventory] Đã sử dụng Bình Máu (Health): Hồi 2 máu sinh tồn!</color>");
                break;

            case "StarterPotion":
                if (RunController.Instance != null)
                {
                    RunController.Instance.Heal(2);
                }
                Debug.Log("<color=green>[ItemInventory] Đã sử dụng Starter Potion: Hồi 2 máu sinh tồn!</color>");
                break;

            case "ColdCoffee":
                if (RunController.Instance != null)
                {
                    RunController.Instance.Heal(1);
                }
                Debug.Log("<color=green>[ItemInventory] Đã uống Cold Coffee: Hồi 1 máu sinh tồn!</color>");
                break;

            case "Shield":
                if (RunController.Instance != null && RunController.Instance.Buffs != null)
                {
                    RunController.Instance.Buffs.hasShield = true;
                    RunController.Instance.Buffs.shieldUsed = false;
                }
                Debug.Log("<color=green>[ItemInventory] Đã kích hoạt Khiên bảo vệ (Shield): Miễn nhiễm 1 đòn tấn công tiếp theo từ quái vật!</color>");
                break;

            case "LightWard":
                if (RunController.Instance != null && RunController.Instance.Buffs != null)
                {
                    RunController.Instance.Buffs.hasLightImmunity = true;
                }
                Debug.Log("<color=green>[ItemInventory] Đã kích hoạt Bùa hộ mệnh (LightWard): Miễn nhiễm 1 lần tắt đèn từ quái vật The Shy!</color>");
                break;

            case "AdrenalineSerum":
                if (RunController.Instance != null)
                {
                    RunController.Instance.Heal(2);
                }
                Debug.Log("<color=green>[ItemInventory] Đã tiêm Adrenaline Serum: Hồi phục 2 máu sinh tồn khẩn cấp!</color>");
                break;

            case "Coin":
                if (RunController.Instance != null && RunController.Instance.Buffs != null)
                {
                    RunController.Instance.Buffs.hasCoin = true;
                    RunController.Instance.Buffs.coinUsed = false;
                }
                Debug.Log("<color=yellow>[ItemInventory] Đã kích hoạt Coin: Nhân đôi tiền thưởng cho lần chụp quái vật đầu tiên!</color>");
                break;

            default:
                Debug.Log($"<color=cyan>[ItemInventory] Đã sử dụng '{itemId}'. (Chưa có hiệu ứng cụ thể, đã tiêu hao)</color>");
                break;
        }
    }

    public static bool IsPuzzleItem(string itemId)
    {
        if (string.IsNullOrEmpty(itemId)) return false;
        return PuzzleItemIds.Contains(itemId);
    }

    public void SetSelected(InventoryItemData item)
    {
        currentSelect = item;
        if (currentSelect == null) 
        {
            Selected?.Invoke(null);
            return;
        } 
        Selected?.Invoke(currentSelect);
    }

    public void InitInventory()
    {
        Inventory = new List<InventoryItemData>();
    }

    public void AddItem(ItemData item, bool syncToGameData = true)
    {
        if (item == null) return;

        var effect = (Core.Item.ItemManager.Instance != null && Core.Item.ItemManager.Instance.ItemEffect != null)
            ? Core.Item.ItemManager.Instance.ItemEffect.GetEffects(item.Id)
            : null;

        InventoryItemData itemInventory = new InventoryItemData(item, effect);
        Inventory.Add(itemInventory);

        if (syncToGameData)
        {
            SyncAddGameData(item.Id);
        }
    }

    public void AddItemSilently(ItemData item)
    {
        AddItem(item, syncToGameData: false);
    }

    public void RemoveItemInventory(InventoryItemData item)
    {
        if (item == null) return;

        if (Inventory.Contains(item))
        {
            Inventory.Remove(item);
        }

        // Đồng bộ xóa khỏi GameData
        SyncRemoveGameData(item.itemId);
    }

    public void InitInventoryTest()
    {
        Inventory = new List<InventoryItemData>();

        if (Core.Item.ItemManager.Instance == null || Core.Item.ItemManager.Instance.LibraryItem == null) return;

        var listLibrary = Core.Item.ItemManager.Instance.LibraryItem.GetAllItems();
        foreach (var item in listLibrary)
        {
            var effect = Core.Item.ItemManager.Instance.ItemEffect != null
                ? Core.Item.ItemManager.Instance.ItemEffect.GetEffects(item.Id)
                : null;

            InventoryItemData itemInventory = new InventoryItemData(item, effect);
            Inventory.Add(itemInventory);
        }
    }

    private void SyncAddGameData(string itemId)
    {
        if (string.IsNullOrEmpty(itemId)) return;

        if (RunController.Instance != null && RunController.Instance.GameData != null)
        {
            var gData = RunController.Instance.GameData;
            if (gData.inventoryData != null && gData.inventoryData.items != null)
            {
                gData.inventoryData.items.Add(new InventoryItemData { itemId = itemId });
            }

            if (gData.worldStateData != null)
            {
                if (gData.worldStateData.collectedItemIds == null)
                {
                    gData.worldStateData.collectedItemIds = new List<string>();
                }
                if (!gData.worldStateData.collectedItemIds.Contains(itemId))
                {
                    gData.worldStateData.collectedItemIds.Add(itemId);
                    Debug.Log($"<color=cyan>[ItemInventory] Đã ghi nhận '{itemId}' vào collectedItemIds để mở khóa trong Shop.</color>");
                }
            }
        }
    }

    private void SyncRemoveGameData(string itemId)
    {
        if (string.IsNullOrEmpty(itemId)) return;

        if (RunController.Instance != null && RunController.Instance.GameData != null)
        {
            var gData = RunController.Instance.GameData;
            if (gData.inventoryData != null && gData.inventoryData.items != null)
            {
                var match = gData.inventoryData.items.Find(x => x.itemId == itemId);
                if (match != null)
                {
                    gData.inventoryData.items.Remove(match);
                }
            }
        }
    }
}
