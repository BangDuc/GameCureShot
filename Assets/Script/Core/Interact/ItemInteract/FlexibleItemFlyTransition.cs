using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class FlexibleItemFlyTransition : MonoBehaviour
{
    [Header("1. Thiết Lập Vị Trí (Kéo thả Transform)")]
    [Tooltip("Điểm xuất hiện bắt đầu")]
    [SerializeField] private Transform startPoint;

    [Tooltip("Điểm đích đến (Slot/Ổ khóa)")]
    [SerializeField] private Transform targetPoint;

    [Header("2. Cài Đặt Chuyển Động (Tween Settings)")]
    [SerializeField] private float flyDuration = 0.5f;
    [SerializeField] private float jumpPower = 1.2f; // Độ cong vòng cung khi bay (đặt = 0 nếu muốn bay thẳng)
    [SerializeField] private Ease flyEase = Ease.InQuad;

    [Header("3. Hiệu Ứng Nảy (Impact Punch)")]
    [SerializeField] private Vector3 impactPunchScale = new Vector3(0.2f, 0.2f, 0f);
    [SerializeField] private float impactDuration = 0.25f;

    [Header("4. Sự Kiện Khi Hoàn Tất")]
    public UnityEvent onUsedSuccessfully;

    private Sequence flySequence;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Gọi hàm này không cần tham số để kích hoạt bay từ startPoint đến targetPoint
    /// </summary>
    [ContextMenu("Test")]
    public void PlayTransition()
    {
        if (startPoint == null || targetPoint == null)
        {
            Debug.LogError($"[FlexibleItemFlyTransition] Chưa gán startPoint hoặc targetPoint trên {gameObject.name}");
            return;
        }

        // Hủy sequence cũ nếu đang chạy dở
        flySequence?.Kill();
        flySequence = DOTween.Sequence();

        // 1. Đặt vật thể vào vị trí bắt đầu
        gameObject.SetActive(true);
        transform.position = startPoint.position;
        transform.localScale = Vector3.zero;

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0f;
            spriteRenderer.color = c;
        }

        // 2. Hiện hình tại điểm xuất phát (Pop-in)
        flySequence.Append(transform.DOScale(targetPoint.localScale * 1.15f, 0.15f).SetEase(Ease.OutBack));
        if (spriteRenderer != null)
        {
            flySequence.Join(spriteRenderer.DOFade(1f, 0.15f));
        }

        // 3. Bay từ Start -> Target (Hỗ trợ nhảy cong hoặc bay thẳng)
        if (jumpPower > 0.01f)
        {
            flySequence.Append(
                transform.DOJump(targetPoint.position, jumpPower, 1, flyDuration).SetEase(flyEase)
            );
        }
        else
        {
            flySequence.Append(
                transform.DOMove(targetPoint.position, flyDuration).SetEase(flyEase)
            );
        }

        // Tự động xoay và co dãn theo đúng targetPoint
        flySequence.Join(transform.DORotate(targetPoint.eulerAngles, flyDuration).SetEase(Ease.OutSine));
        flySequence.Join(transform.DOScale(targetPoint.localScale, flyDuration));

        // 4. Nảy nhẹ khi khớp vào đích
        flySequence.Append(
            transform.DOPunchScale(impactPunchScale, impactDuration, vibrato: 6, elasticity: 0.5f)
        );

        // 5. Kết thúc & Kích hoạt sự kiện
        flySequence.OnComplete(() =>
        {
            transform.position = targetPoint.position;
            transform.rotation = targetPoint.rotation;
            transform.localScale = targetPoint.localScale;

            onUsedSuccessfully?.Invoke();
        });

        flySequence.SetLink(gameObject);
    }

    private void OnDestroy()
    {
        flySequence?.Kill();
    }

    // Vẽ mô phỏng trong Scene view để bạn căn chỉnh 2 điểm trực quan
    private void OnDrawGizmosSelected()
    {
        if (startPoint != null && targetPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(startPoint.position, 0.15f); // Điểm bắt đầu

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPoint.position, 0.15f); // Điểm kết thúc

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(startPoint.position, targetPoint.position);
        }
    }
}