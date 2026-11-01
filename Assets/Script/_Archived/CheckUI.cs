using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIClickDetector : MonoBehaviour
{
    void Update()
    {
        // Khi người dùng click chuột trái (hoặc chạm màn hình)
        if (Input.GetMouseButtonDown(0))
        {
            // Kiểm tra xem chuột có đang nằm trên một đối tượng UI nào không
            if (EventSystem.current != null)
            {
                // Cách 1: Xem EventSystem đang chọn trực tiếp GameObject nào
                GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
                if (currentSelected != null)
                {
                    Debug.Log($"<color=cyan>[Selected] Bạn vừa bấm trúng UI tên là: </color><b>{currentSelected.name}</b>", currentSelected);
                }

                // Cách 2 (Chính xác nhất): Bắn tia Raycast để dò TẤT CẢ UI nằm dưới con trỏ chuột
                PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
                eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

                if (results.Count > 0)
                {
                    // Phần tử đầu tiên (index 0) chính là UI nằm trên cùng, chặn tất cả UI phía sau
                    Debug.Log($"<color=yellow>[Raycast Top] UI nằm TRÊN CÙNG chặn chuột là: </color><b>{results[0].gameObject.name}</b>", results[0].gameObject);

                    // In thêm danh sách các UI bị đè phía dưới nếu bạn muốn kiểm tra sâu hơn
                    for (int i = 1; i < results.Count; i++)
                    {
                        Debug.Log($"   -> UI phía dưới bị che: {results[i].gameObject.name}");
                    }
                }
                else
                {
                    Debug.Log("<color=red>[Raycast] Không bấm trúng bất kỳ UI nào!</color>");
                }
            }
        }
    }
}
