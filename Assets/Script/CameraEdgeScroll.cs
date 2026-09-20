using UnityEngine;
using UnityEngine.InputSystem; // Thêm namespace này ở đầu file

public class CameraEdgeScroll : MonoBehaviour
{
    [Header("Tốc độ & Vùng mép chuột")]
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float edgeBorder = 50f;
    [SerializeField] private float smoothTime = 0.2f;

    [Header("Giới hạn bối cảnh (Background Bounds)")]
    [SerializeField] private SpriteRenderer backgroundRenderer;

    private Camera cam;
    private Vector3 currentVelocity;
    private float minX, maxX, minY, maxY;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        CalculateBounds();
    }

    public void CalculateBounds()
    {
        if (backgroundRenderer == null || cam == null) return;

        Bounds bgBounds = backgroundRenderer.bounds;

        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        minX = bgBounds.min.x + camHalfWidth;
        maxX = bgBounds.max.x - camHalfWidth;
        minY = bgBounds.min.y + camHalfHeight;
        maxY = bgBounds.max.y - camHalfHeight;

        if (minX > maxX) minX = maxX = bgBounds.center.x;
        if (minY > maxY) minY = maxY = bgBounds.center.y;
    }

    private void Update()
    {
        // Kiểm tra xem chuột có kết nối không
        if (Mouse.current == null) return;

        Vector3 moveDir = Vector3.zero;

        // Đọc tọa độ chuột theo New Input System
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

        Vector3 targetPosition = transform.position + moveDir.normalized * (panSpeed * Time.deltaTime);

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        targetPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}