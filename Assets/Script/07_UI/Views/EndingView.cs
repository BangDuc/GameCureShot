using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingView : MonoBehaviour
{
    [SerializeField] private TMP_Text endingTypeText;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image endingImage;
    [SerializeField] private UIButtonView continueButton;

    public event Action ContinueClicked;

    private void Awake()
    {
        AutoFindUIComponents();
    }

    private void OnEnable()
    {
        AutoFindUIComponents();

        if (continueButton != null)
        {
            continueButton.Clicked -= HandleButtonClicked;
            continueButton.Clicked += HandleButtonClicked;
        }
    }

    private void OnDisable()
    {
        if (continueButton != null)
        {
            continueButton.Clicked -= HandleButtonClicked;
        }
    }

    public void AutoFindUIComponents()
    {
        // 1. Tìm kiếm nhanh qua tên GameObject
        if (endingTypeText == null)
        {
            var go = GameObject.Find("EndingTypeText");
            if (go != null) endingTypeText = go.GetComponent<TMP_Text>();
        }

        if (titleText == null)
        {
            var go = GameObject.Find("EndingTitleText");
            if (go != null) titleText = go.GetComponent<TMP_Text>();
        }

        if (descriptionText == null)
        {
            var go = GameObject.Find("EndingDescriptionText");
            if (go != null) descriptionText = go.GetComponent<TMP_Text>();
        }

        if (endingImage == null)
        {
            var go = GameObject.Find("EndingImage");
            if (go != null) endingImage = go.GetComponent<Image>();
        }

        if (continueButton == null)
        {
            var actionSlot = GameObject.Find("ActionButtonSlot");
            if (actionSlot != null)
            {
                continueButton = actionSlot.GetComponentInChildren<UIButtonView>(true);
            }

            if (continueButton == null)
            {
#if UNITY_2023_1_OR_NEWER
                continueButton = UnityEngine.Object.FindFirstObjectByType<UIButtonView>(FindObjectsInactive.Include);
#else
                continueButton = UnityEngine.Object.FindObjectOfType<UIButtonView>(true);
#endif
            }
        }

        // 2. Quét sâu trong Scene nếu chưa tìm thấy (hỗ trợ cả khi object inactive)
        if (endingTypeText == null || titleText == null || descriptionText == null || endingImage == null || continueButton == null)
        {
            var activeScene = SceneManager.GetActiveScene();
            var roots = activeScene.GetRootGameObjects();
            foreach (var root in roots)
            {
                var texts = root.GetComponentsInChildren<TMP_Text>(true);
                foreach (var t in texts)
                {
                    if (endingTypeText == null && t.gameObject.name.Equals("EndingTypeText", StringComparison.OrdinalIgnoreCase))
                        endingTypeText = t;
                    else if (titleText == null && t.gameObject.name.Equals("EndingTitleText", StringComparison.OrdinalIgnoreCase))
                        titleText = t;
                    else if (descriptionText == null && t.gameObject.name.Equals("EndingDescriptionText", StringComparison.OrdinalIgnoreCase))
                        descriptionText = t;
                }

                if (endingImage == null)
                {
                    var images = root.GetComponentsInChildren<Image>(true);
                    foreach (var img in images)
                    {
                        if (img.gameObject.name.Equals("EndingImage", StringComparison.OrdinalIgnoreCase))
                        {
                            endingImage = img;
                            break;
                        }
                    }
                }

                if (continueButton == null)
                {
                    continueButton = root.GetComponentInChildren<UIButtonView>(true);
                }
            }
        }
    }

    public void Render(EndingData ending)
    {
        if (ending == null)
            throw new ArgumentNullException(nameof(ending));

        AutoFindUIComponents();

        // 1. Cập nhật nhãn loại Ending
        if (endingTypeText != null)
        {
            string typeStr = !string.IsNullOrEmpty(ending.endingType) ? ending.endingType.ToUpper() : "UNKNOWN";
            endingTypeText.text = typeStr.EndsWith("END") || typeStr.EndsWith("ENDING")
                ? typeStr
                : $"{typeStr} END";

            // Đổi màu hiển thị tương ứng với loại kết cục
            if (ending.endingId == EndingId.DebtEnding)
            {
                endingTypeText.color = new Color(0.95f, 0.4f, 0.25f, 1f); // Cam đỏ nợ nần
            }
            else if (ending.endingId == EndingId.DeathEnding)
            {
                endingTypeText.color = new Color(0.9f, 0.2f, 0.2f, 1f); // Đỏ thẫm cái chết
            }
            else
            {
                endingTypeText.color = new Color(0.45f, 0.84f, 0.91f, 1f); // Xanh nhạt
            }
        }

        // 2. Cập nhật Tiêu đề Ending
        if (titleText != null)
        {
            titleText.text = ending.endingTitle;
            if (ending.endingId == EndingId.DebtEnding)
            {
                titleText.color = new Color(1f, 0.5f, 0.3f, 1f);
            }
            else if (ending.endingId == EndingId.DeathEnding)
            {
                titleText.color = new Color(1f, 0.3f, 0.3f, 1f);
            }
            else
            {
                titleText.color = Color.white;
            }
        }

        // 3. Cập nhật Mô tả cốt truyện
        if (descriptionText != null)
        {
            descriptionText.text = ending.description;
        }

        // 4. Cập nhật Hình ảnh Ending (nếu có)
        if (endingImage != null && ending.endingImage != null)
        {
            endingImage.sprite = ending.endingImage;
            endingImage.gameObject.SetActive(true);
        }

        // Đảm bảo nút tiếp tục đã đăng ký click
        if (continueButton != null)
        {
            continueButton.Clicked -= HandleButtonClicked;
            continueButton.Clicked += HandleButtonClicked;
        }

        Debug.Log($"<color=magenta>[EndingView] Đã cập nhật thành công giao diện Ending: Type='{ending.endingType}', Title='{ending.endingTitle}'</color>");
    }

    private void HandleButtonClicked()
    {
        ContinueClicked?.Invoke();
    }
}
