using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBorderUI : MonoBehaviour
{
    public Image image;
    public Color currentColor;
    public Color alertColor = Color.red;

    private Coroutine alertCoroutine;


    private void OnEnable()
    {
        // Ví dụ tên event bạn đã đăng ký:
        EventBus.Subscribe("OnPlayerGetHurt", ChangeColorAlert);
    }

    private void OnDisable()
    {
        // Bắt buộc hủy đăng ký khi GameObject bị ẩn hoặc chuẩn bị Destroy
        EventBus.Unsubscribe("OnPlayerGetHurt", ChangeColorAlert);

        // Dừng coroutine và dọn dẹp nếu đang chạy dở
        if (alertCoroutine != null)
        {
            StopCoroutine(alertCoroutine);
            alertCoroutine = null;
        }
    }
    void Start()
    {
        if (image != null)
        {
            currentColor = image.color;
        }
    }

    public void ChangeColorAlert()
    {
        // Nếu đang nháy mà nhận thêm sát thương, reset Coroutine cũ để tránh lỗi đè màu
        if (alertCoroutine != null)
        {
            StopCoroutine(alertCoroutine);
        }

        alertCoroutine = StartCoroutine(FlashRoutine(0.5f));
    }

    // Hiệu ứng chuyển mượt: 0.2s đổi sang alertColor, giữ ngắn, rồi về currentColor
    private IEnumerator FlashRoutine(float totalDuration)
    {
        float halfDuration = totalDuration / 2f;
        float elapsed = 0f;

        // 1. Chuyển từ màu hiện tại sang màu cảnh báo
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            image.color = Color.Lerp(currentColor, alertColor, elapsed / halfDuration);
            yield return null;
        }

        elapsed = 0f;

        // 2. Chuyển từ màu cảnh báo về lại màu ban đầu
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            image.color = Color.Lerp(alertColor, currentColor, elapsed / halfDuration);
            yield return null;
        }

        // Đảm bảo kết thúc đúng chính xác màu gốc
        image.color = currentColor;
        alertCoroutine = null;
    }
}