using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraEdgeScroll : MonoBehaviour
{
    [Header("Tốc độ & Vùng mép chuột")]
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float edgeBorder = 50f;
    [SerializeField] private float smoothTime = 0.2f;

    [Header("Giới hạn bối cảnh (Background Bounds)")]
    [SerializeField] private SpriteRenderer backgroundRenderer;

    private CinemachineCamera cam;
    private Vector3 currentVelocity;
    private float minX, maxX, minY, maxY;

    private void Awake()
    {
        cam = GetComponent<CinemachineCamera>();
        CalculateBounds();
    }

    // Đổi sang Update hoặc gọi lại khi thay đổi độ phân giải màn hình
    private void Start()
    {
        CalculateBounds();
    }

    public void CalculateBounds()
    {
        // Sử dụng Camera.main để lấy tỉ lệ màn hình chuẩn (aspect) thay vì cam.aspect
        if (backgroundRenderer == null || cam == null || Camera.main == null) return;

        Bounds bgBounds = backgroundRenderer.bounds;

        float camHalfHeight = cam.Lens.OrthographicSize;
        float camHalfWidth = camHalfHeight * Camera.main.aspect; // ĐÃ SỬA Ở ĐÂY

        minX = bgBounds.min.x + camHalfWidth;
        maxX = bgBounds.max.x - camHalfWidth;
        minY = bgBounds.min.y + camHalfHeight;
        maxY = bgBounds.max.y - camHalfHeight;

        // Nếu kích thước camera lớn hơn background, giữ tâm camera ở giữa background
        if (minX > maxX) minX = maxX = bgBounds.center.x;
        if (minY > maxY) minY = maxY = bgBounds.center.y;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        Vector3 moveDir = Vector3.zero;
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // Kiểm tra mép trái / phải
        if (mousePos.x <= edgeBorder && mousePos.x >= 0)
            moveDir.x = -1f;
        else if (mousePos.x >= Screen.width - edgeBorder && mousePos.x <= Screen.width)
            moveDir.x = 1f;

        // Kiểm tra mép dưới / trên
        if (mousePos.y <= edgeBorder && mousePos.y >= 0)
            moveDir.y = -1f;
        else if (mousePos.y >= Screen.height - edgeBorder && mousePos.y <= Screen.height)
            moveDir.y = 1f;

        // Tính toán vị trí mong muốn tiếp theo
        Vector3 targetPosition = transform.position + moveDir.normalized * (panSpeed * Time.deltaTime);

        // Kẹp (Clamp) tọa độ để không vượt qua rìa background
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        targetPosition.z = transform.position.z;

        // Di chuyển mượt mà Cinemachine Camera bằng SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }

    // Cập nhật lại giới hạn nếu người chơi thay đổi độ phân giải cửa sổ game khi đang chạy
    private void OnRectTransformDimensionsChange()
    {
        CalculateBounds();
    }
}
