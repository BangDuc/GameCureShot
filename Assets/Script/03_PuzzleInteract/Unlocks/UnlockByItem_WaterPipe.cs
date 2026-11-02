using UnityEngine;

/// <summary>
/// Mở khóa / sửa đường ống nước bằng Cờ-lê (Wrench).
/// Khi mở khóa thành công, dập tắt các dòng nước rò rỉ / vũng nước tràn (ChayNuoc, NuocTran, VungNuoc).
/// </summary>
public class UnlockByItem_WaterPipe : UnlockByItem_Base
{
    [Header("Các Đối Tượng Nước Cần Khóa / Ẩn")]
    [SerializeField] private GameObject[] waterLeakObjects;

    public GameObject[] WaterLeakObjects => waterLeakObjects;

    public void SetWaterLeakObjects(params GameObject[] objects)
    {
        waterLeakObjects = objects;
    }

    public override void TuongTac()
    {
        base.TuongTac();
        if (waterLeakObjects != null)
        {
            foreach (var go in waterLeakObjects)
            {
                if (go != null)
                {
                    go.SetActive(false);
                }
            }
        }
    }
}

