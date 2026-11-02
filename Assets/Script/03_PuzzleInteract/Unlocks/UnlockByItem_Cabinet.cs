using UnityEngine;

/// <summary>
/// Mở khóa tủ/ngăn kéo bằng vật phẩm (ví dụ: dùng Mặt nạ phòng độc FaceMask hoặc Chìa khóa).
/// Khi mở khóa thành công, tự động kích hoạt CabinetTransition để mở cánh tủ.
/// </summary>
public class UnlockByItem_Cabinet : UnlockByItem_Base
{
    [Header("Cấu Hình Mở Khóa Tủ")]
    [SerializeField] private CabinetTransition cabinetTransition;

    public CabinetTransition CabinetTransition => cabinetTransition;

    public void SetCabinet(CabinetTransition transition)
    {
        cabinetTransition = transition;
    }

    public override void TuongTac()
    {
        base.TuongTac();
        if (cabinetTransition != null)
        {
            cabinetTransition.OpenCabinet();
        }
        else
        {
            var inParent = GetComponentInParent<CabinetTransition>();
            if (inParent != null)
            {
                inParent.OpenCabinet();
            }
        }
    }
}

