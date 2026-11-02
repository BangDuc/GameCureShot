using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HighQualityCapture : MonoBehaviour
{
    [SerializeField] private Image previewUI;

    public void TriggerCapture()
    {
        StartCoroutine(CaptureRoutine());
    }

    private IEnumerator CaptureRoutine()
    {
        // Phải đợi đến cuối frame để đảm bảo mọi hiệu ứng ánh sáng, UI, post-processing đã vẽ xong
        yield return new WaitForEndOfFrame();

        // Chụp toàn bộ màn hình game đang hiển thị
        Texture2D screenTexture = ScreenCapture.CaptureScreenshotAsTexture();

        // Tạo Sprite từ Texture (đã bao gồm đầy đủ ánh sáng và filter trên màn hình)
        Sprite photoSprite = Sprite.Create(
            screenTexture,
            new Rect(0, 0, screenTexture.width, screenTexture.height),
            new Vector2(0.5f, 0.5f)
        );

        if (previewUI != null)
        {
            previewUI.sprite = photoSprite;
        }

        Debug.Log("Đã chụp ảnh đầy đủ hiệu ứng ánh sáng & filter!");
    }
}