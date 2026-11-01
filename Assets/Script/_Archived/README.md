# Thư Mục _Archived (CureShot)

Thư mục này dùng để lưu trữ các file code cũ, script thử nghiệm tạm thời (dummy/stub/debug) chưa dùng hoặc đã có module mới thay thế.

## Danh sách file và lý do lưu trữ:

| File | Mô tả | Trạng thái / Script thay thế |
|---|---|---|
| `Player.cs` | Class `Player` rỗng không có nội dung. | Hệ thống sử dụng `PlayerManager`, `HealthPlayer`, `MoneyPlayer` (`02_Gameplay/Player/`). |
| `DOTWeen_Interact.cs` | Class `DOTWeen_Interact` rỗng kế thừa `MonoBehaviour`. | Hệ thống tương tác animation sử dụng `ItemInteract_DOTween`, `CabinetTransition`, `FlexibleItemFlyTransition` (`03_PuzzleInteract/`). |
| `ItemDataRuntime.cs` | Class `ItemDataRuntime` rỗng kế thừa `MonoBehaviour`. | Hệ thống vật phẩm sử dụng `ItemData`, `InventoryItemData`, `ItemUseContext` (`08_DataModels/`, `04_ItemsAndInventory/`). |
| `Base__EffectItem.cs` | Script thử nghiệm hiệu ứng item mẫu. | Đã thay thế bằng các hiệu ứng cụ thể: `HealEffect`, `ShieldEffect`, `BloodEffect`, `CoinEffect`, `GetEffect` (`04_ItemsAndInventory/Effects/`). |
| `Base__EffectItem1.cs` | Script thử nghiệm hiệu ứng item mẫu số 2. | Đã thay thế bằng các hiệu ứng chuẩn trong `04_ItemsAndInventory/Effects/`. |
| `CheckUI.cs` | Tiện ích debug click UI `UIClickDetector`. | Script công cụ hỗ trợ debug EventSystem khi cần kiểm tra tia Raycast UI bị đè. |

> **Lưu ý:** Tuyệt đối không xóa file kèm file `.meta` tùy tiện khi chưa có yêu cầu từ lập trình viên để đảm bảo tính toàn vẹn của dự án Unity.

