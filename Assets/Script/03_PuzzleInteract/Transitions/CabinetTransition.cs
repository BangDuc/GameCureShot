using DG.Tweening;
using UnityEngine;

public class CabinetTransition : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer closedCabinet;
    [SerializeField] private SpriteRenderer openedCabinet;

    [Header("Tween Settings")]
    [SerializeField] private float duration = 0.35f;
    [Header("State")]
    [SerializeField] private bool isOpen = false;

    private Sequence transitionSequence;

    private void Start()
    {
        // Khởi tạo trạng thái ban đầu mà không chạy animation
        SetInitialState(isOpen);
    }

    private void SetInitialState(bool open)
    {
        isOpen = open;

        // Đảm bảo alpha và trạng thái hiển thị đúng lúc vào game
        Color cClosed = closedCabinet.color;
        cClosed.a = open ? 0f : 1f;
        closedCabinet.color = cClosed;
        closedCabinet.gameObject.SetActive(!open);

        Color cOpen = openedCabinet.color;
        cOpen.a = open ? 1f : 0f;
        openedCabinet.color = cOpen;
        openedCabinet.gameObject.SetActive(open);
    }

    /// <summary>
    /// Gọi hàm này khi người chơi click/tương tác vào tủ
    /// </summary>
    /// 
    [ContextMenu("Toggle")]
    public void Toggle()
    {
        if (isOpen)
        {
            CloseCabinet();
        }
        else
        {
            OpenCabinet();
        }
    }

    public void OpenCabinet()
    {
        if (isOpen) return;
        isOpen = true;

        PlayTransition(
            fromRenderer: closedCabinet,
            toRenderer: openedCabinet,
            popScale: new Vector3(1f, 1f, 1f) // Bẹp nhẹ bề dọc, nở ngang lúc bung chốt
        );
    }

    public void CloseCabinet()
    {
        if (!isOpen) return;
        isOpen = false;

        PlayTransition(
            fromRenderer: openedCabinet,
            toRenderer: closedCabinet,
            popScale: new Vector3(1f, 1f, 1f) // Nảy nhẹ khi đóng sập lại
        );
    }

    private void PlayTransition(SpriteRenderer fromRenderer, SpriteRenderer toRenderer, Vector3 popScale)
    {
        // 1. Dọn dẹp tween cũ nếu người chơi click liên tục
        transitionSequence?.Kill();
        transitionSequence = DOTween.Sequence();

        // 2. Kích hoạt cả hai GameObject để bắt đầu hòa trộn alpha
        fromRenderer.gameObject.SetActive(true);
        toRenderer.gameObject.SetActive(true);

        toRenderer.color = new Color(toRenderer.color.r, toRenderer.color.g, toRenderer.color.b, 0f);

        // 3. Chuỗi chuyển động kết hợp:
        // - Tạo độ nảy cơ học (Punch/Scale) cho khung tủ
        transitionSequence.Append(transform.DOScale(popScale, duration * 0.4f).SetEase(Ease.OutQuad));

        // - Hòa trộn đồng thời Fade Out layer cũ và Fade In layer mới
        transitionSequence.Join(fromRenderer.DOFade(0f, duration * 0.2f).SetEase(Ease.OutSine));
        transitionSequence.Join(toRenderer.DOFade(1, duration).SetEase(Ease.InSine));

        // - Trả scale tổng thể về lại kích thước chuẩn ban đầu (1, 1, 1)
        transitionSequence.Append(transform.DOScale(Vector3.one, duration * 0.6f).SetEase(Ease.OutBack));

        // 4. Khi kết thúc, tắt hẳn layer cũ để tối ưu render/click
        transitionSequence.OnComplete(() =>
        {
            fromRenderer.gameObject.SetActive(false);
        });

        // Tự động giải phóng khi object bị hủy
        transitionSequence.SetLink(gameObject);
    }

    private void OnDestroy()
    {
        transitionSequence?.Kill();
    }
}