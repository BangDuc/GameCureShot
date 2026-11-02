using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class UI_OnStart : MonoBehaviour
{
    [SerializeField]public List<PanelView> list_overlays;

    private void Start()
    {
        foreach(Transform child in this.transform)
        {
            PanelView overlay = child.GetComponent<PanelView>();
            if(overlay != null )
            {
                list_overlays.Add(overlay);
            }
            

        }
        RefreshOverlays();
    }

    public void RefreshOverlays()
    {
        foreach (var overlay in list_overlays)
        {
            // 1. Bật UI lên
            overlay.gameObject.SetActive(true);

            // 2. Tìm tất cả các thành phần liên quan đến Layout bên trong Panel này (từ cha đến con)
            // Bao gồm: VerticalLayoutGroup, HorizontalLayoutGroup, ContentSizeFitter...
            LayoutGroup[] subLayouts = overlay.GetComponentsInChildren<LayoutGroup>(true);
            ContentSizeFitter[] fitters = overlay.GetComponentsInChildren<ContentSizeFitter>(true);

            // Ép các thành phần Layout con tính toán trước
            foreach (var layout in subLayouts)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());
            }

            foreach (var fitter in fitters)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(fitter.GetComponent<RectTransform>());
            }

            // Cuối cùng mới ép tấm Panel cha tính toán lại
            RectTransform rect = overlay.GetComponent<RectTransform>();
            if (rect != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            }
        }

        //Debug.Log("Đã ép tính toán sâu từ trong ra ngoài thành công!");

        // 3. Tiến hành tắt đi
        foreach (var overlay in list_overlays)
        {
            overlay.gameObject.SetActive(false);
        }
    }

}
